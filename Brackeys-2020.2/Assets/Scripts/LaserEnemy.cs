﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : Enemy
{
    [SerializeField]
    private LaserEnemy partner;

    // Freely move around
    // Kill both enemies when photon hits one

    public override void PhotonHit()
    {
        base.PhotonHit();
        partner.Die();
        Destroy(transform.parent.gameObject);
    }
}
