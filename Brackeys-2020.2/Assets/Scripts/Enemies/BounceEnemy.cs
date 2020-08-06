using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEnemy : Enemy
{
    [SerializeField]
    private int pieceCount = 3;
    [SerializeField]
    private GameObject piece1;
    [SerializeField]
    private GameObject piece2;

    public override bool PhotonHit()
    {
        if (!base.PhotonHit(false))
            return false;
        
        Explode();

        return true;
    }

    public override void OnPlayField()
    {
        BounceObject bo = GetComponent<BounceObject>();
        bo.continueGoing = false;
        bo.EnableCollider();
    }

    public override bool LaserHit()
    {
        if (!base.LaserHit())
            return false;
        
        Explode();

        return true;
    }

    private void Explode()
    {
        for (int i = 0; i < pieceCount; i ++)
        {
            Instantiate((Random.value < 0.5 ? piece1 : piece2), transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))).GetComponent<Enemy>().BecomeInvincible();
        }
        Die();
    }
}
