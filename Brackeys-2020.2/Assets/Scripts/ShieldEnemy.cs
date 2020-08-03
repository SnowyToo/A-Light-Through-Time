using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    private bool hasShield;

    [SerializeField]
    private float speed;

    void Start()
    {
        hasShield = true;
    }
    
    void Update()
    {
        if (hasShield)
        {
            MoveTowardsPlayer();
        }
        else
        {
            MoveAwayFromPlayer();
        }
    }

    void MoveTowardsPlayer()
    {

    }

    void MoveAwayFromPlayer()
    {

    }

    // Has shield: move towards player
    // Without shield: move away from player

    public void DestroyShield()
    {
        hasShield = false;
        // particles?
    }
}
