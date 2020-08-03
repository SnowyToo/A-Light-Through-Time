using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    private Vector3 target;
    [SerializeField]
    private float range = 0.2f;

    [SerializeField]
    Vector2 xRange;
    [SerializeField]
    Vector2 yRange;

    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float rotateSpeed = 5f;

    void Start()
    {
        target = NewTargetPosition();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target) <= range)
        {
            target = NewTargetPosition();
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    Vector3 NewTargetPosition()
    {
        return new Vector3(
            Random.Range(xRange.x, xRange.y),
            Random.Range(yRange.x, yRange.y),
            0f
        );
    }
}
