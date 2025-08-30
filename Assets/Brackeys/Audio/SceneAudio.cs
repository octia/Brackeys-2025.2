using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class SceneAudio : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private EventReference musicEvent;
    [SerializeField] private float musicFadeTime = 1f;
    [SerializeField] private float delayBeforeStart = 1f;

    [Header("Ambient Settings")]
    [SerializeField] private EventReference ambientEvent;
    [SerializeField] private float ambientFadeTime = 1f;

    [Header("Minigame Settings")]
    [SerializeField] private EventReference minigameEvent;
    [SerializeField] private float minigameFadeTime = 1f;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        string scene = SceneManager.GetActiveScene().name;

        if (scene == "MainMenu")
        {
            // Music + Ambient full volume instantly
            AudioManager.Instance.PlayMusicInstant(musicEvent);
            AudioManager.Instance.StartAmbient(ambientEvent);
            AudioManager.Instance.FadeAmbient(AudioManager.Instance.DefaultAmbientVolume, 0f);
        }
        else // Gameplay or other scenes
        {
            // Music crossfade in
            AudioManager.Instance.PlayMusic(musicEvent, musicFadeTime, delayBeforeStart);

            // Ambient + Minigame start muted
            AudioManager.Instance.StartAmbient(ambientEvent);
            AudioManager.Instance.StartMinigame(minigameEvent);
        }
    }

    // --- Minigame Controls ---
    public void OnMinigameStart()
    {
        AudioManager.Instance.FadeAmbient(AudioManager.Instance.DefaultAmbientVolume, ambientFadeTime);
        AudioManager.Instance.FadeMinigame(AudioManager.Instance.DefaultMinigameVolume, minigameFadeTime);
    }

    public void OnMinigameEnd()
    {
        AudioManager.Instance.FadeAmbient(0f, ambientFadeTime);
        AudioManager.Instance.FadeMinigame(0f, minigameFadeTime);
    }
}
