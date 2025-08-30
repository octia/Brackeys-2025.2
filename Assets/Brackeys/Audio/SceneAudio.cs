using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class SceneAudio : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private EventReference musicEvent;
    [SerializeField] private float fadeTime = 1.25f;
    [SerializeField] private float delayBeforeStart = 1f;

    [Header("Ambient Settings")]
    [SerializeField] private EventReference ambientEvent;
    [SerializeField] private float ambientFadeTime = 1f;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        string sceneName = SceneManager.GetActiveScene().name;

        // --- Music ---
        if (sceneName == "MainMenu")
            AudioManager.Instance.PlayMusicInstant(musicEvent);
        else
            AudioManager.Instance.PlayMusic(musicEvent, fadeTime, delayBeforeStart);

        // --- Ambient ---
        AudioManager.Instance.StartAmbient(ambientEvent);

        float initialAmbient = (sceneName == "MainMenu")
            ? AudioManager.Instance.DefaultAmbientVolume
            : 0f;

        AudioManager.Instance.FadeAmbient(initialAmbient, ambientFadeTime);
    }

    // --- Minigame Controls ---
    public void OnMinigameStart()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.FadeAmbient(AudioManager.Instance.DefaultAmbientVolume, ambientFadeTime);
    }

    public void OnMinigameEnd()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.FadeAmbient(0f, ambientFadeTime);
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.FadeAmbient(0f, ambientFadeTime);
    }
}
