using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement
    public float speed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;

    private float h;
    private float v;

    // Aiming
    private Camera cam;
    private Vector2 mousePosition;

    // Health
    private int health;
    [SerializeField]
    private int maxHealth;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(h, v).normalized * speed;

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        GameManager.GameOver();
        // TODO: player death particles? (or just same as enemies maybe)
        Destroy(gameObject);
    }
}
