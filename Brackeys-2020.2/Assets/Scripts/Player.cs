using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement
    public float speed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;

    private float h;
    private float v;

    // Aiming
    private Camera cam;
    private Vector2 mousePosition;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //OLD Movement system

        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        //movement = new Vector2(horizontal, vertical).normalized;

        //NEW system
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        //OLD system

        //rb.MovePosition(rb.position + (movement * speed * Time.fixedDeltaTime));

        //NEW system
        rb.velocity = new Vector2(h, v).normalized * speed;

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}
