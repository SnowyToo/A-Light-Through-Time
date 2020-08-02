using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : Enemy
{
    private Transform player;
    [SerializeField]


    void Start()
    {
        player = GameManager.playerObject.transform;
    }

    void Update()
    {

    }

    // Move towards player until within range, at which point strafe/circle around the player
    // Always shoot at the player
}
