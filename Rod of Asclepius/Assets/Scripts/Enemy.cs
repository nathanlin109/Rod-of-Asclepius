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
    protected GameObject sceneMan;
    public GameObject[] childObjectMeshRenderers;
    public GameObject[] childObjectParticles;

    // Trap
    private bool trapped;
    private float trapTimer;
    public float trapTime;
    private GameObject triggeredTrap;

    // Abilities
    public bool silenced;
    public float iceEffectTime;
    protected bool slowed;
    private float iceTimer;
    public GameObject silenceParticles;
    public GameObject flareParticles;
    public GameObject bloodParticles;
    public GameObject trapParticles;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // set render queue to properly mask
        player = GameObject.Find("Player");
        sceneMan = GameObject.Find("SceneManager");
        foreach (GameObject gameObject in childObjectMeshRenderers)
        {
            gameObject.GetComponent<Renderer>().material.renderQueue = 3002;
        }
        foreach (GameObject gameObject in childObjectParticles)
        {
            gameObject.GetComponent<Renderer>().material.renderQueue = 3002;
        }
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
        SeekPlayer();
        ShowSilenceParticles();
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

            // Plays trap/blood particles
            trapParticles.GetComponent<ParticleSystem>().Clear();
            trapParticles.GetComponent<ParticleSystem>().Play();
            bloodParticles.GetComponent<ParticleSystem>().Clear();
            bloodParticles.GetComponent<ParticleSystem>().Play();

            // Plays trap triggered sound
            GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("trap-triggered-sound");

            // Plays trap animation
            triggeredTrap.GetComponent<Animation>().Play("Anim_TrapNeedle_Show");
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
            GetComponent<NavMeshAgent>().speed = speed / 3;
            slowed = true;
        }
    }

    // Seeks player
    void SeekPlayer()
    {
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game && trapped == false)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene2)
        {
            float distance = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));

            if (distance > 7)
            {
                GetComponent<NavMeshAgent>().destination = player.transform.position;
            }
            else
            {
                GetComponent<NavMeshAgent>().destination = transform.position;
            }
        }
        else
        {
            GetComponent<NavMeshAgent>().destination = transform.position;
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

                // Plays trap untrap sound
                GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("trap-untrap-sound");

                if (triggeredTrap != null &&
                triggeredTrap.GetComponent<Item>().playedUntrapAnim == false)
                {
                    // Plays trap animation
                    triggeredTrap.GetComponent<Animation>().Play("Anim_TrapNeedle_Hide");
                    triggeredTrap.GetComponent<Item>().playedUntrapAnim = true;
                }
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

    // Shows silence particles
    void ShowSilenceParticles()
    {
        if (silenced)
        {
            if (silenceParticles.activeSelf == false)
            {
                silenceParticles.SetActive(true);
            }
        }
        else
        {
            if (silenceParticles.activeSelf == true)
            {
                silenceParticles.SetActive(false);
            }
        }
    }
}
