using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapEnemy : Enemy
{  
    // Capturing
    [SerializeField]
    private float captureTime = 2f;
    private bool capturing;
    private EdgeCollider2D edgeCol;
    private CircleCollider2D cirCol;

    // Tracking
    private Rigidbody2D photonRB;
    private Photon photon;
    private Transform photonTransform;
    private Vector2 trackingPosition;
    [SerializeField]
    private float timeBetweenUpdates = 1f;
    private float nextUpdate;

    // Movement
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonRB = GameManager.photonObject.GetComponent<Rigidbody2D>();
        photonTransform = GameManager.photonObject.transform;
        photon = GameManager.photon;
        nextUpdate = timeBetweenUpdates;
        capturing = false;

        edgeCol = GetComponent<EdgeCollider2D>();
        cirCol = GetComponent<CircleCollider2D>();

        type = EnemySpawner.EnemyType.SnapEnemy;

        StartCoroutine(Invincibility());
    }

    void Update()
    {
        if (GameManager.gameIsOver)
        {
            EndGame();
            return;
        }

        if (nextUpdate <= 0f)
            UpdatePosition();
        else
            nextUpdate -= Time.deltaTime;
        
        if (!capturing)
            MoveToPhoton();
    }

    void UpdatePosition()
    {
        nextUpdate = timeBetweenUpdates;
        trackingPosition = photonRB.position;
        if (photon.captured)
        {
            trackingPosition = GameManager.playerObject.transform.position;
        }
    }

    void MoveToPhoton()
    {
        rb.velocity = (trackingPosition - rb.position).normalized * speed;
    }

    // Move in front of the photon when normal time
    // Capture photon for 2 seconds when hit
    // Die when hit by laser

    public override void PhotonHit()
    {
        if (attributes.Contains(TIME_WARP) && !GameManager.isRewinding)
        {
            if (curSprite == null)
                curSprite = Instantiate(warpSprite, transform.position, Quaternion.identity, transform);
            return;
        }
        if (shields.Count > 0) return;

        if (invincible) return;

        StartCoroutine(CapturePhoton());
    }

    IEnumerator CapturePhoton()
    {
        if (photon.captured || invincible) yield break;
        capturing = true;
        rb.velocity = Vector2.zero;
        photonTransform.position = transform.position;
        photon.StartCapture();

        //cirCol.enabled = false;
        edgeCol.enabled = true;

        yield return new WaitForSeconds(captureTime);

        capturing = false;
        photon.EndCapture();
        Die();
    }

    public override void LaserHit()
    {
        if (attributes.Contains(TIME_WARP) && !GameManager.isRewinding)
        {
            if (curSprite == null)
                curSprite = Instantiate(warpSprite, transform.position, Quaternion.identity, transform);
            return;
        }

        if(shields.Count > 0)
        {
            shields.Peek().Hit();
            return;
        }

        capturing = false;
        photon.EndCapture();

        Die(collectPoints:false);
    }

    void EndGame()
    {
        rb.velocity = Vector2.zero;
        
        this.enabled = false;
    }
}
