using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 8f;
    private Rigidbody2D rb;
    [SerializeField]
    private bool startOutsideWalls = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (startOutsideWalls)
            rb.velocity = (new Vector2(Random.Range(-2f, 2f), Random.Range(-1.5f, 1.5f)) - rb.position).normalized * maxSpeed;
        else
            rb.velocity = GameManager.RandomVelocity(maxSpeed);
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}
