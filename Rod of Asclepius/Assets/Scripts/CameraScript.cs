using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Fields
    GameObject player;
    SceneMan sceneMan;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sceneMan = GameObject.Find("SceneManager").GetComponent<SceneMan>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x,
            15,
            player.transform.position.z);
    }
}
