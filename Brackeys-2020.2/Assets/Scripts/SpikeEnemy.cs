using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : Enemy
{
    private Rigidbody2D player;
    private Rigidbody2D rb;
    [SerializeField]
    private float range;
    [SerializeField]
    private float speed;

    void Start()
    {
        player = GameManager.playerObject.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.gameIsOver)
        {
            EndGame();
            return;
        }
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(rb.position, player.position);
        if (distance <= range)
            CirclePlayer();
        else
            MoveToPlayer();
    }

    void CirclePlayer()
    {
        rb.velocity = Vector2.zero;
        // TODO: make enemy circle player
    }

    void MoveToPlayer()
    {
        rb.velocity = (player.position - rb.position).normalized * speed;

        Vector2 aimDirection = player.position - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    void EndGame()
    {
        rb.velocity = Vector2.zero;
        
        this.enabled = false;
    }

    // Move towards player until within range, at which point strafe/circle around the player
    // Always shoot at the player
}
