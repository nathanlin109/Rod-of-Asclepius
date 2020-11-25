using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wizard : Enemy
{
    // Fields
    public GameObject spotLight;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


        // slow down when within a certain distance of the player
        //if (Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.z - transform.position.z, 2) <= Mathf.Pow(15, 2))
        //{
        //    GetComponent<NavMeshAgent>().speed = 3;
        //}
        //else
        //{
        //    GetComponent<NavMeshAgent>().speed = 6;
        //}
    }

    // Handles collisions w/ player
    private void OnCollisionStay(Collision collision)
    {
        // Handles collision w/ player
        if (collision.gameObject.tag == "Player" &&
            player.GetComponent<Player>().hasCollided == false &&
            player.GetComponent<Player>().health > 0 && silenced == false)
        {
            Debug.Log("Damaged Player");
            player.GetComponent<Player>().health--;
            player.GetComponent<Player>().hasCollided = true;
        }
    }
}
