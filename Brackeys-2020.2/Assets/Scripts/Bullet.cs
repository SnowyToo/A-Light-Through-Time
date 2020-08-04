using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public int damage;

    [SerializeField]
    private AudioClip wallHit;
    [SerializeField]
    private GameObject redParticles;
    private GameObject greenParticles;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            GameObject particles = redParticles;
            if (GetComponent<SpriteRenderer>().color == Color.green)
                particles = greenParticles;

            GameManager.SpawnParticles(particles, gameObject);
            GameManager.PlaySound(wallHit, gameObject, 0.8f, 5f);

            Destroy(gameObject);
        }
    }
}
