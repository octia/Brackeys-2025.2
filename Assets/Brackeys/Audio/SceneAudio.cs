using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class SceneAudio : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private EventReference musicEvent;
    [SerializeField] private float fadeTime = 1.25f;
    [SerializeField] private float delayBeforeStart = 1f;
    [SerializeField] private float musicVolume = 1f;

    [Header("Ambient Settings")]
    [SerializeField] private float ambientFadeTime = 1f;
    [SerializeField] private float mainMenuAmbientVolume = 1f;
    [SerializeField] private float otherScenesAmbientVolume = 0f;

    private bool firstLoad = true;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.SetMusicVolume(musicVolume);

        string sceneName = SceneManager.GetActiveScene().name;

        // --- Scene Music ---
        if (sceneName == "MainMenu" && firstLoad)
        {
            // Start Main Menu music instantly on first load
            AudioManager.Instance.PlayMusicInstant(musicEvent);
            firstLoad = false;
        }
        else
        {
            // Fade in normally for all other cases
            AudioManager.Instance.PlayMusic(musicEvent, fadeTime, delayBeforeStart);
        }

        // --- Ambient ---
        AudioManager.Instance.StartAmbient();

        float initialAmbientVolume = (sceneName == "MainMenu") ? mainMenuAmbientVolume : otherScenesAmbientVolume;
        AudioManager.Instance.FadeAmbient(initialAmbientVolume, ambientFadeTime);
    }

    // --- Minigame Controls ---
    public void OnMinigameStart(float targetVolume = 1f)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.FadeAmbient(targetVolume, ambientFadeTime);
    }

    public void OnMinigameEnd(float targetVolume = 0f)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.FadeAmbient(targetVolume, ambientFadeTime);
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.FadeAmbient(0f, ambientFadeTime);
    }
}
