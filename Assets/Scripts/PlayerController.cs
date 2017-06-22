<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb_;
    CameraController playerCamera_;
    BattleManager bm_;
    Animator anim_;

    // Movement vector
    Vector2 movement_;
    float movementTimer_;
    bool moving_;
    Vector2 destPosition_;
    Vector2 lastPosition_;
    bool inBattle_;

    // Use this for initialization
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        playerCamera_ = GameObject.Find( "Main Camera" ).GetComponent<CameraController>();
        bm_ = GameObject.Find( "Areas" ).GetComponent<BattleManager>();
        anim_ = GetComponent<Animator>();

        movement_ = new Vector2();
        movementTimer_ = 0.0f;
        moving_ = false;
        inBattle_ = false;

        destPosition_ = rb_.position;
        lastPosition_ = rb_.position;
    }
	
    // Update is called once per frame
	void Update()
    {
        if( inBattle_ )
        {
            if( bm_.IsPlayerOnTurn() )
            {
                // Process battle keyboard input
                ProcessBattleKeyboard();
            }

            return;
        }

        // Decrease movement timer
        movementTimer_ -= Time.deltaTime;
        
        // Move rigidbody
        rb_.MovePosition( rb_.position + ( destPosition_ - lastPosition_ ) * 2 * Time.deltaTime );
        
        // Set proper animation
        anim_.SetFloat( "Horizontal", movement_.x );
        anim_.SetFloat( "Vertical", movement_.y );

        // Wait for player to finish his movement
        if( moving_ && Vector2.SqrMagnitude( destPosition_  - rb_.position ) > 0.001f )
        {
            return;
        }

        // Set precise position
        rb_.position *= 2.0f;
        rb_.position = new Vector2( Mathf.Round( rb_.position.x ), Mathf.Round( rb_.position.y ) );
        rb_.position /= 2.0f;

        // Update last position to actual
        lastPosition_ = destPosition_;

        // Process map keyboard input
        ProcessMapKeyboard();

        // Check if player wants to move
        if( moving_ )
        {
            // Test if player can move in the desired direction
            RaycastHit2D ray = Physics2D.Raycast( rb_.position, movement_, 1.0f, LayerMask.GetMask( "Collider", "Enemy" ) );
            // No collider in player's way. Player can move
            if( ray.collider == null )
            {
                // Save last position
                lastPosition_  = rb_.position;
                // Set destination position
                destPosition_ = rb_.position + movement_;
                // Set move timer
                movementTimer_ = 2.0f;
            }
            // Collider in player's way
            else
            {
                if( ray.collider.tag == "Enemy" )
                {
                    // Set battle mode and battle camera
                    inBattle_ = true;

                    // Set battle animation
                    anim_.SetFloat( "Horizontal", 0 );
                    anim_.SetFloat( "Vertical", 1 );

                    // Start battle. Pass enemy name on map
                    bm_.StartBattle( ray.collider.name );
                }
                // Clear moving bool and movement vector
                moving_ = false;
                movement_ = Vector2.zero;
            }
        }
    }

    void ProcessBattleKeyboard()
    {
        if( Input.GetKeyDown( KeyCode.W ) )
        {
            bm_.SelectMoveUp();
        }
        else if( Input.GetKeyDown( KeyCode.S ) )
        {
            bm_.SelectMoveDown();
        }
        else if( Input.GetKeyDown( KeyCode.A ) )
        {
            bm_.SelectMoveLeft();
        }
        else if( Input.GetKeyDown( KeyCode.D ) )
        {
            bm_.SelectMoveRight();
        }
        else if( Input.GetKeyDown( KeyCode.Space ) )
        {
            bm_.ConfirmMove();
        }
        else if( Input.GetKeyDown( KeyCode.Backspace ) )
        {
            bm_.CancelMove();
        }
    }

    void ProcessMapKeyboard()
    {
        // Clear movement vector
        movement_ = Vector2.zero;

        // Check movement key press
        if( Input.GetKey( KeyCode.W ) )
        {
            movement_.y = 1;
        }
        else if( Input.GetKey( KeyCode.S ) )
        {
            movement_.y = -1;
        }
        else if( Input.GetKey( KeyCode.A ) )
        {
            movement_.x = -1;
        }
        else if( Input.GetKey( KeyCode.D ) )
        {
            movement_.x = 1;
        }
        //else if( Input.GetKeyDown( KeyCode.Space ) )
        //{
        //    areaInfo_.ChangeArea( "Intro" );
        //}

        // Test if player is moving
        if( movement_.magnitude > 0 )
        {
            moving_ = true;
        }
        else
        {
            moving_ = false;
        }
    }

    private void LateUpdate()
    {
        if( !inBattle_ )
        {
            // Update camera position
            playerCamera_.MovePosition( rb_.position );
        }
    }

    public void BattleEnd()
    {
        inBattle_ = false;
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb_;
    CameraController playerCamera_;
    BattleManager bm_;
    Animator anim_;

    // Movement vector
    Vector2 movement_;
    float movementTimer_;
    bool moving_;
    Vector2 destPosition_;
    Vector2 lastPosition_;
    bool inBattle_;

    // Use this for initialization
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        playerCamera_ = GameObject.Find( "Main Camera" ).GetComponent<CameraController>();
        bm_ = GameObject.Find( "Areas" ).GetComponent<BattleManager>();
        anim_ = GetComponent<Animator>();

        movement_ = new Vector2();
        movementTimer_ = 0.0f;
        moving_ = false;
        inBattle_ = false;

        destPosition_ = rb_.position;
        lastPosition_ = rb_.position;
    }
	
    // Update is called once per frame
	void Update()
    {
        if( inBattle_ )
        {
            if( bm_.IsPlayerOnTurn() )
            {
                // Process battle keyboard input
                ProcessBattleKeyboard();
            }
            return;
        }

        // Decrease movement timer
        movementTimer_ -= Time.deltaTime;
        
        // Move rigidbody
        rb_.MovePosition( rb_.position + ( destPosition_ - lastPosition_ ) * 2 * Time.deltaTime );
        
        // Set proper animation
        anim_.SetFloat( "Horizontal", movement_.x );
        anim_.SetFloat( "Vertical", movement_.y );

        // Wait for player to finish his movement
        if( moving_ && Vector2.SqrMagnitude( destPosition_  - rb_.position ) > 0.001f )
        {
            return;
        }

        // Set precise position
        rb_.position *= 2.0f;
        rb_.position = new Vector2( Mathf.Round( rb_.position.x ), Mathf.Round( rb_.position.y ) );
        rb_.position /= 2.0f;

        // Update last position to actual
        lastPosition_ = destPosition_;

        // Process map keyboard input
        ProcessMapKeyboard();

        // Check if player wants to move
        if( moving_ )
        {
            // Test if player can move in the desired direction
            RaycastHit2D ray = Physics2D.Raycast( rb_.position, movement_, 1.0f, LayerMask.GetMask( "Collider", "Enemy" ) );
            // No collider in player's way. Player can move
            if( ray.collider == null )
            {
                // Save last position
                lastPosition_  = rb_.position;
                // Set destination position
                destPosition_ = rb_.position + movement_;
                // Set move timer
                movementTimer_ = 2.0f;
            }
            // Collider in player's way
            else
            {
                if( ray.collider.tag == "Enemy" )
                {
                    // Set battle mode and battle camera
                    inBattle_ = true;

                    // Set battle animation
                    anim_.SetFloat( "Horizontal", 0 );
                    anim_.SetFloat( "Vertical", 1 );

                    // Start battle. Pass enemy name on map
                    bm_.StartBattle( ray.collider.name );
                }
                // Clear moving bool and movement vector
                moving_ = false;
                movement_ = Vector2.zero;
            }
        }
    }

    void ProcessBattleKeyboard()
    {
        if( Input.GetKeyDown( KeyCode.W ) )
        {
            bm_.SelectMoveUp();
        }
        else if( Input.GetKeyDown( KeyCode.S ) )
        {
            bm_.SelectMoveDown();
        }
        else if( Input.GetKeyDown( KeyCode.A ) )
        {
            bm_.SelectMoveLeft();
        }
        else if( Input.GetKeyDown( KeyCode.D ) )
        {
            bm_.SelectMoveRight();
        }
        else if( Input.GetKeyDown( KeyCode.Space ) )
        {
            bm_.ConfirmMove();
        }
        else if( Input.GetKeyDown( KeyCode.Backspace ) )
        {
            bm_.CancelMove();
        }
    }

    void ProcessMapKeyboard()
    {
        // Clear movement vector
        movement_ = Vector2.zero;

        // Check movement key press
        if( Input.GetKey( KeyCode.W ) )
        {
            movement_.y = 1;
        }
        else if( Input.GetKey( KeyCode.S ) )
        {
            movement_.y = -1;
        }
        else if( Input.GetKey( KeyCode.A ) )
        {
            movement_.x = -1;
        }
        else if( Input.GetKey( KeyCode.D ) )
        {
            movement_.x = 1;
        }
        //else if( Input.GetKeyDown( KeyCode.Space ) )
        //{
        //    areaInfo_.ChangeArea( "Intro" );
        //}

        // Test if player is moving
        if( movement_.magnitude > 0 )
        {
            moving_ = true;
        }
        else
        {
            moving_ = false;
        }
    }

    private void LateUpdate()
    {
        if( !inBattle_ )
        {
            // Update camera position
            playerCamera_.MovePosition( rb_.position );
        }
    }

    public void BattleEnd()
    {
        inBattle_ = false;
    }
}
>>>>>>> b26a507af31452553c716ea355cbdf2c1484bb42
