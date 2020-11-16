using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Fields
    public bool triggered;
    public bool enemyCurrentlyCaught;

    // Start is called before the first frame update
    void Start()
    {
        // set render queue to properly mask
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
        triggered = false;
        enemyCurrentlyCaught = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
