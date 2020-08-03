using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private ShieldEnemy enemyScript;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Photon"))
        {
            enemyScript.DestroyShield();
            Destroy(gameObject);
        }
    }
}
