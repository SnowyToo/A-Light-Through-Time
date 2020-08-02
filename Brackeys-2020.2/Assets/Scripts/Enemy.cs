using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Photon"))
        {
            GameManager.EnemyKill(gameObject.tag);
            // TODO: death particles maybe??
            Destroy(gameObject);
        }
    }
}
