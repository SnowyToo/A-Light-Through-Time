using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject deathParticles;
    [SerializeField]
    private AudioClip[] deathSounds;

    private List<EnemyAttribute> attributes = new List<EnemyAttribute>();
    private Stack<Shield> shields = new Stack<Shield>();
    [SerializeField]
    private GameObject shield;
    public bool invincible;

    private readonly EnemyAttribute TIME_WARP = new EnemyAttribute(EnemyAttribute.AttributeType.TIME_ONLY);
    private readonly EnemyAttribute SHIELD = new EnemyAttribute(EnemyAttribute.AttributeType.SHIELD);

    private Animator anim;

    [SerializeField]
    private int collisionDamage;

    private void Awake()
    {
        //Addtribute(new EnemyAttribute(EnemyAttribute.AttributeType.TIME_ONLY));
        Addtribute(new EnemyAttribute(EnemyAttribute.AttributeType.SHIELD, 2));
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
        if (invincible) return;

        if (attributes.Contains(TIME_WARP) && !GameManager.isRewinding) return;

        if(shields.Count > 0) return;
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
        GameManager.CameraShake(0.2f, 0.2f);
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
        GameManager.SpawnParticles(deathParticles, gameObject);
        GameManager.CameraShake(0.2f, 0.3f);
        Destroy(gameObject);
    }

    public void Addtribute(EnemyAttribute attribute)
    {
        if (attribute.type == EnemyAttribute.AttributeType.SHIELD && attribute.amount == 0) return;
        
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
