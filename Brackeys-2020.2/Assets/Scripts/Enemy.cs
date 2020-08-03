<<<<<<< Updated upstream
﻿using UnityEngine;
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
>>>>>>> Stashed changes

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject deathParticles;
    [SerializeField]
    private AudioClip[] deathSounds;

<<<<<<< Updated upstream
=======
    private List<EnemyAttribute> attributes = new List<EnemyAttribute>();
    private Stack<Shield> shields = new Stack<Shield>();
    [SerializeField]
    private GameObject shield;
    public bool invincible;

    private readonly EnemyAttribute TIME_WARP = new EnemyAttribute(EnemyAttribute.AttributeType.TIME_ONLY);
    private readonly EnemyAttribute SHIELD = new EnemyAttribute(EnemyAttribute.AttributeType.SHIELD);

    private Animator anim;

>>>>>>> Stashed changes
    [SerializeField]
    private int collisionDamage;

    private void Awake()
    {
        Addtribute(new EnemyAttribute(EnemyAttribute.AttributeType.TIME_ONLY));
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Photon"))
        {
            PhotonHit();
        }
        else if (other.CompareTag("Player"))
        {
            PlayerHit();
        }
        else if (other.CompareTag("Laser"))
        {
            LaserHit();
        }
    }

    public virtual void PhotonHit()
    {
<<<<<<< Updated upstream
=======
        if (invincible) return;

        if (attributes.Contains(TIME_WARP) && !GameManager.isRewinding) return;

        if(shields.Count > 0) return;

>>>>>>> Stashed changes
        Die();
    }

    public virtual void PlayerHit()
    {
        GameManager.player.TakeDamage(collisionDamage);
    }

    public virtual void LaserHit()
    {

    }

    public void ShieldHit()
    {
        shields.Pop();
        //Particles?
        anim.SetTrigger("Hit");
        StartCoroutine(Invincibility());
    }

    public IEnumerator Invincibility()
    {
        invincible = true;
        yield return new WaitForSeconds(1.75f);
        invincible = false;
    }

    public void Die()
    {
        GameManager.EnemyKill(gameObject.tag);
        GameManager.PlaySound(deathSounds, this.gameObject);
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Addtribute(EnemyAttribute attribute)
    {
        attributes.Add(attribute);

        if (attribute.type == EnemyAttribute.AttributeType.SHIELD)
        {
            for(int i = 0; i < attribute.amount; i++)
            {
                Shield s = Instantiate(shield, transform.position, Quaternion.identity, transform).GetComponent<Shield>();

                s.transform.localScale = (1.6f + 0.3f * i) * new Vector2(1, 1);
                s.transform.localEulerAngles = Vector3.forward * 20f * i;
                
                s.parent = this;
                shields.Push(s);
            }
        }

        if(attribute.type == EnemyAttribute.AttributeType.TIME_ONLY)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }

        if(attribute.type == EnemyAttribute.AttributeType.REFLECT)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
<<<<<<< Updated upstream
=======

[System.Serializable]
public struct EnemyAttribute
{
    public enum AttributeType { SHIELD, TIME_ONLY, REFLECT }
    public AttributeType type;

    public int amount;

    public EnemyAttribute(AttributeType _type, int _amount = 0)
    {
        type = _type;
        amount = _amount;
    }

    public bool Equals(EnemyAttribute other)
    {
        return type == other.type;
    }
}
>>>>>>> Stashed changes
