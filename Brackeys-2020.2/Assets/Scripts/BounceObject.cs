using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 8f;
    private Rigidbody2D rb;
    [SerializeField]
    private bool immediate = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (immediate) rb.velocity = GameManager.RandomVelocity(maxSpeed);
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    public Vector2 RandomDirection()
    {
        Vector2 vel = GameManager.RandomVelocity(maxSpeed);
        rb.velocity = vel;
        return vel;
    }

    public void GoTo(Vector3 pos)
    {
        //rb = GetComponent<Rigidbody2D>();
        rb.velocity = ((Vector2) pos - rb.position).normalized * maxSpeed;
    }
}
