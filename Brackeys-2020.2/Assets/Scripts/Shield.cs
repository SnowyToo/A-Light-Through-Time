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

    [SerializeField]
    private bool destroyable = true;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Photon" && !parent.invincible)
        {
            parent.ShieldHit(destroyable);

            GameManager.PlaySound(shieldBoop, gameObject);

            if (destroyable)
            {
                GameManager.SpawnParticles(shieldPop, gameObject);
                Destroy(gameObject);
            }
        }
    }
}
