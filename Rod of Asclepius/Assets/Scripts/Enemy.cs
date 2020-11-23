using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Fields
    protected GameObject player;
    private float speed;
    private float acceleration;

    // Trap
    private bool trapped;
    private float trapTimer;
    public float trapTime;
    private GameObject triggeredTrap;

    // Abilities
    public bool silenced;
    public float iceEffectTime;
    private bool slowed;
    private float iceTimer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // set render queue to properly mask
        player = GameObject.Find("Player");
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
        speed = GetComponent<NavMeshAgent>().speed;
        acceleration = GetComponent<NavMeshAgent>().acceleration;
        trapped = false;
        trapTimer = 0;
        silenced = false;
        iceTimer = 0;
        slowed = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UnTrap();
        IceSlow();
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
            GetComponent<NavMeshAgent>().destination = transform.position;
            trapped = true;
            triggeredTrap = other.gameObject;
            triggeredTrap.GetComponent<Item>().triggered = true;
            triggeredTrap.GetComponent<Item>().enemyCurrentlyCaught = true;
        }
        else if (other.gameObject.tag == "IceParticles")
        {
            GetComponent<NavMeshAgent>().speed = speed / 2;
            slowed = true;
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Trap")
        {
            triggeredTrap = other.gameObject;
        }
        else if (other.gameObject.tag == "IceParticles")
        {
            GetComponent<NavMeshAgent>().speed = speed / 2;
            slowed = true;
        }
    }

    // Trap
    void UnTrap()
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
                GetComponent<NavMeshAgent>().destination = player.transform.position;
                trapped = false;
            }
        }
    }

    // Ice Ability
    void IceSlow()
    {
        if (slowed == true)
        {
            iceTimer += Time.deltaTime;

            if (iceTimer >= iceEffectTime)
            {
                iceTimer = 0;
                slowed = false;
                GetComponent<NavMeshAgent>().speed = speed;
            }
        }
    }
}
