using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] EventReference LevelMusic;
    [SerializeField] float rate;
    [SerializeField] float volume;
    [SerializeField] GameObject audioPlayer;
    // [SerializeField] EventReference ButtonClick;

    float time;
    
    public static AudioManager Instance { get; private set; }

    [EventRef] // Attribute to easily select FMOD events in the Inspector
    public string myOneShotSoundEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    private void Start()
    {
        PlayLevelMusic();
    }
	
	// public void PlayButtonClick()
	// {
	
	// }

    public void PlayLevelMusic()
    {
    RuntimeManager.PlayOneShot(LevelMusic);
    }
}