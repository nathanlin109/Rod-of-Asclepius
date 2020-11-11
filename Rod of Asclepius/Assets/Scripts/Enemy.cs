using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Fields
    public float speed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // set render queue to properly mask
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Collisions (traps/abilities)
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            speed = 0;
        }
        else if (collision.gameObject.tag == "Ability")
        {

        }
    }
}
