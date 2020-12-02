using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Cutscene1, Cutscene2, Cutscene3, Cutscene4, Cutscene5,
    Game, GameNoCombat, Pause, Death }

public class SceneMan : MonoBehaviour
{
    // Fields
    public GameState gameState;
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
    public bool wonGame;
    public GameObject blackFadeTexture;
    public float fadeMultiplier = .1f;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Cutscene1;
        resurrectedMom = false;
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);
        wonGame = false;
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
        if (wonGame == true)
        {
            Color fadeColor = blackFadeTexture.GetComponent<Image>().color;
            fadeColor.a += Time.deltaTime * fadeMultiplier;
            blackFadeTexture.GetComponent<Image>().color = fadeColor;
        }
    }
}
