using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Fields
    int health;
    SceneMan sceneMan;
    Vector3 middleModel;

    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        sceneMan = GameObject.Find("SceneManager").GetComponent<SceneMan>();
        middleModel = gameObject.transform.position +
            new Vector3(0, gameObject.GetComponent<CapsuleCollider>().bounds.size.y / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneMan.gameState == GameState.Game)
        {
            MouseInputs();
        }
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

        gameObject.transform.rotation = Quaternion.Euler(0, -angleOfRotation, 0);
        Debug.Log("Player Pos: " + gameObject.transform.position);
        Debug.Log("Mouse Pos: " + mouseWorldPos);
        Debug.Log("Angle: " + angleOfRotation);
    }
}
