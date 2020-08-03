using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : Enemy
{
    // Movement
    private Rigidbody2D player;
    private Rigidbody2D rb;
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
        float distance = Vector2.Distance(rb.position, player.position);
        if (distance <= range)
            CirclePlayer();
        else
            MoveToPlayer();
        
        Vector2 aimDirection = player.position - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    void CirclePlayer()
    {
        rb.velocity = Vector2.zero;
        // TODO: make enemy circle player
    }

    void MoveToPlayer()
    {
        rb.velocity = (player.position - rb.position).normalized * speed;
    }

    void Shoot()
    {
        nextShot = timeBetweenShots;
        GameManager.PlaySound(shootSounds, gameObject);
        GameObject bulletGO = Instantiate(bullet, shotPoint.position, transform.rotation);
        bulletGO.GetComponent<Rigidbody2D>().velocity = (player.position - rb.position).normalized * bulletSpeed;
        bulletGO.GetComponent<Bullet>().damage = bulletDamage;
    }

    void EndGame()
    {
        rb.velocity = Vector2.zero;
        
        this.enabled = false;
    }
}
