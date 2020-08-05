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

    public override bool PhotonHit()
    {
        if (!base.PhotonHit())
            return false;

        partner.Die(false, false);
        Destroy(transform.parent.gameObject);
        return true;
    }

    public void Sadden()
    {
        GetComponent<SpriteRenderer>().sprite = sadSprite;
    }
}
