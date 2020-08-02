using System.Collections.Generic;
using UnityEngine;

public class Photon : MonoBehaviour
{
    // Bouncing
    public float maxSpeed = 8f;
    private Rigidbody2D rb;

    // Time rewind
    private Stack<Vector2> history;
    private bool rewinding;
    private Vector2 startPoint;

    // Enemies
    public string enemyLayerName;

    void Start()
    {
        history = new Stack<Vector2>();
        rewinding = false;

        rb = GetComponent<Rigidbody2D>();
        SetRandomVelocity();
        
        startPoint = rb.position;
        history.Push(startPoint);
    }

    void Update()
    {
        if (Input.GetButton("Rewind"))
        {
            rewinding = true;
        }
        else
        {
            rewinding = false;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * maxSpeed;

        if (rewinding)
        {
            if (history.Count > 0)
                rb.position = history.Pop();
            else
                rb.position = startPoint;
        }
        else
        {
            history.Push(rb.position);
        }
    }

    void SetRandomVelocity()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * maxSpeed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject other = col.gameObject;
        if (other.layer == LayerMask.NameToLayer(enemyLayerName))
        {
            GameManager.KillEnemy(other);
        }
    }
}
