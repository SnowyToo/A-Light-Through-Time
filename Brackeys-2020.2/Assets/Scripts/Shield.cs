using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Enemy parent;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Photon" && !parent.invincible)
        {
            parent.ShieldHit();
            Destroy(gameObject);
        }
    }
}
