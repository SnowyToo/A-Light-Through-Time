using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public int damage;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
