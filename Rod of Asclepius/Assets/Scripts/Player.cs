using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    // Player Fields
    public GameObject[] childObjectMeshRenderers;
    public int health;
    GameObject sceneMan;
    public float moveSpeed;
    public Vector3 moveVector;
    private bool playedDeathSound;
    public Animator animator;
    private bool isDead;

    // Collisions
    public float playerImmuneTime;
    private float immunityTimer;
    private float flashingTimer;
    public bool hasCollided;

    // Trap
    public GameObject trapPrefab;
    private float trapDeployTimer;
    public float trapDeployTime;
    private float trapMoveSpeedMultiplier;
    private bool placedTrap;

    // Trap UI
    public GameObject trapIcon;
    public GameObject trapProgressBackground;
    public GameObject trapProgressBar;

    // Ability
    public GameObject flarePrefab;
    public GameObject silencePrefab;
    public GameObject icePrefab;
    public float projectileSpeed;
    public float flareCooldown;
    private float flareTimeTillCooldown;
    public float silenceCooldown;
    private float silenceTimeTillCooldown;
    public float iceCooldown;
    private float iceTimeTillCooldown;
    private float animationThrowDelay;
    private float animationThrowDelayTimer;
    private bool throwing;
    enum ThrowingAbility { Flare, Ice, Silence}
    private ThrowingAbility throwingAbility;

    // Ability UI
    public GameObject abilityUI;
    public GameObject iceIcon;
    public GameObject iceCooldownText;
    public GameObject silenceIcon;
    public GameObject silenceCooldownText;
    public GameObject flareIcon;
    public GameObject flareCooldownText;

    // Objectives
    public float objectiveItemsCollected;

    // Cutscene movement
    public bool cutscene1ShouldMove;
    public bool cutscene2ShouldRotateCoffin;
    public bool cutscene2ShouldRotateVampire;
    public bool cutscene4ShouldRotateCoffin;
    public bool cutscene5ShouldRotate;
    private bool cutscene5ShouldCloseGate;
    private bool cutscene5ClosedGate;

    // Enemies
    public GameObject vampire;
    public GameObject wizard;

    // Particles
    public GameObject healthPickupParticles;
    public GameObject electricHitParticles;
    public GameObject bloodParticles;

    // Start is called before the first frame update
    void Start()
    {
        // Own fields
        health = 2;
        sceneMan = GameObject.Find("SceneManager");
        moveVector = Vector3.zero;
        playedDeathSound = false;
        isDead = false;

        // Cutscene movement
        cutscene1ShouldMove = false;
        cutscene2ShouldRotateCoffin = false;
        cutscene2ShouldRotateVampire = false;
        cutscene5ShouldRotate = false;
        cutscene5ShouldCloseGate = false;
        cutscene5ClosedGate = false;

        // Collision/damage
        hasCollided = false;
        immunityTimer = 0;
        flashingTimer = 0;

        // Traps
        trapDeployTimer = 0;
        trapMoveSpeedMultiplier = 1;
        placedTrap = false;

        // Objectives
        objectiveItemsCollected = 0;

        // Abilities
        flareTimeTillCooldown = flareCooldown;
        silenceTimeTillCooldown = silenceCooldown;
        iceTimeTillCooldown = iceCooldown;
        throwing = false;
        animationThrowDelay = 1f;
        animationThrowDelayTimer = 0;
        throwingAbility = ThrowingAbility.Flare;
}

