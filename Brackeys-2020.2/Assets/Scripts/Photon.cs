using System.Collections.Generic;
using UnityEngine;

public class Photon : MonoBehaviour
{
    // Bouncing
    public float maxSpeed = 8f;
    private Rigidbody2D rb;

    // Time rewind
    private Stack<Vector2> positionHistory;
    private Stack<Vector2> velocityHistory;
    private Vector2 startPoint;
    private Vector2 startVelocity;
    private Collider2D col;

    public bool rewindVelocity = true;

    void Start()
    {
        col = GetComponent<Collider2D>();

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
        col.enabled = false;

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
        col.enabled = true;

        positionHistory.Push(rb.position);
        if (rewindVelocity)
            velocityHistory.Push(rb.velocity);
    }

    void SetRandomVelocity()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * maxSpeed;
    }
}
