using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    // Fields
    public float effectTime;
    protected float timeActive;
    protected bool hasHit;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        timeActive = 0;
        hasHit = false;
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
