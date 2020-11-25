using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    // Player Fields
    public int health;
    GameObject sceneMan;
    Vector3 middleModel;
    public float moveSpeed;
    private Vector3 moveVector;

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

    // Objectives
    public int objectiveItemsCollected;
    public GameObject pickupObjectiveItemText;

    // Cutscene1 movement
    public bool cutscene1ShouldMove;

    // Cutscene2 movement
    public bool cutscene2ShouldRotateCoffin;
    public bool cutscene2ShouldRotateVampire;

    // Enemies
    public GameObject vampire;
    public GameObject wizard;

    // Start is called before the first frame update
    void Start()
    {
        // Own fields
        health = 2;
        cutscene1ShouldMove = false;
        cutscene2ShouldRotateCoffin = false;
        cutscene2ShouldRotateVampire = false;
        sceneMan = GameObject.Find("SceneManager");
        middleModel = gameObject.transform.position +
            new Vector3(0, gameObject.GetComponent<BoxCollider>().bounds.size.y / 2, 0);
        moveVector = Vector3.zero;

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
}

// Update is called once per frame
void Update()
    {
        // Game Movement
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            MovementKeyBoardInputs();
            MouseInputs();
            CollisionCooldown();
            PlaceTrap();
            ThrowAbility();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.GameNoCombat)
        {
            MovementKeyBoardInputs();
            MouseInputs();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene1 && cutscene1ShouldMove == true)
        {
            GetComponent<Rigidbody>().transform.Translate(new Vector3(0, 0, 1.0f) * moveSpeed / 2 * Time.deltaTime, Space.World);
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene2)
        {
            RotatePlayerCutscene2();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene4)
        {
            GetComponent<Rigidbody>().transform.Translate(new Vector3(0, 0, -1.0f) * moveSpeed / 2 * Time.deltaTime, Space.World);
        }
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
    void RotatePlayerCutscene2()
    {
        if (cutscene2ShouldRotateCoffin == true)
        {
            // find the angle to rotate the player so that it points towards the mother's grave
            GameObject coffin = GameObject.Find("StaticLevelAssets/OtherObjects/Coffin Closed Standing");
            Vector3 targetForwardDirection = (coffin.transform.position - transform.position);
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

            Debug.Log(angleBetweenRotation);
            // Rotates the player
            if (angleBetweenRotation < 0)
            {
                transform.Rotate(-Vector3.up * Time.deltaTime * 400);
            }
            else
            {
                transform.Rotate(Vector3.up * Time.deltaTime * 400);
            }

            // Stops rotating player and shows cutscene2 text
            if (Mathf.Abs(angleBetweenRotation) <= 4)
            {
                cutscene2ShouldRotateCoffin = false;
                sceneMan.GetComponent<InputManager>().ButtonPromptText.SetActive(true);
                sceneMan.GetComponent<InputManager>().cutscene2_1Text.SetActive(true);
            }
        }
        else if (cutscene2ShouldRotateVampire == true)
        {
            // find the angle to rotate the player so that it points towards the tree
            GameObject tree = GameObject.Find("StaticLevelAssets/Graves/TopLeft/GraveRow (2)/gravestone_04 (7)");
            Vector3 targetForwardDirection = (tree.transform.position - transform.position);
            targetForwardDirection.y = 0;
            targetForwardDirection.Normalize();

            // Rotates the player
            transform.Rotate(-Vector3.up * Time.deltaTime * 400);

            float angleBetweenRotation = Mathf.Atan2(transform.forward.normalized.z,
                transform.forward.normalized.x) * Mathf.Rad2Deg - Mathf.Atan2(targetForwardDirection.z, targetForwardDirection.x) * Mathf.Rad2Deg;

            // Stops rotating player and shows cutscene2 text
            if (Mathf.Abs(angleBetweenRotation) <= 3)
            {
                cutscene2ShouldRotateVampire = false;
                sceneMan.GetComponent<InputManager>().cutscene2_3Text.SetActive(true);
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

            // Deploys trap
            if (trapDeployTimer >= trapDeployTime)
            {
                trapDeployTimer = 0;
                trapMoveSpeedMultiplier = 1;
                placedTrap = true;

                GameObject.Instantiate(trapPrefab,
                    new Vector3(transform.position.x,
                    .25f,
                    transform.position.z),
                    Quaternion.identity);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            trapDeployTimer = 0;
            trapMoveSpeedMultiplier = 1;
        }
    }

    // Throw
    void ThrowAbility()
    {
        // Increments timers
        flareTimeTillCooldown += Time.deltaTime;
        silenceTimeTillCooldown += Time.deltaTime;
        iceTimeTillCooldown += Time.deltaTime;

        // Flare
        if (flareTimeTillCooldown >= flareCooldown && Input.GetMouseButtonDown(1) && trapDeployTimer == 0)
        {
            GameObject flare = Instantiate(flarePrefab, transform.position, Quaternion.identity);
            flare.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed + moveVector * moveSpeed / 4, ForceMode.Impulse);
            flareTimeTillCooldown = 0;
        }

        // Silence
        if (silenceTimeTillCooldown >= silenceCooldown && Input.GetMouseButtonDown(2) && trapDeployTimer == 0)
        {
            GameObject silence = Instantiate(silencePrefab, transform.position, Quaternion.identity);
            silence.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed + moveVector * moveSpeed / 4, ForceMode.Impulse);
            silenceTimeTillCooldown = 0;
        }

        // Ice
        if (iceTimeTillCooldown >= iceCooldown && Input.GetMouseButtonDown(0) && trapDeployTimer == 0)
        {
            GameObject silence = Instantiate(icePrefab, transform.position, Quaternion.identity);
            silence.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed + moveVector * moveSpeed / 4, ForceMode.Impulse);
            iceTimeTillCooldown = 0;
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
                GetComponent<MeshRenderer>().enabled = true;
            }

            // Flashes player
            if (flashingTimer >= .25f)
            {
                GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
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
                if (pickupObjectiveItemText.activeSelf == false)
                {
                    pickupObjectiveItemText.SetActive(true);
                }
            }

            // Game to Cutscene3 (Trigger Mother's grave)
            if (other.gameObject.tag == "TriggerMotherGrave" && objectiveItemsCollected == 6)
            {
                sceneMan.GetComponent<InputManager>().ButtonPromptText.SetActive(true);
                sceneMan.GetComponent<InputManager>().cutscene3Text.SetActive(true);
                sceneMan.GetComponent<SceneMan>().gameState = GameState.Cutscene3;
                objectiveItemsCollected++;

                // find the angle to rotate the player so that it points towards the mother's grave
                GameObject coffin = GameObject.Find("StaticLevelAssets/OtherObjects/Coffin Closed Standing");
                float angleOfRotation = Mathf.Atan2(coffin.transform.position.z - gameObject.transform.position.z,
                    coffin.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg - 90.0f;

                GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, -angleOfRotation, 0);
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
                    GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Close");
                    
                }
                else if(other.gameObject.tag == "TriggerOpenGateBeginning")
                {
                    sceneMan.GetComponent<InputManager>().ButtonPromptText.SetActive(true);
                    sceneMan.GetComponent<InputManager>().cutscene4Text.SetActive(true);
                    sceneMan.GetComponent<SceneMan>().gameState = GameState.Cutscene4;
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
                sceneMan.GetComponent<InputManager>().ButtonPromptText.SetActive(true);
                sceneMan.GetComponent<InputManager>().cutscene1_3Text.SetActive(true);
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

    private void OnTriggerStay(Collider other)
    {
        // Game state
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            // Pickup trap
            if (other.gameObject.tag == "Trap")
            {
                if (other.gameObject.GetComponent<Item>().enemyCurrentlyCaught == false && Input.GetKey(KeyCode.E))
                {
                    if (sceneMan.GetComponent<InputManager>().pickupTrapText.activeSelf == true)
                    {
                        sceneMan.GetComponent<InputManager>().pickupTrapText.SetActive(false);
                    }

                    Destroy(other.gameObject);
                    placedTrap = false;
                }
            }

            //Pickup objective item
            else if (other.gameObject.tag == "ObjectiveItem")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if (pickupObjectiveItemText.activeSelf == true)
                    {
                        pickupObjectiveItemText.SetActive(false);
                    }
                    
                    Destroy(other.gameObject);
                    objectiveItemsCollected++;
                    sceneMan.GetComponent<ObjectiveManager>().UpdateOICollectedText();
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
                if (pickupObjectiveItemText.activeSelf == true)
                {
                    pickupObjectiveItemText.SetActive(false);
                }
            }
        }
    }
}
