using System.Collections;
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

        if (invincible) return;

        if (shields.Count > 0) return;

        if (attributes.Contains(TIME_WARP) && !GameManager.isRewinding) return;

        partner.Die(false);
        Destroy(transform.parent.gameObject);
    }
}
