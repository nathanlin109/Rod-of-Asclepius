using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mother : MonoBehaviour
{
    // Fields
    private GameObject sceneMan;
    private GameObject player;
    public float playerRadius;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sceneMan = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game ||
            sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene5 &&
            sceneMan.GetComponent<SceneMan>().resurrectedMom == true)
        {
            Vector3 target= player.transform.position - (player.transform.position - transform.position).normalized * playerRadius;
            GetComponent<NavMeshAgent>().destination = target;
        }
        else
        {
            GetComponent<NavMeshAgent>().destination = transform.position;
        }
    }
}
