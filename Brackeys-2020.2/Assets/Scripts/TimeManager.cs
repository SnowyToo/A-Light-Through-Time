using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private Stack<Vector3> cameraPositions;
    [HideInInspector]
    public bool isWarping;

    void Awake()
    {
        cameraPositions = new Stack<Vector3>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Warp"))
        {
            GameManager.player.GetComponent<PlayerMovement>().StickToScreen(GameManager.cam.GetComponent<Camera>().WorldToScreenPoint(GameManager.player.position));
            Vector3 pos = GameManager.cam.GetComponent<Camera>().WorldToScreenPoint(GameManager.player.position);
            Debug.Log(pos);
            Debug.Log(GameManager.cam.GetComponent<Camera>().ScreenToWorldPoint(pos));
        }
    }

    void FixedUpdate()
    {
        if(Input.GetButton("Warp"))
        {
            if(cameraPositions.Count > 0)
                GameManager.cam.position = cameraPositions.Pop();
            isWarping = true;
        }
        else
        {
            cameraPositions.Push(GameManager.cam.position);
            isWarping = false;
        }
    }
}
