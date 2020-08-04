using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : Enemy
{
    // Movement
    private Rigidbody2D player;
    private Rigidbody2D rb;
    private Vector2 target;
    private Vector2 repulsionForce;
    [SerializeField]
    private float range = 2f;
    [SerializeField]
    private float speed = 3f;

    // Shooting
    [SerializeField]
    private float timeBetweenShots = 1f;
    private float nextShot;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform shotPoint;
    [SerializeField]
    private int bulletDamage;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private AudioClip[] shootSounds;

    private void Start()
    {
        player = GameManager.playerObject.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();

        nextShot = timeBetweenShots;
    }

    void Update()
    {
        if (GameManager.gameIsOver)
        {
            EndGame();
            return;
        }

        if (nextShot <= 0f)
            Shoot();
        else
            nextShot -= Time.deltaTime;
    }

    // Move towards player until within range, at which point strafe/circle around the player
    // Always shoot at the player

    void FixedUpdate()
    {
        List<Collider2D> enemies = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;

        bool enemyClose = false;

        Physics2D.OverlapCircle(transform.position, 4, filter, enemies);

        repulsionForce = Vector2.zero;

        foreach(Collider2D c in enemies)
        {
            if(c.gameObject != this.gameObject && c.gameObject.layer == 10)
            {
                repulsionForce += (Vector2)(c.transform.position - transform.position).normalized * 1f/Vector2.Distance(c.transform.position, transform.position);
                enemyClose = true;
            }
        }

        target = player.position - rb.position - repulsionForce;

        float distance = Vector2.Distance(rb.position, player.position);
        if (distance >= range)
            MoveToPlayer();
        else if (enemyClose)
            rb.velocity = -repulsionForce;
        else
            rb.velocity = Vector2.zero;


        Vector2 aimDirection = player.position - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    void MoveToPlayer()
    {
        rb.velocity = target.normalized * speed;
    }

    void Shoot()
    {
        nextShot = timeBetweenShots;
        GameManager.PlaySound(shootSounds, gameObject);
        GameObject bulletGO = Instantiate(bullet, shotPoint.position, transform.rotation);
        if(attributes.Contains(TIME_WARP))
            bulletGO.GetComponent<SpriteRenderer>().color = Color.green;
        bulletGO.GetComponent<Rigidbody2D>().velocity = (player.position - rb.position).normalized * bulletSpeed;
        bulletGO.GetComponent<Bullet>().damage = bulletDamage;
    }

    void EndGame()
    {
        rb.velocity = Vector2.zero;
        
        this.enabled = false;
    }
}
