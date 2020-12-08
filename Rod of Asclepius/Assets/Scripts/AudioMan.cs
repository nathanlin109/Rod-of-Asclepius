using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMan : MonoBehaviour
{
    // Fields
    public static AudioMan instance;
    public Sound mainTheme;
    public Sound[] sounds;

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

        mainTheme = sounds[sounds.Length - 1];
    }

    // Starts playing main theme
    private void Start()
    {

    }

    void Update()
    {
        
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
