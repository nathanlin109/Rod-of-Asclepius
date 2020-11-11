using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    // Fields
    private GameObject player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Handles collisions w/ player
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //Debug.Log("HAS COLLIDED? " + player.GetComponent<Player>().hasCollided);
        //Debug.Log("HEALTH? " + player.GetComponent<Player>().health);

        if (collision.gameObject.tag == "Player" &&
            player.GetComponent<Player>().hasCollided == false &&
            player.GetComponent<Player>().health > 0)
        {
            //Debug.Log("Collision w/ player");

            player.GetComponent<Player>().health--;
            player.GetComponent<Player>().hasCollided = true;
        }
    }
}
