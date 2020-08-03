using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject deathParticles;
    [SerializeField]
    private AudioClip[] deathSounds;

    [SerializeField]
    private List<EnemyAttribute> attributes = new List<EnemyAttribute>();

    private readonly EnemyAttribute TIME_WARP = new EnemyAttribute(EnemyAttribute.AttributeType.TIME_ONLY);
    private readonly EnemyAttribute SHIELD = new EnemyAttribute(EnemyAttribute.AttributeType.SHIELD);

    [SerializeField]
    private int collisionDamage;

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
        if (attributes.Contains(TIME_WARP) && !GameManager.isRewinding) return;

        Die();
    }

    public virtual void PlayerHit()
    {
        GameManager.player.TakeDamage(collisionDamage);
    }

    public virtual void LaserHit()
    {

    }

    public void Die()
    {
        GameManager.EnemyKill(gameObject.tag);
        GameManager.PlaySound(deathSounds, this.gameObject);
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

[System.Serializable]
public struct EnemyAttribute
{
    public enum AttributeType { SHIELD, TIME_ONLY }
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