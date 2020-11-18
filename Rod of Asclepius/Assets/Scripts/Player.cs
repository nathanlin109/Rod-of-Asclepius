using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Trap/ability
    public GameObject trapPrefab;
    private float trapDeployTimer;
    public float trapDeployTime;
    private float trapMoveSpeedMultiplier;
    private bool placedTrap;

    // Objectives
    public int objectiveItemsCollected;
    public GameObject pickupObjectiveItemText;

    // Movement during UI prompts
    public bool shouldMove; // Set false when trigger UI prompt, set true when press space on UI prompt

    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        sceneMan = GameObject.Find("SceneManager");
        middleModel = gameObject.transform.position +
            new Vector3(0, gameObject.GetComponent<BoxCollider>().bounds.size.y / 2, 0);
        moveVector = Vector3.zero;
        hasCollided = false;
        immunityTimer = 0;
        flashingTimer = 0;
        shouldMove = true;
        trapDeployTimer = 0;
        trapMoveSpeedMultiplier = 1;
        placedTrap = false;
        objectiveItemsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Game Movement
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            MovementKeyBoardInputs();
            MouseInputs();
            TrapAbilityKeyboardInputs();
            CollisionCooldown();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.GameNoCombat && shouldMove)
        {
            MovementKeyBoardInputs();
            MouseInputs();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene1 && shouldMove == true)
        {
            GetComponent<Rigidbody>().transform.Translate(new Vector3(0, 0, 1.0f) * moveSpeed / 2 * Time.deltaTime, Space.World);
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

    // Traps/abilities
    void TrapAbilityKeyboardInputs()
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
        }

        // Cutscene1 to GameNoCombat trigger
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene1)
        {
            if (other.gameObject.tag == "TriggerOpenGate")
            {
                GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Open");
            }
            else if (other.gameObject.tag == "TriggerCloseGate")
            {
                GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Close");
                sceneMan.GetComponent<InputManager>().ButtonPromptText.SetActive(true);
                sceneMan.GetComponent<InputManager>().cutscene1Text.SetActive(true);
                shouldMove = false;
            }
        }

        // GameNoCombat to Cutscene2 trigger
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.GameNoCombat)
        {
            if (other.gameObject.tag == "TriggerMotherGrave")
            {
                sceneMan.GetComponent<InputManager>().ButtonPromptText.SetActive(true);
                sceneMan.GetComponent<InputManager>().cutscene2Text.SetActive(true);
                shouldMove = false;

                // find the angle to rotate the player so that it points towards the mother's grave
                GameObject coffin = GameObject.Find("StaticLevelAssets/OtherObjects/Coffin Closed Standing");
                float angleOfRotation = Mathf.Atan2(coffin.transform.position.z - gameObject.transform.position.z,
                    coffin.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg - 90.0f;

                GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, -angleOfRotation, 0);
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
