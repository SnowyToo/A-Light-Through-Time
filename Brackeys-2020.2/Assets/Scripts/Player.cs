using System.Collections;
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
    private Vector2 aimDirection;

    // Health
    private int health;
    [SerializeField]
    public int maxHealth;
    [SerializeField]
    private float invincibiltyTime = 2.5f;
    private bool invincible;

    // Misc
    private Animator anim;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private GameObject deathParticles;

    void Awake()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(h, v).normalized * speed;

        aimDirection = GameManager.GetMousePosition() - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    public void TakeDamage(int damage)
    {
        if (invincible)
            return;

        //health -= damage;

        GameManager.uiManager.UpdateHealth(health);

        if (health <= 0)
            Die();
        else
            Hit();
    }

    void Die()
    {
        GameManager.GameOver();
        GameManager.PlaySound(deathSound, gameObject);
        GameManager.SpawnParticles(deathParticles, gameObject);
        GameManager.CameraShake(0.2f, 0.7f);
        Destroy(gameObject);
    }

    void Hit()
    {
        anim.SetTrigger("Hit");
        GameManager.PlaySound(hitSound, gameObject);
        GameManager.CameraShake(0.2f, 0.5f);
        StartCoroutine(Invincibility());
    }

    private IEnumerator Invincibility()
    {
        invincible = true;
        gameObject.layer = 12;
        yield return new WaitForSeconds(invincibiltyTime);
        invincible = false;
        gameObject.layer = 8;
        GetComponent<Collider2D>().enabled = true;
    }
}
