using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindFlash : MonoBehaviour
{
    Vector3 up;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        up = transform.up;
        yield return new WaitForSeconds(2.25f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = up;
    }
}
