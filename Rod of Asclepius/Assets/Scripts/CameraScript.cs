using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Fields
    GameObject player;
    SceneMan sceneMan;
    public float moveSpeed;
    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sceneMan = GameObject.Find("SceneManager").GetComponent<SceneMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneMan.gameState == GameState.Game)
        {
            KeyBoardInputs();
        }
    }

    // Process keyboard inputs
    void KeyBoardInputs()
    {
        moveVector = Vector3.zero;

        // Movements
        if (Input.GetKey(KeyCode.W))
        {
            moveVector += new Vector3(0, 1.0f, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector += new Vector3(-1.0f, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector += new Vector3(0, -1.0f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector += new Vector3(1.0f, 0, 0);
        }

        // Applies the transformation
        moveVector.Normalize();
        gameObject.transform.Translate(moveVector * moveSpeed * Time.deltaTime);
    }
}
