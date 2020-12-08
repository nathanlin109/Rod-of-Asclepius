using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wizard : Enemy
{
    // Fields
    public GameObject spotLight;

    // Abilities
    public GameObject electricBallPrefab;
    public float electricBallCooldown;
    private float electricBallTimeTillCooldown;
    public float electricBallRange;
    public float projectileSpeed;
    private float animationShootDelay;
    private float animationShootDelayTimer;
    private bool shooting;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        electricBallTimeTillCooldown = 0.0f;
        animationShootDelay = .45f;
        animationShootDelayTimer = 0;
        shooting = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // check for game state
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            // shoot electric balls when within a certain distance of the player
            if (Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.z - transform.position.z, 2) <= Mathf.Pow(electricBallRange, 2))
            {
                // Plays animation only once
                if (electricBallTimeTillCooldown >= electricBallCooldown && silenced == false && shooting == false)
                {
                    animator.SetTrigger("Attacking");
                    shooting = true;
                }
                else if (silenced == true)
                {
                    animationShootDelayTimer = 0;
                    electricBallTimeTillCooldown = 0;
                    shooting = false;
                }

                // Delay between animation and shooting
                if (shooting == true)
                {
                    animationShootDelayTimer += Time.deltaTime;

                    // create the electric ball and reset cooldown
                    if (animationShootDelayTimer >= animationShootDelay)
                    {
                        GameObject electricBall = Instantiate(electricBallPrefab, transform.position, Quaternion.identity);
                        electricBall.GetComponent<Rigidbody>().AddForce(new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z).normalized * projectileSpeed, ForceMode.Impulse);
                        electricBallTimeTillCooldown = 0;
                        GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("wizard-attack-sound");
                        animationShootDelayTimer = 0;
                        shooting = false;
                    }
                }
            }

            // slow down when within a certain distance of the player
            if (Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.z - transform.position.z, 2) <= Mathf.Pow(12, 2))
            {
                if (slowed == false)
                {
                    GetComponent<NavMeshAgent>().speed = 4.5f;
                }
            }
            // speed up when player is farther away and do not attack
            else
            {
                if (slowed == false)
                {
                    GetComponent<NavMeshAgent>().speed = 7.0f;
                }
            }

            // update cooldown
            electricBallTimeTillCooldown += Time.deltaTime;
        }
        // game state is not game
        else
        {
            GetComponent<NavMeshAgent>().speed = 6.0f;
        }
    }
}