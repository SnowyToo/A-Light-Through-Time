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
    private bool canCapture;

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
        canCapture = false;

        edgeCol = GetComponent<EdgeCollider2D>();

        type = EnemySpawner.EnemyType.SnapEnemy;

        StartCoroutine(Invincibility());
    }

    public override void OnPlayField()
    {
        canCapture = true;
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

    public override bool PhotonHit()
    {
        if (!base.PhotonHit(false))
            return false;

        StartCoroutine(CapturePhoton());

        return true;
    }

    IEnumerator CapturePhoton()
    {
        if (photon.captured || invincible || !canCapture) yield break;
        capturing = true;
        rb.velocity = Vector2.zero;
        photonTransform.position = transform.position;
        photon.StartCapture();

        edgeCol.enabled = true;

        yield return new WaitForSeconds(captureTime);

        capturing = false;
        photon.EndCapture();
        Die();
    }

    public override bool LaserHit()
    {
        if (!base.LaserHit())
            return false;

        capturing = false;
        photon.EndCapture();

        //Die(collectPoints:false);
        return true;
    }

    void EndGame()
    {
        rb.velocity = Vector2.zero;
        
        this.enabled = false;
    }
}
