using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public enum Axis { HORIZONTAL, VERTICAL, BOTH }
    private Axis axisFollow;

    private bool canMove;

    [SerializeField]
    private float damping;

    void Awake()
    {
        canMove = true;
        axisFollow = Axis.HORIZONTAL;
    }

    void FixedUpdate()
    {
        canMove = !GameManager.timeManager.isWarping;
        if (canMove)
        {
            Vector3 newPosition = transform.position;
            switch (axisFollow)
            {
                case Axis.HORIZONTAL:
                    newPosition = new Vector3(Mathf.Lerp(transform.position.x, GameManager.player.position.x, damping), transform.position.y, -10);
                    break;
                case Axis.VERTICAL:
                    newPosition = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, GameManager.player.position.y, damping), -10);
                    break;
                case Axis.BOTH:
                    newPosition = new Vector3(Mathf.Lerp(transform.position.x, GameManager.player.position.x, damping), Mathf.Lerp(transform.position.y, GameManager.player.position.y, damping), -10);
                    break;
            }
            transform.position = newPosition;
        }
    }

    public void SetAxisToFollow(Axis a)
    {
        axisFollow = a;
    }
}
