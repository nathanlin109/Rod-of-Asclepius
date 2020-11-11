using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Fields
    public int health;
    SceneMan sceneMan;
    Vector3 middleModel;
    public float moveSpeed;
    private Vector3 moveVector;

    // Collisions
    public float playerImmuneTime;
    private float immunityTimer;
    private float flashingTimer;
    public bool hasCollided;

    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        sceneMan = GameObject.Find("SceneManager").GetComponent<SceneMan>();
        middleModel = gameObject.transform.position +
            new Vector3(0, gameObject.GetComponent<BoxCollider>().bounds.size.y / 2, 0);
        moveVector = Vector3.zero;
        hasCollided = false;
        immunityTimer = 0;
        flashingTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneMan.gameState == GameState.Game)
        {
            KeyBoardInputs();
            MouseInputs();
            CollisionCooldown();
            Debug.Log(health);
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
        if (other.gameObject.tag == "HealingItem" && health == 1)
        {
            health++;
            Destroy(other.gameObject);
        }
    }
}
