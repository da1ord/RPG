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

    // Use this for initialization
    void Start()
    {	
	}
	
	// Update is called once per frame
	void Update()
    {
    }
    
    // Player hurt function
    public void TakeDamage( int damage )
    {
        // Player is alive
        if( health_ - damage > 0 )
        {
            // Decrease the health, update healt slider and the text
            health_ -= damage;
            healthText_.text = health_.ToString() + "/" + maxHealth_.ToString();

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
