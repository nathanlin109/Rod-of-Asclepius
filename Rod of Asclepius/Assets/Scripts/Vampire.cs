using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vampire : Enemy
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
            player.GetComponent<Player>().health--;
            player.GetComponent<Player>().hasCollided = true;
            player.GetComponent<Player>().bloodParticles.GetComponent<ParticleSystem>().Clear();
            player.GetComponent<Player>().bloodParticles.GetComponent<ParticleSystem>().Play();
            GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("vampire-bite-sound");
        }
    }
}
