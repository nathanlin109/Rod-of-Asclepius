using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Cutscene1, Cutscene2, Cutscene3, Cutscene4, Cutscene5,
    Game, GameNoCombat, Pause, Death, Win }


public class SceneMan : MonoBehaviour
{
    // Fields
    public GameState gameState;
    public GameState gameStateBeforePause;
    public bool resurrectedMom;

    // Objective particles
    public GameObject birdParticles;
    public GameObject birdLight;
    public GameObject skullParticles;
    public GameObject skullLight;
    public GameObject flowerParticles;
    public GameObject flowerLight;

    // Cursor
    public Texture2D cursorTexture;

    // Win game
    public GameObject blackFadeTexture;
    public float fadeMultiplier = .1f;
    private bool playedWinSound;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Cutscene1;
        resurrectedMom = false;
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);
        GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("background-sounds");
        playedWinSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        FadeToBlack();
    }

    // Enables objective particles
    public void EnableObjectiveItemParticles()
    {
        birdParticles.SetActive(true);
        birdLight.SetActive(true);
        skullParticles.SetActive(true);
        skullLight.SetActive(true);
        flowerParticles.SetActive(true);
        flowerLight.SetActive(true);
    }

    // Fades to black when finished with game
    void FadeToBlack()
    {
        if (gameState == GameState.Win)
        {
            Color fadeColor = blackFadeTexture.GetComponent<Image>().color;
            fadeColor.a += Time.deltaTime * fadeMultiplier;
            blackFadeTexture.GetComponent<Image>().color = fadeColor;

            if (playedWinSound == false)
            {
                playedWinSound = true;
                GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("win-sound");
            }

            if (fadeColor.a >= .95f)
            {
                SceneManager.LoadScene("Win", LoadSceneMode.Single);

                // Stops ambient sound
                Sound backgroundSound = Array.Find(GameObject.Find("AudioManager").GetComponent<AudioMan>().sounds, sound => sound.name == "background-sounds");
                if (backgroundSound != null && backgroundSound.source.isPlaying == true)
                {
                    backgroundSound.source.Stop();
                }
            }
        }
        else if (gameState == GameState.Death)
        {
            Color fadeColor = blackFadeTexture.GetComponent<Image>().color;
            fadeColor.a += Time.deltaTime * fadeMultiplier;
            blackFadeTexture.GetComponent<Image>().color = fadeColor;

            if (fadeColor.a >= .95f)
            {
                SceneManager.LoadScene("Death", LoadSceneMode.Single);

                // Stops ambient sound
                Sound backgroundSound = Array.Find(GameObject.Find("AudioManager").GetComponent<AudioMan>().sounds, sound => sound.name == "background-sounds");
                if (backgroundSound != null && backgroundSound.source.isPlaying == true)
                {
                    backgroundSound.source.Stop();
                }
            }
        }
    }
}
