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

    private Vector3 pos;
    [HideInInspector]
    public bool continueGoing = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (immediate) rb.velocity = GameManager.RandomVelocity(maxSpeed);
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * maxSpeed;

        if (continueGoing) 
            GoTo(pos);
    }

    public Vector2 RandomDirection()
    {
        Vector2 vel = GameManager.RandomVelocity(maxSpeed);
        rb.velocity = vel;
        return vel;
    }

    public void GoTo(Vector3 _pos)
    {
        continueGoing = true;
        pos = _pos;
        rb.velocity = ((Vector2) pos - rb.position).normalized * maxSpeed;
    }

    public void EnableCollider()
    {
        //transform.GetChild(0).gameObject.layer = 17;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
    }
}
