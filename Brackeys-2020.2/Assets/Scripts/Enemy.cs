using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject deathParticles;
    [SerializeField]
    private AudioClip[] deathSounds;

    private Animator anim;

    [SerializeField]
    private int collisionDamage;

    [HideInInspector]
    public EnemySpawner.EnemyType type;

    // Attributes
    [HideInInspector]
    public List<EnemyAttribute> attributes = new List<EnemyAttribute>();
    [HideInInspector]
    public Stack<Shield> shields = new Stack<Shield>();
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private GameObject reflect;

    [HideInInspector]
    public bool invincible;

    private readonly EnemyAttribute TIME_WARP = new EnemyAttribute(EnemyAttribute.AttributeType.TIME_ONLY);

    private void Awake()
    {
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

    public virtual void LaserHit() { }

    public void ShieldHit(bool hurt)
    {
        GameManager.CameraShake(0.2f, 0.2f);
        if (hurt)
        {
            shields.Pop();
            anim.SetTrigger("Hit");
            StartCoroutine(Invincibility());
        }
    }

    public IEnumerator Invincibility()
    {
        invincible = true;
        yield return new WaitForSeconds(1.75f);
        invincible = false;
    }

    public void Die(bool remove = true)
    {
        if (remove) GameManager.enemySpawner.RemoveEnemy(type);
        GameManager.EnemyKill(this);
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

            Shield s = Instantiate(reflect, transform.position, Quaternion.identity, transform).GetComponent<Shield>();
            s.transform.localPosition = new Vector3(0f, 0.2f, 0f);
            s.transform.localScale = 1.9f * new Vector2(1, 1);

            s.parent = this;
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
