using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    Transform target;
    
    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null)
        {
            transform.position = new Vector2(Mathf.Clamp(target.position.x, -11.5f, 11.5f), Mathf.Clamp(target.position.y, -6f, 3.9f));

            if (transform.position == target.position)
            {
                target.gameObject.GetComponent<Enemy>()?.OnPlayField();
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
