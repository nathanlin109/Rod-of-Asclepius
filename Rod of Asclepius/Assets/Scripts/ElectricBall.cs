using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    // Fields
    public GameObject wizard;
    private GameObject player;
    public GameObject electricHitParticles;
    private float destroySelfRange;
    private bool hasHit;
    private bool hitPlayer;

    // Start is called before the first frame update
    void Start()
    {
        hasHit = false;
        hitPlayer = false;
        destroySelfRange = 80.0f;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // destroy the electric ball when it is far away
        if (Mathf.Pow(transform.position.x - wizard.transform.position.x, 2) + Mathf.Pow(transform.position.z - wizard.transform.position.z, 2) >= Mathf.Pow(destroySelfRange, 2) || hasHit)
        {
            // Plays the player's particles
            if (hitPlayer == true)
            {
                player.GetComponent<Player>().electricHitParticles.GetComponent<ParticleSystem>().Clear();
                player.GetComponent<Player>().electricHitParticles.GetComponent<ParticleSystem>().Play();
            }
            // Plays particle effect in stationary place
            else
            {  
                Instantiate(electricHitParticles, transform.position, Quaternion.identity);
            }
            
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
                if (player.GetComponent<Player>().health > 0 &&
                    player.GetComponent<Player>().hasCollided == false)
                {
                    player.GetComponent<Player>().health--;
                    player.GetComponent<Player>().hasCollided = true;
                }
                
                hasHit = true;
                hitPlayer = true;
            }else if (other.gameObject.tag == "ElectricBallNoHit")
            {
                hasHit = true;
            }
        }
    }
}