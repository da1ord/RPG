using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Player's max health
    int maxHealth_ = 100;
    // Player's actual health
    int health_ = 100;
    
    // Health text
    public Text healthText_;


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

        defaultColor_ = spriteRenderer_.color;
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
            transform.position += new Vector3( 0.0f, 3.5f * Time.deltaTime, 0.0f );
        }
    }
    
    // Player hurt function
    public void TakeDamage( int damage )
    {
        position_ = transform.position;

        // Player is alive
        if( health_ - damage > 0 )
        {
            // Decrease the health, update healt slider and the text
            health_ -= damage;
            healthText_.text = health_.ToString() + "/" + maxHealth_.ToString();

            colorTimer_ = 1.0f;
            hitKnockback_ = 0.3f;
            transform.position = position_ + new Vector3( 0.0f, -hitKnockback_, 0.0f );
        }
        // Player is dead
        else
        {
            // Set health to 0, update healt slider and the text
            health_ = 0;
            healthText_.text = health_.ToString() + "/" + maxHealth_.ToString();

            // Start death routine
            //StartCoroutine( Death() );
        }
    }

    // Player health getter
    public int GetHealth()
    {
        return health_;
    }
}