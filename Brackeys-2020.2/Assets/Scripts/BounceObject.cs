using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    public float maxSpeed = 8f;
    private Rigidbody2D rb;
    [SerializeField]
    private bool immediate = false;

    private Vector3 pos;
    [HideInInspector]
    public bool continueGoing = false;
    [SerializeField]
    private bool rotate = true;
    [SerializeField]
    private float rotateSpeed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (immediate) rb.velocity = GameManager.RandomVelocity(maxSpeed);
        if (Random.value < 0.5) rotateSpeed *= -1f;
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * maxSpeed;

        if (continueGoing) 
            GoTo(pos);
    }

    void Update()
    {
        if (rotate)
        {
            transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        }
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
        continueGoing = false;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
    }
}
