using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Fields
    private float speed;
    private float acceleration;

    // Trap
    private bool trapped;
    private float trapTimer;
    public float trapTime;
    private GameObject triggeredTrap;

    // Abilities
    public bool silenced;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // set render queue to properly mask
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
        speed = GetComponent<NavMeshAgent>().speed;
        acceleration = GetComponent<NavMeshAgent>().acceleration;
        trapped = false;
        trapTimer = 0;
        silenced = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        unTrap();
    }


    // Triggers
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trap" &&
            other.gameObject.GetComponent<Item>().triggered == false &&
            other.gameObject.GetComponent<Item>().enemyCurrentlyCaught == false)
        {
            GetComponent<NavMeshAgent>().speed = 0;
            GetComponent<NavMeshAgent>().acceleration *= 1000;
            trapped = true;
            triggeredTrap = other.gameObject;
            triggeredTrap.GetComponent<Item>().triggered = true;
            triggeredTrap.GetComponent<Item>().enemyCurrentlyCaught = true;
        }
        else if (other.gameObject.tag == "Ability")
        {

        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Trap")
        {
            triggeredTrap = other.gameObject;
        }
        else if (other.gameObject.tag == "Ability")
        {

        }
    }

    // Trap
    void unTrap()
    {
        if (trapped == true)
        {
            trapTimer += Time.deltaTime;

            if (trapTimer >= trapTime)
            {
                trapTimer = 0;
                triggeredTrap.GetComponent<Item>().enemyCurrentlyCaught = false;
                GetComponent<NavMeshAgent>().speed = speed;
                GetComponent<NavMeshAgent>().acceleration = acceleration;
                trapped = false;
            }
        }
    }
}
