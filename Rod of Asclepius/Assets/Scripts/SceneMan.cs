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
}
