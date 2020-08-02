using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : Enemy
{
    private Transform player;

    void Start()
    {
        player = GameManager.player.transform;
    }

    // Movement:
    // Move towards player until within range, at which point strafe/circle around the player
}
