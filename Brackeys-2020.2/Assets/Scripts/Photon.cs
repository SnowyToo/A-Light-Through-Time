using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photon : MonoBehaviour
{
    // Bouncing
    private Rigidbody2D rb;
    private CircleCollider2D col;
    [SerializeField]
    private AudioClip boop;
    [SerializeField]
    private GameObject hitParticles;

    // Time rewind
    private Stack<Vector2> positionHistory;
    private Stack<Vector2> velocityHistory;
    private Vector2 startPoint;
    private Vector2 startVelocity;
    private bool isCovered = false;

    // Other
    [SerializeField]
    private GameObject deathParticles;

    // Capturing
    [HideInInspector]
    public bool captured = false;

    void Start()
    {
        positionHistory = new Stack<Vector2>();
        velocityHistory = new Stack<Vector2>();

        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        
        startPoint = rb.position;
        startVelocity = GetComponent<BounceObject>().RandomDirection();
        positionHistory.Push(startPoint);
    }

    void FixedUpdate()
    {
        if (GameManager.isRewinding)
            Rewind();
        else
            StorePosition();
    }

    void Rewind()
    {
        col.isTrigger = true;

        if (positionHistory.Count > 0)
            rb.position = positionHistory.Pop();
        else
            rb.position = startPoint;

        if (velocityHistory.Count > 0)
            rb.velocity = velocityHistory.Pop();
        else
            rb.velocity = startVelocity;
    }

    void StorePosition()
    {
        col.isTrigger = false;
        positionHistory.Push(rb.position);
        velocityHistory.Push(rb.velocity);
    }

    public void Die()
    {
        GameManager.SpawnParticles(deathParticles, gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Hit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (captured) return;
        if (other.gameObject.layer == 11 && !GameManager.isRewinding)
        {
            Hit();
            rb.velocity = GameManager.GetMousePosition() - rb.position;
        }
    }

    private void Hit()
    {
        GameManager.PlaySound(boop, gameObject, 0.4f);
        GameManager.SpawnParticles(hitParticles, gameObject);
    }

    public void StartCapture()
    {
        captured = true;
        GameManager.uiManager.UpdateRewind(false);
    }

    public void EndCapture()
    {
        captured = false;
        GameManager.uiManager.UpdateRewind(true);
    }
}
