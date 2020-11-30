using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    // Fields
    public GameObject wizard;
    private GameObject player;
    private float destroySelfRange;
    private bool hasHit;

    // Start is called before the first frame update
    void Start()
    {
        hasHit = false;
        destroySelfRange = 80.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // destroy the electric ball when it is far away
        if (Mathf.Pow(transform.position.x - wizard.transform.position.x, 2) + Mathf.Pow(transform.position.z - wizard.transform.position.z, 2) >= Mathf.Pow(destroySelfRange, 2) || hasHit)
        {
            Destroy(gameObject);
        }
    }

    // Trigger enter
    private void OnTriggerEnter(Collider other)
    {
        // check for collisions with the player
        if (hasHit == false)
        {
            // damage the player if they are hit
            if (other.gameObject.tag == "Player")
            {
                player = other.gameObject;

                if (player.GetComponent<Player>().health > 0)
                {
                    player.GetComponent<Player>().health--;
                }
                
                hasHit = true;
            }
        }
    }
}