<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
    int health_;
    SpriteRenderer spriteRenderer_;
    Color defaultColor_;
    Color hitColor_ = new Color( 1.0f, 0.0f, 0.0f );
    float colorTimer_ = 0.0f;
    float hitKnockback_ = 0.0f;
    Vector3 position_;

    // Use this for initialization
    void Start()
    {
        spriteRenderer_ = GetComponent<SpriteRenderer>();
        position_ = transform.position;

        defaultColor_ = spriteRenderer_.color;
        health_ = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if( colorTimer_ > 0.0f )
        {
            colorTimer_ -= 2.0f * Time.deltaTime;
            spriteRenderer_.color = Color.Lerp( defaultColor_, hitColor_, colorTimer_ );
        }
        if( hitKnockback_ > 0.0f )
        {
            hitKnockback_ -= 4.0f * Time.deltaTime;
            transform.position -= new Vector3( 0.0f, 3.5f * Time.deltaTime, 0.0f );
        }
    }

    public bool TakeDamage( int damage )
    {
        colorTimer_ = 1.0f;
        hitKnockback_ = 0.3f;
        transform.position = position_ + new Vector3( 0.0f, hitKnockback_, 0.0f );
        health_ -= damage;
        return health_ > 0;
    }

    public void Attack()
    {
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
    int health_;

    // Use this for initialization
    void Start()
    {
        health_ = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TakeDamage( int damage )
    {
        health_ -= damage;
        return health_ > 0;
    }
}
>>>>>>> origin/master
