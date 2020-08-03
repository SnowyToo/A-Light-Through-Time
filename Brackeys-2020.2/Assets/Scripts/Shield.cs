using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [HideInInspector]
    public Enemy parent;

    [SerializeField]
    GameObject shieldPop;
    [SerializeField]
    AudioClip shieldBoop;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Photon" && !parent.invincible)
        {
            parent.ShieldHit();

            GameManager.PlaySound(shieldBoop, gameObject);
            GameManager.SpawnParticles(shieldPop, gameObject);

            Destroy(gameObject);
        }
    }
}