// Update is called once per frame
void Update()
    {
        SetAnimatorVariables();
        AbilityUI();
        // Game Movement
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            MovementKeyBoardInputs();
            MouseInputs();
            CollisionCooldown();
            PlaceTrap();
            ThrowAbility();
            ShowBlood();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Death)
        {
            // Shows blood texture on death
            ShowBlood();
            moveVector = Vector3.zero;
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.GameNoCombat)
        {
            // Movement/mouse
            MovementKeyBoardInputs();
            MouseInputs();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene1)
        {
            if (cutscene1ShouldMove == true)
            {
                // Moves player up in cutscene1
                moveVector = new Vector3(0, 0, 1.0f);
                GetComponent<Rigidbody>().transform.Translate(moveVector * moveSpeed / 2 * Time.deltaTime, Space.World);
            }
            else
            {
                moveVector = Vector3.zero;
            }
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene2 ||
            sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene4 ||
            sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene5)
        {
            // Rotates the player during cutscenes
            RotatePlayerCutscenes();

            if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene5)
            {
                // Closes gate in cutscene5
                CloseGateCutscene5();
            }
            moveVector = Vector3.zero;
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Win)
        {
            // Moves player down in win
            moveVector = new Vector3(0, 0, -1.0f);
            GetComponent<Rigidbody>().transform.Translate(moveVector * moveSpeed / 2 * Time.deltaTime, Space.World);

            // Closes gate in cutscene5
            CloseGateCutscene5();
        }
    }

    // Sets animator variables
    private void SetAnimatorVariables()
    {
        animator.SetFloat("Speed", moveVector.magnitude);
        animator.SetBool("IsDead", isDead);
        animator.SetBool("IsThrowing", throwing);

        if (trapDeployTimer > 0)
        {
            animator.SetBool("IsCrouching", true);
        }
        else
        {
            animator.SetBool("IsCrouching", false);
        }

        // Gets the angle to determine which direction to move
        // Gets the angle between forward and target forward
        float forwardAngle = Mathf.Atan2(transform.forward.z,
            transform.forward.x) * Mathf.Rad2Deg;
        float moveAngle = Mathf.Atan2(moveVector.z, moveVector.x) * Mathf.Rad2Deg;
        float angleBetweenRotation = forwardAngle - moveAngle;

        if (angleBetweenRotation < -180)
        {
            angleBetweenRotation = 180 + (angleBetweenRotation + 180);
        }
        else if (angleBetweenRotation > 180)
        {
            angleBetweenRotation = -180 + (angleBetweenRotation - 180);
        }
        animator.SetFloat("Angle", angleBetweenRotation);
    }

    // Process keyboard inputs
    void MovementKeyBoardInputs()
    {
        moveVector = Vector3.zero;

        // Movements
        if (Input.GetKey(KeyCode.W))
        {
            moveVector += new Vector3(0, 0, 1.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector += new Vector3(-1.0f, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector += new Vector3(0, 0, -1.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector += new Vector3(1.0f, 0, 0);
        }

        // Applies the transformation
        moveVector.Normalize();
        GetComponent<Rigidbody>().transform.Translate(moveVector * moveSpeed * trapMoveSpeedMultiplier * Time.deltaTime, Space.World);
    }

    // Process mouse inputs
    void MouseInputs()
    {
        // Gets mouse position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - transform.position.y;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // find the angle to rotate the player so that it points towards the mouse
        float angleOfRotation = Mathf.Atan2(mouseWorldPos.z - gameObject.transform.position.z,
            mouseWorldPos.x - gameObject.transform.position.x) * Mathf.Rad2Deg - 90.0f;

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, -angleOfRotation, 0);
    }

    // Rotates player cutscene2
    void RotatePlayerCutscenes()
    {
        if (cutscene2ShouldRotateCoffin == true || cutscene4ShouldRotateCoffin == true ||
            cutscene2ShouldRotateVampire == true || cutscene5ShouldRotate)
        {
            GameObject targetObject = gameObject;
            if (cutscene2ShouldRotateCoffin == true || cutscene4ShouldRotateCoffin == true)
            {
                // find the angle to rotate the player so that it points towards the mother's grave
                targetObject = GameObject.Find("StaticLevelAssets/OtherObjects/Coffin Closed Standing");
            }
            else if (cutscene2ShouldRotateVampire == true)
            {
                // find the angle to rotate the player so that it points towards the tree
                targetObject = GameObject.Find("StaticLevelAssets/Graves/TopLeft/GraveRow (2)/gravestone_04 (7)");
            }
            else if (cutscene5ShouldRotate == true)
            {
                // find the angle to rotate the player so that it points towards the bottom of map
                targetObject = GameObject.Find("StaticLevelAssets/Trees/Tree 1 (13)/");
            }

            // Sets the target forward direction
            Vector3 targetForwardDirection = targetObject.transform.position - transform.position;
            targetForwardDirection.y = 0;
            targetForwardDirection.Normalize();

            // Gets the angle between forward and target forward
            float forwardAngle = Mathf.Atan2(transform.forward.z,
                transform.forward.x) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(targetForwardDirection.z, targetForwardDirection.x) * Mathf.Rad2Deg;
            float angleBetweenRotation = forwardAngle - targetAngle;

            if (angleBetweenRotation < -180)
            {
                angleBetweenRotation = 180 + (angleBetweenRotation + 180);
            }
            else if (angleBetweenRotation > 180)
            {
                angleBetweenRotation = -180 + (angleBetweenRotation - 180);
            }

            // Rotates the player
            if (angleBetweenRotation < 0)
            {
                transform.Rotate(-Vector3.up * Time.deltaTime * 400);
            }
            else
            {
                transform.Rotate(Vector3.up * Time.deltaTime * 400);
            }

            // Stops rotating player and shows correct cutscene text
            if (Mathf.Abs(angleBetweenRotation) <= 4)
            {
                sceneMan.GetComponent<InputManager>().buttonPromptText.SetActive(true);
                sceneMan.GetComponent<InputManager>().dialogueBackground.SetActive(true);
                if (cutscene2ShouldRotateCoffin == true)
                {
                    cutscene2ShouldRotateCoffin = false;
                    sceneMan.GetComponent<InputManager>().cutscene2_1Text.SetActive(true);
                }
                else if (cutscene2ShouldRotateVampire)
                {
                    cutscene2ShouldRotateVampire = false;
                    sceneMan.GetComponent<InputManager>().cutscene2_3Text.SetActive(true);
                }
                else if (cutscene4ShouldRotateCoffin == true)
                {
                    cutscene4ShouldRotateCoffin = false;
                    sceneMan.GetComponent<InputManager>().cutscene4_1Text.SetActive(true);
                }
                else if (cutscene5ShouldRotate == true)
                {
                    cutscene5ShouldRotate = false;
                    sceneMan.GetComponent<InputManager>().cutscene5_1Text.SetActive(true);
                }
            }
        }
    }

    // Shows blood splatter
    void ShowBlood()
    {
        // 2 HP
        if (health == 2 && sceneMan.GetComponent<InputManager>().blood.activeSelf == true)
        {
            sceneMan.GetComponent<InputManager>().blood.SetActive(false);
        }
        // 1 HP
        else if (health == 1 && sceneMan.GetComponent<InputManager>().blood.activeSelf == false)
        {
            sceneMan.GetComponent<InputManager>().blood.SetActive(true);
        }
        // 0 HP
        else if (health == 0 && sceneMan.GetComponent<InputManager>().blood.activeSelf == true)
        {
            Color bloodColor = sceneMan.GetComponent<InputManager>().blood.GetComponent<Image>().color;
            bloodColor.a = 1;
            sceneMan.GetComponent<InputManager>().blood.GetComponent<Image>().color = bloodColor;
            isDead = true;
            sceneMan.GetComponent<SceneMan>().gameState = GameState.Death;

            // Plays death sound
            if (playedDeathSound == false)
            {
                Sound deathSound = Array.Find(GameObject.Find("AudioManager").GetComponent<AudioMan>().sounds, sound => sound.name == "death-sound");
                if (deathSound != null && deathSound.source.isPlaying == false)
                {
                    deathSound.source.Play();
                    playedDeathSound = true;
                }
            }
        }
    }

    // Traps/abilities
    void PlaceTrap()
    {
        if (Input.GetKey(KeyCode.Space) && placedTrap == false)
        {
            trapDeployTimer += Time.deltaTime;
            trapMoveSpeedMultiplier = .3f;

            // enable trap progress UI
            trapProgressBackground.SetActive(true);
            trapProgressBar.SetActive(true);
            trapProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5, (trapDeployTimer / trapDeployTime) * 100);

            // Plays trap setting sound
            Sound trapSoundPlay = Array.Find(GameObject.Find("AudioManager").GetComponent<AudioMan>().sounds, sound => sound.name == "trap-set-sound");
            if (trapSoundPlay != null && trapSoundPlay.source.isPlaying == false)
            {
                trapSoundPlay.source.Play();
            }

            // Deploys trap
            if (trapDeployTimer >= trapDeployTime)
            {
                trapDeployTimer = 0;
                trapMoveSpeedMultiplier = 1;
                placedTrap = true;

                // reset and disable trap progress UI
                trapProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 0);
                trapProgressBackground.SetActive(false);
                trapProgressBar.SetActive(false);

                // Stops trap setting sound
                Sound trapSoundStop = Array.Find(GameObject.Find("AudioManager").GetComponent<AudioMan>().sounds, sound => sound.name == "trap-set-sound");
                if (trapSoundStop != null && trapSoundStop.source.isPlaying == true)
                {
                    trapSoundStop.source.Stop();
                }

                GameObject.Instantiate(trapPrefab,
                    new Vector3(transform.position.x,
                    0,
                    transform.position.z),
                    Quaternion.identity);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            trapDeployTimer = 0;
            trapMoveSpeedMultiplier = 1;

            // reset and disable trap progress UI
            trapProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 0);
            trapProgressBackground.SetActive(false);
            trapProgressBar.SetActive(false);

            // Stops trap setting sound
            Sound trapSoundStop = Array.Find(GameObject.Find("AudioManager").GetComponent<AudioMan>().sounds, sound => sound.name == "trap-set-sound");
            if (trapSoundStop != null && trapSoundStop.source.isPlaying == true)
            {
                trapSoundStop.source.Stop();
            }
        }
    }

    // Throw
    void ThrowAbility()
    {
        // Increments timers
        flareTimeTillCooldown += Time.deltaTime;
        silenceTimeTillCooldown += Time.deltaTime;
        iceTimeTillCooldown += Time.deltaTime;

        // Flare (Anim)
        if (flareTimeTillCooldown >= flareCooldown && Input.GetMouseButtonDown(1) && trapDeployTimer == 0 && throwing == false)
        {          
            animator.SetTrigger("Throw");
            throwing = true;
            throwingAbility = ThrowingAbility.Flare;
        }

        // Silence (Anim)
        if (silenceTimeTillCooldown >= silenceCooldown && Input.GetMouseButtonDown(2) && trapDeployTimer == 0 && throwing == false)
        {
            animator.SetTrigger("Throw");
            throwing = true;
            throwingAbility = ThrowingAbility.Silence;
        }

        // Ice (Anim)
        if (iceTimeTillCooldown >= iceCooldown && Input.GetMouseButtonDown(0) && trapDeployTimer == 0 && throwing == false)
        {
            animator.SetTrigger("Throw");
            throwing = true;
            throwingAbility = ThrowingAbility.Ice;
        }

        // Delay between animation and throwing
        if (throwing == true)
        {
            animationThrowDelayTimer += Time.deltaTime;

            if (animationThrowDelayTimer >= animationThrowDelay)
            {
                throwing = false;
                animationThrowDelayTimer = 0;
                GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("throw-ability-sound");

                // Throws correct ability
                switch (throwingAbility)
                {
                    case ThrowingAbility.Flare:
                        GameObject flare = Instantiate(flarePrefab,
                            transform.position + new Vector3(0, gameObject.GetComponent<BoxCollider>().bounds.size.y / 2, 0),
                            Quaternion.identity);
                        flare.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed + moveVector * moveSpeed / 4, ForceMode.Impulse);
                        flareTimeTillCooldown = 0;
                        break;
                    case ThrowingAbility.Silence:
                        GameObject silence = Instantiate(silencePrefab,
                            transform.position + new Vector3(0, gameObject.GetComponent<BoxCollider>().bounds.size.y / 2, 0),
                            Quaternion.identity);
                        silence.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed + moveVector * moveSpeed / 4, ForceMode.Impulse);
                        silenceTimeTillCooldown = 0;
                        break;
                    case ThrowingAbility.Ice:
                        GameObject ice = Instantiate(icePrefab,
                            transform.position + new Vector3(0, gameObject.GetComponent<BoxCollider>().bounds.size.y / 2, 0),
                            Quaternion.identity);
                        ice.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed + moveVector * moveSpeed / 4, ForceMode.Impulse);
                        iceTimeTillCooldown = 0;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // Ability UI and Cooldowns
    void AbilityUI()
    {
        // Game state
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            // enable ability UI
            abilityUI.SetActive(true);

            // trap unavailable
            if (placedTrap)
            {
                trapIcon.GetComponent<Image>().color = new Color32(128, 128, 128, 255);
            }
            // trap available
            else
            {
                trapIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }

            // ice available
            if (iceTimeTillCooldown >= iceCooldown)
            {
                iceIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                iceCooldownText.SetActive(false);
            }
            // ice unavailable
            else
            {
                iceIcon.GetComponent<Image>().color = new Color32(128, 128, 128, 255);
                iceCooldownText.SetActive(true);
                iceCooldownText.GetComponent<Text>().text = (iceCooldown - iceTimeTillCooldown).ToString("0");
            }

            // silence available
            if (silenceTimeTillCooldown >= silenceCooldown)
            {
                silenceIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                silenceCooldownText.SetActive(false);
            }
            // silence unavailable
            else
            {
                silenceIcon.GetComponent<Image>().color = new Color32(128, 128, 128, 255);
                silenceCooldownText.SetActive(true);
                silenceCooldownText.GetComponent<Text>().text = (silenceCooldown - silenceTimeTillCooldown).ToString("0");
            }

            // flare available
            if (flareTimeTillCooldown >= flareCooldown)
            {
                flareIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                flareCooldownText.SetActive(false);
            }
            // flare unavailable
            else
            {
                flareIcon.GetComponent<Image>().color = new Color32(128, 128, 128, 255);
                flareCooldownText.SetActive(true);
                flareCooldownText.GetComponent<Text>().text = (flareCooldown - flareTimeTillCooldown).ToString("0");
            }
        }
        // Other states
        else
        {
            // disable ability UI
            abilityUI.SetActive(false);
        }
    }

    // Prevents player from colliding too much
    void CollisionCooldown()
    {
        if (hasCollided)
        {
            immunityTimer = Mathf.MoveTowards(immunityTimer, playerImmuneTime, Time.deltaTime);
            flashingTimer = Mathf.MoveTowards(flashingTimer, .25f, Time.deltaTime);

            // Resets ability to collide
            if (immunityTimer >= playerImmuneTime)
            {
                immunityTimer = 0;
                hasCollided = false;
                flashingTimer = 0;
                foreach (GameObject gameObject in childObjectMeshRenderers)
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                }
            }

            // Flashes player
            if (flashingTimer >= .25f)
            {
                foreach (GameObject gameObject in childObjectMeshRenderers)
                {
                    gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
                }
                flashingTimer = 0;
            }
        }
    }

    // Handles triggers with items
    private void OnTriggerEnter(Collider other)
    {
        // Game state
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            // Healing item
            if (other.gameObject.tag == "HealingItem" && health == 1)
            {
                health++;
                Destroy(other.gameObject);
                healthPickupParticles.GetComponent<ParticleSystem>().Clear();
                healthPickupParticles.GetComponent<ParticleSystem>().Play();
                GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("heal-sound");
            }

            // Trap pickup prompt
            else if (other.gameObject.tag == "Trap" && other.gameObject.GetComponent<Item>().enemyCurrentlyCaught == false)
            {
                if (sceneMan.GetComponent<InputManager>().pickupTrapText.activeSelf == false)
                {
                    sceneMan.GetComponent<InputManager>().pickupTrapText.SetActive(true);
                }
            }

            // Objective item pickup prompt
            else if (other.gameObject.tag == "ObjectiveItem")
            {
                if (sceneMan.GetComponent<InputManager>().pickupObjectiveItemText.activeSelf == false)
                {
                    sceneMan.GetComponent<InputManager>().pickupObjectiveItemText.SetActive(true);
                }
            }

            // Game to Cutscene4 (Trigger Mother's grave)
            if (other.gameObject.tag == "TriggerMotherGrave" && objectiveItemsCollected >= 5)
            {
                sceneMan.GetComponent<SceneMan>().gameState = GameState.Cutscene4;
                objectiveItemsCollected = -1;
                cutscene4ShouldRotateCoffin = true;
            }

            // Trigger gate open after resurrect mother
            if (sceneMan.GetComponent<SceneMan>().resurrectedMom == true)
            {
                if (other.gameObject.tag == "TriggerOpenGateEnd")
                {
                    GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Open");
                }
                else if (other.gameObject.tag == "TriggerCloseGateEnd")
                {
                    sceneMan.GetComponent<SceneMan>().gameState = GameState.Cutscene5;
                    cutscene5ShouldRotate = true;
                    cutscene5ShouldCloseGate = true;
                }
            }
        }

        // Cutscene1 to GameNoCombat trigger
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene1)
        {
            if (other.gameObject.tag == "TriggerOpenGateBeginning")
            {
                GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Open");
            }
            else if (other.gameObject.tag == "TriggerCloseGateBeginning")
            {
                GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Close");
                sceneMan.GetComponent<InputManager>().buttonPromptText.SetActive(true);
                sceneMan.GetComponent<InputManager>().cutscene1_3Text.SetActive(true);
                sceneMan.GetComponent<InputManager>().dialogueBackground.SetActive(true);
                cutscene1ShouldMove = false;
            }
        }

        // GameNoCombat to Cutscene2 trigger
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.GameNoCombat)
        {
            if (other.gameObject.tag == "TriggerMotherGrave")
            {
                sceneMan.GetComponent<SceneMan>().gameState = GameState.Cutscene2;
                cutscene2ShouldRotateCoffin = true;
            }
        }
    }

    // Closes the gate when the mother is outside the graveyard
    void CloseGateCutscene5()
    {
        if (cutscene5ShouldCloseGate == true &&
            sceneMan.GetComponent<InputManager>().mother.transform.position.z <= 49 &&
            cutscene5ClosedGate == false)
        {
            GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Close");
            cutscene5ClosedGate = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Game state
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            // Pickup trap
            if (other.gameObject.tag == "Trap")
            {
                if (other.gameObject.GetComponent<Item>().enemyCurrentlyCaught == false)
                {
                    // Shows trap text when enemy not caught in it
                    if (sceneMan.GetComponent<InputManager>().pickupTrapText.activeSelf == false)
                    {
                        sceneMan.GetComponent<InputManager>().pickupTrapText.SetActive(true);
                    }

                    // Picks up the trap
                    if (Input.GetKey(KeyCode.E))
                    {
                        if (sceneMan.GetComponent<InputManager>().pickupTrapText.activeSelf == true)
                        {
                            sceneMan.GetComponent<InputManager>().pickupTrapText.SetActive(false);
                        }

                        Destroy(other.gameObject);
                        placedTrap = false;
                        GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("trap-pickup-sound");
                    }
                }
                // Disables trap text when enemy caught in it
                else
                {
                    if (sceneMan.GetComponent<InputManager>().pickupTrapText.activeSelf == true)
                    {
                        sceneMan.GetComponent<InputManager>().pickupTrapText.SetActive(false);
                    }
                }
            }

            //Pickup objective item
            else if (other.gameObject.tag == "ObjectiveItem")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if (sceneMan.GetComponent<InputManager>().pickupObjectiveItemText.activeSelf == true)
                    {
                        sceneMan.GetComponent<InputManager>().pickupObjectiveItemText.SetActive(false);
                    }
                    
                    Destroy(other.gameObject);
                    objectiveItemsCollected++;
                    sceneMan.GetComponent<ObjectiveManager>().UpdateUICollectedText();
                    GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("item-pickup-sound");

                    if (objectiveItemsCollected >= 5)
                    {
                        sceneMan.GetComponent<InputManager>().buttonPromptText.SetActive(true);
                        sceneMan.GetComponent<InputManager>().cutscene3Text.SetActive(true);
                        sceneMan.GetComponent<InputManager>().dialogueBackground.SetActive(true);
                        sceneMan.GetComponent<SceneMan>().gameState = GameState.Cutscene3;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Game state
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            // Trap pickup prompt
            if (other.gameObject.tag == "Trap")
            {
                if (sceneMan.GetComponent<InputManager>().pickupTrapText.activeSelf == true)
                {
                    sceneMan.GetComponent<InputManager>().pickupTrapText.SetActive(false);
                }
            }

            // Objective item pickup prompt
            if (other.gameObject.tag == "ObjectiveItem")
            {
                if (sceneMan.GetComponent<InputManager>().pickupObjectiveItemText.activeSelf == true)
                {
                    sceneMan.GetComponent<InputManager>().pickupObjectiveItemText.SetActive(false);
                }
            }
        }
    }
}
