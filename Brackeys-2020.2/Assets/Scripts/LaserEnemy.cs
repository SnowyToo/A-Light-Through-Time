using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : Enemy
{
    [SerializeField]
    private LaserEnemy partner;

    [SerializeField]
    private Sprite sadSprite;

    // Freely move around
    // Kill both enemies when photon hits one

    void Start()
    {
        type = EnemySpawner.EnemyType.LaserEnemy;
    }

    public override void PhotonHit()
    {
        base.PhotonHit();

        if (invincible) return;

        if (shields.Count > 0) return;

        if (attributes.Contains(TIME_WARP) && !GameManager.isRewinding) return;

        partner.Die(false, false);
        Destroy(transform.parent.gameObject);
    }

    public void Sadden()
    {
        GetComponent<SpriteRenderer>().sprite = sadSprite;
    }
}
