using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Fields
    private GameObject sceneMan;
    public GameObject ButtonPromptText;
    public GameObject cutscene1Text;

    // Start is called before the first frame update
    void Start()
    {
        sceneMan = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        UIKeyboardInputs();
    }

    // Keyboard UI Inputs
    void UIKeyboardInputs()
    {
        // Cutscene1
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
    }
}
