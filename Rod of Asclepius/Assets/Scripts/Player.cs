using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Fields
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

    // Movement during cutscenes
    private bool cutsceneShouldMove;

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
        cutsceneShouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Game Movement
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            KeyBoardInputs();
            MouseInputs();
            CollisionCooldown();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.GameNoCombat)
        {
            KeyBoardInputs();
            MouseInputs();
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene1 && cutsceneShouldMove == true)
        {
            GetComponent<Rigidbody>().transform.Translate(new Vector3(0, 0, 1.0f) * moveSpeed / 2 * Time.deltaTime, Space.World);
        }
    }

    // Process keyboard inputs
    void KeyBoardInputs()
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
        GetComponent<Rigidbody>().transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
    }

    // Process mouse inputs
    void MouseInputs()
    {
        // Gets mosue position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - transform.position.y;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // find the angle to rotate the player so that it points towards the mouse
        float angleOfRotation = Mathf.Atan2(mouseWorldPos.z - gameObject.transform.position.z,
            mouseWorldPos.x - gameObject.transform.position.x) * Mathf.Rad2Deg - 90.0f;

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, -angleOfRotation, 0);
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
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            if (other.gameObject.tag == "HealingItem" && health == 1)
            {
                health++;
                Destroy(other.gameObject);
            }
        }
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene1)
        {
            if (other.gameObject.tag == "OpenGate")
            {
                GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Open");
            }
            else if (other.gameObject.tag == "CloseGate")
            {
                GameObject.Find("gate_01").GetComponent<Animation>().Play("Gate Close");
                sceneMan.GetComponent<InputManager>().ButtonPromptText.SetActive(true);
                sceneMan.GetComponent<InputManager>().cutscene1Text.SetActive(true);
                cutsceneShouldMove = false;
            }
        }
    }
}
