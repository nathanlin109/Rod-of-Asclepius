using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioMan : MonoBehaviour
{
    // Fields
    public Sound[] sounds;

    public static AudioMan instance;
    private Sound mainTheme;
    private int mainThemeIndex;

    // Start is called before the first frame update
    void Awake()
    {
        // Ensures only 1 instance of audio manager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Creates sound objects
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }
    }

    // Starts playing main theme
    private void Start()
    {
        /*mainThemeIndex = 3 + UnityEngine.Random.Range(0, 3);
        mainTheme = sounds[mainThemeIndex];
        mainTheme.source.Play();*/
    }

    void Update()
    {
        // Chooses the next theme to play
        /*if (!mainTheme.source.isPlaying)
        {
            // Ensures that a new theme plays
            int nextIndex = 3 + UnityEngine.Random.Range(0, 3);
            while (nextIndex == mainThemeIndex)
            {
                nextIndex = 3 + UnityEngine.Random.Range(0, 3);
            }
            mainThemeIndex = nextIndex;

            mainTheme = sounds[mainThemeIndex];
            mainTheme.source.Play();
        }*/
    }

    // Plays any audio clip
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
