using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vampire : Enemy
{
    // Fields
    private GameObject player;
    private GameObject sceneMan;
    public GameObject spotLight;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        sceneMan = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }
        else
        {
            GetComponent<NavMeshAgent>().destination = transform.position;
        }
    }

    // Handles collisions w/ player
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        // Handles collision w/ player
        if (collision.gameObject.tag == "Player" &&
            player.GetComponent<Player>().hasCollided == false &&
            player.GetComponent<Player>().health > 0)
        {
            player.GetComponent<Player>().health--;
            player.GetComponent<Player>().hasCollided = true;
        }
    }
}
