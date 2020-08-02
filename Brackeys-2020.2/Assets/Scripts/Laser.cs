using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private int damage;

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject other = col.gameObject;
        if (other.CompareTag("Player"))
        {
            GameManager.player.TakeDamage(damage);
        }
    }
}
