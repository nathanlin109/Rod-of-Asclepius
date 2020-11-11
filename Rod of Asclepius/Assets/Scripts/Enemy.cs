using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // set render queue to properly mask
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
