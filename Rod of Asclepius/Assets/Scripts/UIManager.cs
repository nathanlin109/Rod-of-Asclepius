using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Fields
    public GameObject mainCanvas;
    public GameObject loreCanvas;
    public GameObject creditsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // Plays menu theme
        if ((SceneManager.GetActiveScene().name == "MainMenu" ||
            SceneManager.GetActiveScene().name == "Win") &&
            GameObject.Find("AudioManager").GetComponent<AudioMan>().mainTheme.source.isPlaying == false)
        {
            GameObject.Find("AudioManager").GetComponent<AudioMan>().mainTheme.source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunGameScene()
    {
        PlayClickSound();
        if (GameObject.Find("AudioManager").GetComponent<AudioMan>().mainTheme.source.isPlaying == true)
        {
            GameObject.Find("AudioManager").GetComponent<AudioMan>().mainTheme.source.Stop();
        }
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void RunMenuScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        PlayClickSound();
        Application.Quit();
    }

    public void OpenLore()
    {
        PlayClickSound();
        GameObject.Find("MainMenuCanvas/LoreButton").GetComponent<ButtonHover>().spriteIndex = 0;
        GameObject.Find("MainMenuCanvas/LoreButton").GetComponent<Image>().sprite = GameObject.Find("MainMenuCanvas/LoreButton").GetComponent<ButtonHover>().buttonSprites[0];
        GameObject.Find("MainMenuCanvas/LoreButton").GetComponentInChildren<Text>().color = GameObject.Find("MainMenuCanvas/LoreButton").GetComponent<ButtonHover>().buttonColors[0];
        mainCanvas.SetActive(false);
        loreCanvas.SetActive(true);
    }

    public void OpenCredits()
    {
        PlayClickSound();
        GameObject.Find("MainMenuCanvas/CreditsButton").GetComponent<ButtonHover>().spriteIndex = 0;
        GameObject.Find("MainMenuCanvas/CreditsButton").GetComponent<Image>().sprite = GameObject.Find("MainMenuCanvas/CreditsButton").GetComponent<ButtonHover>().buttonSprites[0];
        GameObject.Find("MainMenuCanvas/CreditsButton").GetComponentInChildren<Text>().color = GameObject.Find("MainMenuCanvas/CreditsButton").GetComponent<ButtonHover>().buttonColors[0];
        mainCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void CloseCreditsLore()
    {
        PlayClickSound();
        if (creditsCanvas.activeSelf)
        {
            GameObject.Find("CreditsCanvas/BackButton").GetComponent<ButtonHover>().spriteIndex = 0;
            GameObject.Find("CreditsCanvas/BackButton").GetComponent<Image>().sprite = GameObject.Find("CreditsCanvas/BackButton").GetComponent<ButtonHover>().buttonSprites[0];
            GameObject.Find("CreditsCanvas/BackButton").GetComponentInChildren<Text>().color = GameObject.Find("CreditsCanvas/BackButton").GetComponent<ButtonHover>().buttonColors[0];
            creditsCanvas.SetActive(false);
        }
        else if (loreCanvas.activeSelf)
        {
            GameObject.Find("LoreCanvas/BackButton").GetComponent<ButtonHover>().spriteIndex = 0;
            GameObject.Find("LoreCanvas/BackButton").GetComponent<Image>().sprite = GameObject.Find("LoreCanvas/BackButton").GetComponent<ButtonHover>().buttonSprites[0];
            GameObject.Find("LoreCanvas/BackButton").GetComponentInChildren<Text>().color = GameObject.Find("LoreCanvas/BackButton").GetComponent<ButtonHover>().buttonColors[0];
            loreCanvas.SetActive(false);
        }
        mainCanvas.SetActive(true);
    }

    public void Pause()
    {
        PlayClickSound();
        GameObject.Find("Player").GetComponent<Player>().moveVector = Vector3.zero;
        GameObject sceneManager = GameObject.Find("SceneManager");
        sceneManager.GetComponent<SceneMan>().gameStateBeforePause = sceneManager.GetComponent<SceneMan>().gameState;
        sceneManager.GetComponent<SceneMan>().gameState = GameState.Pause;
        if (sceneManager.GetComponent<InputManager>().pauseCanvas.activeSelf == false)
        {
            sceneManager.GetComponent<InputManager>().pauseCanvas.SetActive(true);
        }
        if (sceneManager.GetComponent<InputManager>().pauseButton.activeSelf == true)
        {
            sceneManager.GetComponent<InputManager>().pauseButton.SetActive(false);
            /*sceneManager.GetComponent<InputManager>().pauseButton.GetComponent<ButtonHover>().spriteIndex = 0;
            sceneManager.GetComponent<InputManager>().pauseButton.GetComponent<Image>().sprite = sceneManager.GetComponent<InputManager>().pauseButton.GetComponent<ButtonHover>().buttonSprites[0];
            sceneManager.GetComponent<InputManager>().pauseButton.GetComponentInChildren<Text>().color = sceneManager.GetComponent<InputManager>().pauseButton.GetComponent<ButtonHover>().buttonColors[0];*/
        }
    }

    public void UnPause()
    {
        PlayClickSound();
        GameObject sceneManager = GameObject.Find("SceneManager");
        sceneManager.GetComponent<SceneMan>().gameState = sceneManager.GetComponent<SceneMan>().gameStateBeforePause;
        if (sceneManager.GetComponent<InputManager>().pauseCanvas.activeSelf == true)
        {
            /*GameObject.Find("PauseCanvas/ResumeButton").GetComponent<ButtonHover>().spriteIndex = 0;
            GameObject.Find("PauseCanvas/ResumeButton").GetComponent<Image>().sprite = GameObject.Find("PauseCanvas/ResumeButton").GetComponent<ButtonHover>().buttonSprites[0];
            GameObject.Find("PauseCanvas/ResumeButton").GetComponentInChildren<Text>().color = GameObject.Find("PauseCanvas/ResumeButton").GetComponent<ButtonHover>().buttonColors[0];
            GameObject.Find("PauseCanvas/MenuButton").GetComponent<ButtonHover>().spriteIndex = 0;
            GameObject.Find("PauseCanvas/MenuButton").GetComponent<Image>().sprite = GameObject.Find("PauseCanvas/MenuButton").GetComponent<ButtonHover>().buttonSprites[0];
            GameObject.Find("PauseCanvas/MenuButton").GetComponentInChildren<Text>().color = GameObject.Find("PauseCanvas/MenuButton").GetComponent<ButtonHover>().buttonColors[0];*/
            sceneManager.GetComponent<InputManager>().pauseCanvas.SetActive(false);
        }
        if (sceneManager.GetComponent<InputManager>().pauseButton.activeSelf == false)
        {
            sceneManager.GetComponent<InputManager>().pauseButton.SetActive(true);
        }
    }

    private void PlayClickSound()
    {
        FindObjectOfType<AudioMan>().Play("button-sound");
    }
}
