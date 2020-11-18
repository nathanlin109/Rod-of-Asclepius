using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    private GameObject sceneMan;
    private GameObject player;
    public GameObject objectiveItemsCollectedText;

    // Start is called before the first frame update
    void Start()
    {
        sceneMan = GameObject.Find("SceneManager");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Game
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Game)
        {
            objectiveItemsCollectedText.SetActive(true);
        }
    }

    // Update the objective items collected text
    public void UpdateOICollectedText()
    {
        objectiveItemsCollectedText.GetComponent<Text>().text = "Ritual Items (" + player.GetComponent<Player>().objectiveItemsCollected / 2 + "/3)";
    }
}
