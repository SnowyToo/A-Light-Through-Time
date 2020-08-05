using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [HideInInspector]
    public Enemy parent;

    [HideInInspector]
    public bool timeOnly;

    [SerializeField]
    GameObject shieldPop;
    [SerializeField]
    GameObject greenShieldPop;

    private GameObject shieldPart;

    [SerializeField]
    AudioClip shieldBoop;

    [SerializeField]
    private bool destroyable = true;

    private void Awake()
    {
        shieldPart = shieldPop;
    }

    private void FixedUpdate()
    {
        if(!destroyable && GameManager.player != null)
        {
            Vector2 aimDirection = GameManager.player.transform.position - transform.position;
            transform.up = aimDirection;
        }
    }

    public void Hit()
    {
        if (timeOnly && !GameManager.isRewinding)
            return;

        parent.ShieldHit(destroyable);

        GameManager.PlaySound(shieldBoop, gameObject);

        if (destroyable)
        {
            GameManager.SpawnParticles(shieldPart, gameObject);
            Destroy(gameObject);
        }
    }

    public void SetTimeOnly()
    {
        timeOnly = true;
        GetComponent<SpriteRenderer>().color = Color.green;
        shieldPart = greenShieldPop;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Photon" && !parent.invincible)
        {
            Hit();
        }
    }
}
