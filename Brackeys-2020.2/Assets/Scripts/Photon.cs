using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photon : MonoBehaviour
{
    // Bouncing
    public float maxSpeed = 8f;
    private Rigidbody2D rb;
    [SerializeField]
    private AudioClip boop;
    [SerializeField]
    private GameObject hitParticles;

    // Time rewind
    private Stack<Vector2> positionHistory;
    private Stack<Vector2> velocityHistory;
    private Vector2 startPoint;
    private Vector2 startVelocity;

    public bool rewindVelocity = true;

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

        rb = GetComponent<Rigidbody2D>();
        SetRandomVelocity();
        
        startPoint = rb.position;
        startVelocity = rb.velocity;
        positionHistory.Push(startPoint);
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * maxSpeed;

        if (GameManager.isRewinding)
            Rewind();
        else
            StorePosition();
    }

    void Rewind()
    {
        if (positionHistory.Count > 0)
            rb.position = positionHistory.Pop();
        else
            rb.position = startPoint;

        if (rewindVelocity)
        {
            if (velocityHistory.Count > 0)
                rb.velocity = velocityHistory.Pop();
            else
                rb.velocity = startVelocity;
        }
    }

    void StorePosition()
    {
        positionHistory.Push(rb.position);
        if (rewindVelocity)
            velocityHistory.Push(rb.velocity);
    }

    void SetRandomVelocity()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * maxSpeed;
    }

    public void Die()
    {
        GameManager.SpawnParticles(deathParticles, gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (captured) return;
        Hit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (captured) return;
        if (other.gameObject.layer == 11)
        {
            Hit();
            rb.velocity = GameManager.GetMousePosition() - (Vector2)rb.transform.position;
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
