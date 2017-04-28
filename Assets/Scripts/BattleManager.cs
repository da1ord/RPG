using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    AreaInfo areaInfo_;
    static PlayerController player_;
    PlayerHealth playerHealth_;
    CameraController playerCamera_;
    static Vector2 playerPosition_;
    bool playersTurn_;
    string battleMove_;
    int battleMoveLevel_;
    bool moveConfirmed_;
    bool selectingEnemy_;
    int selectedTarget_;
    int initialEnemyCount_;

    int enemyHealth_;
    string mapEnemyName_;

    Color moveColor_;
    Color highlightedMoveColor_;
    Image attackFrame_;
    Text attackLabel_;
    Image fleeFrame_;
    Text fleeLabel_;
    GameObject enemyMarker_;
    Canvas battleStatusBarCanvas_;
    Canvas battleMovesCanvas_;

    List<GameObject> moves_;
    GameObject highlightedMove_;

    static List<EnemyController> enemies_;

    // Status text
    public Text statusText_;

    // Use this for initialization
    void Start()
    {
        areaInfo_ = GetComponent<AreaInfo>();
        player_ = GameObject.Find( "Player" ).GetComponent<PlayerController>();
        playerHealth_ = GameObject.Find( "Player" ).GetComponent<PlayerHealth>();
        playerCamera_ = GameObject.Find( "Main Camera" ).GetComponent<CameraController>();
        playersTurn_ = true;
        battleMove_ = "Attack";
        battleMoveLevel_ = 0;
        moveConfirmed_ = false;
        selectingEnemy_ = false;
        selectedTarget_ = 0;

        enemyHealth_ = 100;

        moveColor_ = new Color( 1.0f, 1.0f, 1.0f );
        highlightedMoveColor_ = new Color( 1.0f, 1.0f, 0.0f );

        attackFrame_ = GameObject.Find( "BattleMoves/Level0/Battle/Background/Frame" ).GetComponent<Image>();
        attackLabel_ = GameObject.Find( "BattleMoves/Level0/Battle/Label" ).GetComponent<Text>();

        fleeFrame_ = GameObject.Find( "BattleMoves/Level0/Flee/Background/Frame" ).GetComponent<Image>();
        fleeLabel_ = GameObject.Find( "BattleMoves/Level0/Flee/Label" ).GetComponent<Text>();

        enemyMarker_ = GameObject.Find( "EnemyMarker" );
        enemyMarker_.SetActive( false );

        battleStatusBarCanvas_ = GameObject.Find( "BattleStatusBar" ).GetComponent<Canvas>();
        battleMovesCanvas_ = GameObject.Find( "BattleMoves" ).GetComponent<Canvas>();

        moves_ = new List<GameObject>();

        enemies_ = new List<EnemyController>();
    }
	
	// Update is called once per frame
	void Update()
    {
    }

    // Battle round coroutine
    IEnumerator Round()
    {
        // While player is alive and enemies are alive, battle continues
        while( playerHealth_.GetHealth() > 0 && enemies_.Count > 0 ) // playerIsAlive
        {
            //Debug.Log( "Battle Round" );
            //Debug.Log( "Player's turn: " + playersTurn_ );
            //Debug.Log( "Move confirmed: " + moveConfirmed_ );

            // Check if player takes turn now and if he has confirmed the move
            if( playersTurn_ )
            {
                if( moveConfirmed_ )
                {
                    if( highlightedMove_.name == "Flee" )
                    {
                        Debug.Log( "Player fled!" );

                        foreach( EnemyController enemy in enemies_ )
                        {
                            Destroy( enemy.gameObject );
                        }
                        enemies_.Clear();
                        EndBattle();
                        yield return 0;
                    }
                    else if( highlightedMove_.name == "Attack" )
                    {
                        int damage = Random.Range( 50, 70 );
                        Debug.Log( "Player attacked for " + damage + "HP!" );
                        statusText_.text = "Player attacked for " + damage + "HP!";

                        /*enemyHealth_ -= damage;*/

                        if( !enemies_[ selectedTarget_ ].GetComponent<EnemyController>().TakeDamage( damage ) )//enemyHealth_ <= 0 )
                        {
                            Destroy( enemies_[ selectedTarget_ ].gameObject );
                            enemies_.RemoveAt( selectedTarget_ );
                            if( enemies_.Count == 0 )
                            {
                                Debug.Log( "Player win!" );
                                statusText_.text = "Player win!";
                                yield return new WaitForSeconds( 2 );
                                // Find enemy on map and destroy him
                                GameObject enemy = GameObject.Find( "Intro/Enemies/" + mapEnemyName_ );
                                areaInfo_.DestroyEnemy( enemy );

                                EndBattle();
                                yield return 0;
                            }
                        }
                    }

                    GetBattleMoves( 0 );
                    HighlightMove();

                    // Player just had his turn. Now it is enemy's turn
                    playersTurn_ = false;
                    moveConfirmed_ = false;
                }
                else if( selectingEnemy_ )
                {

                }
                yield return 0;
            }
            else
            {   
                yield return new WaitForSeconds( 2 );
                foreach( EnemyController enemy in enemies_ )
                {
                    statusText_.text = enemy.name + " is attacking!";
                    yield return new WaitForSeconds( 1 );
                    statusText_.text = "";
                    playerHealth_.TakeDamage( 1 );
                    yield return new WaitForSeconds( 1 );
                }
                // Every enemy had their turn. Now it is player's turn.
                playersTurn_ = true;
            }
        }
    }

    public void StartBattle( string mapEnemyName )
    {
        GetBattleMoves( 0 );
        HighlightMove();

        // Set battle camera
        playerCamera_.BattleCamera();

        // Backup player's map position
        playerPosition_ = player_.transform.position;
        // Set player's position
        player_.transform.position = new Vector2( -20.0f, -1.0f );

        battleStatusBarCanvas_.enabled = true;
        battleMovesCanvas_.enabled = true;

        // Get enemy name and type
        mapEnemyName_ = mapEnemyName;
        string enemyType = mapEnemyName_.Split( ' ' )[0];
        
        // Generate the number of enemies in battle
        initialEnemyCount_ = Random.Range( 1, 4 );

        for( int i = 0; i < initialEnemyCount_; i++ )
        {
            // Instantiate enemy
            GameObject enemy = (GameObject)Instantiate( Resources.Load( "Prefabs/" + enemyType + "_fight" ) );
            enemy.transform.position = new Vector2( -20.0f + 2 * ( -initialEnemyCount_ + 2 * i + 1 ), 2.0f );
            enemy.transform.localScale = new Vector2( 2.0f, 2.0f );
            enemy.name = enemyType + ( i + 1 ).ToString();

            // Add enemy to the enemies list
            enemies_.Add( enemy.GetComponent<EnemyController>() );
        }

        // TEMP
        enemyHealth_ = 100;

        // Set player's turn
        playersTurn_ = true;

        // Clear status bar text
        statusText_.text = "";

        selectingEnemy_ = false;
        selectedTarget_ = 0;

        // Start round coroutine
        StartCoroutine( Round() );
    }

    public void EndBattle()
    {
        // Set map camera
        playerCamera_.MapCamera();

        player_.transform.position = playerPosition_;

        battleStatusBarCanvas_.enabled = false;
        battleMovesCanvas_.enabled = false;

        // Clear inBattle flag
        player_.BattleEnd();
    }

    // Select upper battle move
    public void SelectMoveUp()
    {
        
    }

    // Select down battle move
    public void SelectMoveDown()
    {
    }

    // Select left battle move
    public void SelectMoveLeft()
    {
        if( selectingEnemy_ )
        {
            selectedTarget_ = --selectedTarget_ >= 0 ? selectedTarget_ : enemies_.Count - 1;
            HighlightEnemy();
        }
        else
        {
            selectedTarget_ = --selectedTarget_ >= 0 ? selectedTarget_ : moves_.Count - 1;
            HighlightMove();
        }
    }

    // Select right battle move
    public void SelectMoveRight()
    {
        if( selectingEnemy_ )
        {
            selectedTarget_ = ++selectedTarget_ % enemies_.Count;
            HighlightEnemy();
        }
        else
        {
            selectedTarget_ = ++selectedTarget_ % moves_.Count;
            HighlightMove();
        }
    }

    // Select battle move
    public void SelectMove()
    {
        if( battleMove_ == "Battle" )
        {
            attackFrame_.color = moveColor_;
            attackLabel_.color = moveColor_;

            battleMove_ = "Flee";
            fleeFrame_.color = highlightedMoveColor_;
            fleeLabel_.color = highlightedMoveColor_;
        }
        else
        {
            fleeFrame_.color = moveColor_;
            fleeLabel_.color = moveColor_;

            battleMove_ = "Battle";
            attackFrame_.color = highlightedMoveColor_;
            attackLabel_.color = highlightedMoveColor_;
        }
    }

    // Confirm battle move
    public void ConfirmMove()
    {
        #if true
        if( selectingEnemy_ )
        {
            selectingEnemy_ = false;
            enemyMarker_.SetActive( false );
            moveConfirmed_ = true;
            return;
        }

        if( battleMoveLevel_ == 0 )
        {
            if( highlightedMove_.name == "Flee" )
            {
                moveConfirmed_ = true;
                return;
            }
            // update moves list
            GetBattleMoves( battleMoveLevel_ + 1 );
            HighlightMove();
        }
        else
        {
            // selecting target state
            selectingEnemy_ = true;
            enemyMarker_.SetActive( true );

            selectedTarget_ = 0;
            HighlightEnemy();
        }
        #endif

        //moveConfirmed_ = true;
    }
    
    // Cancel battle move
    public void CancelMove()
    {
        if( selectingEnemy_ )
        {
            selectingEnemy_ = false;
            return;
        }

        if( battleMoveLevel_ == 1 )
        {
            // update moves list
            GetBattleMoves( battleMoveLevel_ - 1 );
            HighlightMove();
        }
    }

    void HighlightMove()
    {
        // Check if highlighted move was set before
        if( highlightedMove_ != null )
        {
            highlightedMove_.transform.Find( "Background/Frame" ).GetComponent<Image>().color = moveColor_;
            highlightedMove_.transform.Find( "Label" ).GetComponent<Text>().color = moveColor_;
        }

        highlightedMove_ = moves_[selectedTarget_];
        highlightedMove_.transform.Find( "Background/Frame" ).GetComponent<Image>().color = highlightedMoveColor_;
        highlightedMove_.transform.Find( "Label" ).GetComponent<Text>().color = highlightedMoveColor_;
    }

    void HighlightEnemy()
    {
        // Move the enemy marker to selected enemy position
        enemyMarker_.transform.position = enemies_[selectedTarget_].transform.position;
    }

    void GetBattleMoves( int nextLevel )
    {
        // Clear list of moves
        moves_.Clear();

        // Hide previous menu level moves
        GameObject.Find( "BattleMoves/Level" + battleMoveLevel_ ).transform.gameObject.GetComponent<Canvas>().enabled = false;

        battleMoveLevel_ = nextLevel;
        Transform level = GameObject.Find( "BattleMoves/Level" + battleMoveLevel_ ).transform;
        
        // Show actual menu level moves
        level.gameObject.GetComponent<Canvas>().enabled = true;

        foreach( Transform move in level )
        {
            // Add move
            moves_.Add( move.gameObject );
        }

        // Set selected target to first item
        selectedTarget_ = 0;
    }

    public bool IsPlayerOnTurn()
    {
        return playersTurn_;
    }
}
