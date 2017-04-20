using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaInfo : MonoBehaviour
{
    string areaName_;
    List<GameObject> enemiesList_;
    SpriteRenderer battleBackground_;
    bool respawnableArea_;

	// Use this for initialization
	void Start()
    {
        respawnableArea_ = false;
        
        // Get battle background image component
        battleBackground_ = GameObject.Find( "BattleBackgroundImage" ).GetComponent<SpriteRenderer>();

        // Set initial area
        ChangeArea( "Intro" );
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Change area on transition
    public void ChangeArea( string name )
    {
        Debug.Log( "Area changed." );
        areaName_ = name;

        battleBackground_.sprite = Resources.Load<Sprite>( "Battlegrounds/" + name );

        Transform enemies = transform.Find( name + "/Enemies" );
        enemiesList_ = new List<GameObject>();

        foreach( Transform child in enemies )
        {
            if( child.CompareTag( "Enemy" ) )
            {
                Debug.Log( child.name + " added." );
                enemiesList_.Add( child.gameObject );
                child.gameObject.SetActive( true );
            }
        }
    }

    public void DestroyEnemy( GameObject enemy )
    {
        // Check if enemies respawn on area enter
        if( respawnableArea_ )
        {
            // If enemies respawn, hide them
            enemy.SetActive( false );
        }
        else
        {
            // If enemies don't respawn, destroy them
            Destroy( enemy );
            enemiesList_.Remove( enemy );
        }
    }

    public string GetAreaName()
    {
        return areaName_;
    }

    public bool RespawnableArea()
    {
        return respawnableArea_;
    }
}
