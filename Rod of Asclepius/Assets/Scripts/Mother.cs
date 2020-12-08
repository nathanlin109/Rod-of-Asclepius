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
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sceneMan = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);
        if ((sceneMan.GetComponent<SceneMan>().gameState == GameState.Game ||
            sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene5 ||
            sceneMan.GetComponent<SceneMan>().gameState == GameState.Win) &&
            sceneMan.GetComponent<SceneMan>().resurrectedMom == true)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
            Vector3 target= player.transform.position - (player.transform.position - transform.position).normalized * playerRadius;
            GetComponent<NavMeshAgent>().destination = target;
        }
        else
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        }
    }
}
