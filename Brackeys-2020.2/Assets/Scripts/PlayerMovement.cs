using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Editor values
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpSpeed;

    //Input
    private float h;
    private bool isJump;

    //Cyote Jump
    private bool canJump;
    private bool onPlatform;

    //Physics
    private Rigidbody2D rb;
    private float yScale;

    //TimeWarp
    private Vector3 screenPoint;
    private Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GameManager.cam.GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        isJump = Input.GetAxisRaw("Vertical") > 0 || Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        rb.isKinematic =  GameManager.timeManager.isWarping;
        if (GameManager.timeManager.isWarping)
        {
            Debug.Log("Player says: " + screenPoint);
            transform.position = cam.ScreenToWorldPoint(screenPoint);
            return;
        }
        
        //Left & Right movement
        rb.velocity = new Vector2(h * maxSpeed, rb.velocity.y);

        //Grounded?
        if (Physics2D.Raycast(transform.position, Vector2.down, .55f))
        {
            canJump = true;
            onPlatform = true;
        }
        else
        {
            onPlatform = false;
            CyoteJump();
        }

        if(canJump && isJump)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private IEnumerator _CyoteJump()
    {
        yield return new WaitForSeconds(0.3f);
        if(!onPlatform)
        {
            canJump = false;
        }
    }

    public void StickToScreen(Vector3 screenPos)
    {
        Debug.Log("Player received: " + screenPos);
        screenPoint = screenPos;
    }

    //Helper functions

    void CyoteJump()
    {
        StartCoroutine(_CyoteJump());
    }
}
