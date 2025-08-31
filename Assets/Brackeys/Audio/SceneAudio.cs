using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class SceneAudio : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private EventReference musicEvent;
    [SerializeField] private float musicFadeTime = 1f;

    [Header("Ambient Settings")]
    [SerializeField] private EventReference ambientEvent;
    [SerializeField] private float ambientFadeTime = 1f;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        string scene = SceneManager.GetActiveScene().name;

        if (scene == "MainMenu")
        {
            AudioManager.Instance.PlayMusicInstant(musicEvent);
            AudioManager.Instance.StartAmbient(ambientEvent);
            AudioManager.Instance.FadeAmbient(AudioManager.Instance.DefaultAmbientVolume, 0f);
        }
        else
        {
            // Fade old scene audio out & start new scene music
            AudioManager.Instance.CrossfadeSceneAudio(musicEvent, musicFadeTime);

            // AudioManager.Instance.PlayMusicInstant(musicEvent);
            AudioManager.Instance.StartAmbient(ambientEvent);
            AudioManager.Instance.FadeAmbient(AudioManager.Instance.DefaultAmbientVolume, ambientFadeTime);

            // Minigame parameter starts at 0 automatically
        }
    }

    public void OnMinigameStart()
    {
        AudioManager.Instance.FadeAmbient(AudioManager.Instance.DefaultAmbientVolume, ambientFadeTime);
        AudioManager.Instance.FadeMinigame(1f, 1f);
    }

    public void OnMinigameEnd()
    {
        AudioManager.Instance.FadeAmbient(0f, ambientFadeTime);
        AudioManager.Instance.FadeMinigame(0f, 1f);
    }

    private void OnDisable()
    {
        AudioManager.Instance.FadeAmbient(0f, ambientFadeTime);
        // AudioManager.Instance.FadeMinigame(0f, minigameFadeTime);
        AudioManager.Instance.FadeMusic(0f, musicFadeTime);
    }
}
