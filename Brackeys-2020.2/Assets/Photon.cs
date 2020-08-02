using UnityEngine;

public class Photon : MonoBehaviour
{
    public float maxSpeed = 8f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetRandomVelocity();
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    void SetRandomVelocity()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * maxSpeed;
    }
}
