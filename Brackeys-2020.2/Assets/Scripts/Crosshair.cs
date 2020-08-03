using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        Cursor.visible = false;
        cam = Camera.main;
    }

    void Update()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }
}
