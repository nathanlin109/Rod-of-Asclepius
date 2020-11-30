using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Cutscene1, Cutscene2, Cutscene3, Cutscene4, Cutscene5,
    Game, GameNoCombat, Pause, Death }

public class SceneMan : MonoBehaviour
{
    // Fields
    public GameState gameState;
    public bool resurrectedMom;
    public GameObject birdParticles;
    public GameObject birdLight;
    public GameObject skullParticles;
    public GameObject skullLight;
    public GameObject flowerParticles;
    public GameObject flowerLight;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Cutscene1;
        resurrectedMom = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
