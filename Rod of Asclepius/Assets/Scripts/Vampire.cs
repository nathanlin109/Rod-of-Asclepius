using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vampire : Enemy
{
    // Fields
    public GameObject spotLight;
    private float animationBiteDelay;
    private float animationBiteDelayTimer;
    private bool biting;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animationBiteDelay = 1;
        animationBiteDelayTimer = 0;
        biting = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (biting == true)
        {
            animationBiteDelayTimer += Time.deltaTime;
        }
    }

    // Handles collisions w/ player
    private void OnCollisionStay(Collision collision)
    {
        // Handles collision w/ player
        if (collision.gameObject.tag == "Player" &&
            player.GetComponent<Player>().hasCollided == false &&
            player.GetComponent<Player>().health > 0 && silenced == false &&
            sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            if (biting == false)
            {
                animator.SetTrigger("Attacking");
                biting = true;
            }
            else if (biting == true && animationBiteDelayTimer >= animationBiteDelay)
            {
                biting = false;
                animationBiteDelayTimer = 0;
                player.GetComponent<Player>().health--;
                player.GetComponent<Player>().hasCollided = true;
                player.GetComponent<Player>().bloodParticles.GetComponent<ParticleSystem>().Clear();
                player.GetComponent<Player>().bloodParticles.GetComponent<ParticleSystem>().Play();
                GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("vampire-bite-sound");
            }
        }
    }
}
