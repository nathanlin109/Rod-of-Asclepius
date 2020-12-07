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

    private void PlayClickSound()
    {
        FindObjectOfType<AudioMan>().Play("button-sound");
    }
}
