using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Fields
    private GameObject sceneMan;
    private GameObject player;
    public GameObject mother;
    public GameObject ButtonPromptText;
    public GameObject cutscene1Text;
    public GameObject cutscene2Text;
    public GameObject cutscene3Text;
    public GameObject cutscene4Text;
    public GameObject pickupTrapText;

    // Start is called before the first frame update
    void Start()
    {
        sceneMan = GameObject.Find("SceneManager");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UIKeyboardInputs();
    }

    // Keyboard UI Inputs
    void UIKeyboardInputs()
    {
        // Cutscene1 to GameNoCombat
        if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (cutscene1Text.activeSelf == true && ButtonPromptText.activeSelf == true)
                {
                    cutscene1Text.SetActive(false);
                    ButtonPromptText.SetActive(false);
                    sceneMan.GetComponent<SceneMan>().gameState = GameState.GameNoCombat;
                }
            }
        }
        // Cutscene2 to Game
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene2)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (cutscene2Text.activeSelf == true && ButtonPromptText.activeSelf == true)
                {
                    cutscene2Text.SetActive(false);
                    ButtonPromptText.SetActive(false);
                    sceneMan.GetComponent<SceneMan>().gameState = GameState.Game;
                }
            }
        }
        // Cutscene3 to Game
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene3)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (cutscene3Text.activeSelf == true && ButtonPromptText.activeSelf == true)
                {
                    cutscene3Text.SetActive(false);
                    ButtonPromptText.SetActive(false);
                    sceneMan.GetComponent<SceneMan>().gameState = GameState.Game;
                    sceneMan.GetComponent<SceneMan>().resurrectedMom = true;
                    mother.SetActive(true);
                }
            }
        }
        // Cutscene4 to End
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene4)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
            }
        }
    }
}
