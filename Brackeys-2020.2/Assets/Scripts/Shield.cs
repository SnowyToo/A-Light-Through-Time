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

    [SerializeField]
    private GameObject warpSprite; //I will keep using time warp out of spite now.
    private GameObject curSprite;

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
        {
            if (curSprite == null)
                curSprite = Instantiate(warpSprite, transform.position, Quaternion.identity, transform);
            return;
        }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(GameManager.isRewinding)
        {
            if (other.tag == "Photon" && !parent.invincible)
            {
                Hit();
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Photon" && !parent.invincible)
        {
            Hit();
        }
    }
}
