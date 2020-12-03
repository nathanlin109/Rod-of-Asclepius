using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    // Fields
    public float inputDelayTime = 1f;
    private float inputDelayTimer;
    private GameObject sceneMan;
    private GameObject player;
    public GameObject vampire;
    public GameObject wizard;
    public GameObject mother;
    public GameObject blood;
    public GameObject buttonPromptText;
    public GameObject dialogueBackground;
    public GameObject cutscene1_1Text;
    public GameObject cutscene1_2Text;
    public GameObject cutscene1_3Text;
    public GameObject cutscene1_4Text;
    public GameObject cutscene2_1Text;
    public GameObject cutscene2_2Text;
    public GameObject cutscene2_3Text;
    public GameObject cutscene2_4Text;
    public GameObject cutscene2_5Text;
    public GameObject cutscene2_6Text;
    public GameObject cutscene3Text;
    public GameObject cutscene4_1Text;
    public GameObject cutscene4_2Text;
    public GameObject cutscene4_3Text;
    public GameObject cutscene4_4Text;
    public GameObject cutscene4_5Text;
    public GameObject cutscene5_1Text;
    public GameObject cutscene5_2Text;
    public GameObject cutscene5_3Text;
    public GameObject cutscene5_4Text;
    public GameObject pickupTrapText;
    public GameObject pickupObjectiveItemText;

    // Start is called before the first frame update
    void Start()
    {
        sceneMan = GameObject.Find("SceneManager");
        player = GameObject.Find("Player");
        inputDelayTimer = 0;
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
            inputDelayTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && inputDelayTimer >= inputDelayTime)
            {
                inputDelayTimer = 0;
                if (cutscene1_1Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene1_1Text.SetActive(false);
                    cutscene1_2Text.SetActive(true);
                }
                else if (cutscene1_2Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene1_2Text.SetActive(false);
                    buttonPromptText.SetActive(false);
                    dialogueBackground.SetActive(false);
                    player.GetComponent<Player>().cutscene1ShouldMove = true;
                }
                else if (cutscene1_3Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene1_3Text.SetActive(false);
                    cutscene1_4Text.SetActive(true);
                }
                else if (cutscene1_4Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene1_4Text.SetActive(false);
                    buttonPromptText.SetActive(false);
                    dialogueBackground.SetActive(false);
                    sceneMan.GetComponent<SceneMan>().gameState = GameState.GameNoCombat;
                }
            }
        }
        // Cutscene2 to Game
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene2)
        {
            inputDelayTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && inputDelayTimer >= inputDelayTime)
            {
                inputDelayTimer = 0;
                if (cutscene2_1Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene2_1Text.SetActive(false);
                    cutscene2_2Text.SetActive(true);
                }
                else if (cutscene2_2Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene2_2Text.SetActive(false);
                    buttonPromptText.SetActive(false);
                    player.GetComponent<Player>().cutscene2ShouldRotateVampire = true;
                    dialogueBackground.SetActive(false);
                    vampire.SetActive(true);

                    // Plays sound
                    GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("vampire-intro-sound");
                }
                else if (cutscene2_3Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene2_3Text.SetActive(false);
                    cutscene2_4Text.SetActive(true);
                    wizard.SetActive(true);


                    // Plays sound
                    GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("wizard-intro-sound");
                }
                else if (cutscene2_4Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene2_4Text.SetActive(false);
                    cutscene2_5Text.SetActive(true);
                }
                else if (cutscene2_5Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene2_5Text.SetActive(false);
                    cutscene2_6Text.SetActive(true);
                    dialogueBackground.SetActive(false);
                }
                else if (cutscene2_6Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene2_6Text.SetActive(false);
                    buttonPromptText.SetActive(false);
                    sceneMan.GetComponent<SceneMan>().gameState = GameState.Game;
                    sceneMan.GetComponent<SceneMan>().EnableObjectiveItemParticles();
                }
            }
        }
        // Cutscene3 to Game (Collected all items and need to bring to mother's grave
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene3)
        {
            inputDelayTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && inputDelayTimer >= inputDelayTime)
            {
                inputDelayTimer = 0;
                cutscene3Text.SetActive(false);
                buttonPromptText.SetActive(false);
                dialogueBackground.SetActive(false);
                sceneMan.GetComponent<SceneMan>().gameState = GameState.Game;
            }
        }
        // Cutscene4 to Game
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene4)
        {
            inputDelayTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && inputDelayTimer >= inputDelayTime)
            {
                inputDelayTimer = 0;
                if (cutscene4_1Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene4_1Text.SetActive(false);
                    cutscene4_2Text.SetActive(true);

                    // Sets wizard/vampire position
                    if (Vector3.Distance(player.transform.position, vampire.transform.position) >= 15f)
                    {
                        vampire.transform.position = new Vector3(142, 1.1f, 63);
                    }
                    if (Vector3.Distance(player.transform.position, wizard.transform.position) >= 15f)
                    {
                        wizard.transform.position = new Vector3(153, 1.1f, 80);
                    }

                    // Plays the particles
                    GameObject.Find("MotherParticles/MotherAmbientParticles1").GetComponent<ParticleSystem>().Clear();
                    GameObject.Find("MotherParticles/MotherAmbientParticles1").GetComponent<ParticleSystem>().Play();
                    GameObject.Find("MotherParticles/MotherAmbientParticles2").GetComponent<ParticleSystem>().Clear();
                    GameObject.Find("MotherParticles/MotherAmbientParticles2").GetComponent<ParticleSystem>().Play();
                    GameObject.Find("MotherParticles/MotherParticlesRing").GetComponent<ParticleSystem>().Clear();
                    GameObject.Find("MotherParticles/MotherParticlesRing").GetComponent<ParticleSystem>().Play();

                    // Plays sound
                    GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("resurrection-aura-sound");
                }
                else if (cutscene4_2Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene4_2Text.SetActive(false);
                    cutscene4_3Text.SetActive(true);

                    // Plays the particles
                    GameObject.Find("MotherParticles/MotherRessurectionParticles").GetComponent<ParticleSystem>().Clear();
                    GameObject.Find("MotherParticles/MotherRessurectionParticles").GetComponent<ParticleSystem>().Play();
                    mother.SetActive(true);

                    // Plays sound
                    GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("resurrection-complete-sound");
                }
                else if (cutscene4_3Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene4_3Text.SetActive(false);
                    cutscene4_4Text.SetActive(true);

                    // Stops the particles
                    GameObject.Find("MotherParticles/MotherAmbientParticles1").GetComponent<ParticleSystem>().Stop();
                    GameObject.Find("MotherParticles/MotherAmbientParticles2").GetComponent<ParticleSystem>().Stop();
                    GameObject.Find("MotherParticles/MotherParticlesRing").GetComponent<ParticleSystem>().Stop();
                    GameObject.Find("MotherParticles/MotherRessurectionParticles").GetComponent<ParticleSystem>().Stop();

                    // Stops ambient sound
                    Sound resurrectAmbientSound = Array.Find(GameObject.Find("AudioManager").GetComponent<AudioMan>().sounds, sound => sound.name == "resurrection-aura-sound");
                    if (resurrectAmbientSound != null && resurrectAmbientSound.source.isPlaying == true)
                    {
                        resurrectAmbientSound.source.Stop();
                    }
                }
                else if (cutscene4_4Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene4_4Text.SetActive(false);
                    cutscene4_5Text.SetActive(true);
                }
                else if (cutscene4_5Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene4_5Text.SetActive(false);
                    buttonPromptText.SetActive(false);
                    dialogueBackground.SetActive(false);
                    sceneMan.GetComponent<SceneMan>().gameState = GameState.Game;
                    sceneMan.GetComponent<SceneMan>().resurrectedMom = true;
                }
            }
        }
        // Cutscene5 to End
        else if (sceneMan.GetComponent<SceneMan>().gameState == GameState.Cutscene5)
        {
            inputDelayTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && inputDelayTimer >= inputDelayTime)
            {
                inputDelayTimer = 0;
                if (cutscene5_1Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene5_1Text.SetActive(false);
                    cutscene5_2Text.SetActive(true);
                }
                else if (cutscene5_2Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene5_2Text.SetActive(false);
                    cutscene5_3Text.SetActive(true);
                }
                else if (cutscene5_3Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene5_3Text.SetActive(false);
                    cutscene5_4Text.SetActive(true);
                }
                else if (cutscene5_4Text.activeSelf == true && buttonPromptText.activeSelf == true)
                {
                    cutscene5_4Text.SetActive(false);
                    buttonPromptText.SetActive(false);
                    dialogueBackground.SetActive(false);
                    player.GetComponent<Player>().cutscene5ShouldMove = true;
                    sceneMan.GetComponent<SceneMan>().wonGame = true;
                }
            }
        }
    }
}
