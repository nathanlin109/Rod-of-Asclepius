using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Fields
    GameObject player;
    SceneMan sceneMan;
    public float moveSpeed;

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
        // Movements
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(new Vector3(0, 1.0f, 0) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(new Vector3(-1.0f, 0, 0) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(new Vector3(0, -1.0f, 0) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(new Vector3(1.0f, 0, 0) * moveSpeed * Time.deltaTime);
        }
    }
}
