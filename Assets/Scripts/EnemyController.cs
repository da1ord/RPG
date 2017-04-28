using System.Collections;
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
