using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Transform player;
    public static Transform cam;
    public static TimeManager timeManager;
    
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        cam = GameObject.FindWithTag("MainCamera").transform;
        timeManager = GetComponent<TimeManager>();
    }
}
