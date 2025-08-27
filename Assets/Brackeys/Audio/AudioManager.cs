using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Settings")]
    [SerializeField] private float defaultMusicVolume = 1f;
    private EventInstance currentMusicInstance;
    private Coroutine musicFadeCoroutine;

    [Header("Ambient Settings")]
    [SerializeField] private EventReference ambientEvent;
    [SerializeField] private float defaultAmbientVolume = 1f;
    private EventInstance ambientInstance;
    private Coroutine ambientFadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // -------------------- MUSIC --------------------
    public void PlayMusic(EventReference musicEvent, float fadeDuration = 1f, float delayBeforeStart = 0f)
    {
        if (musicEvent.IsNull) return;
        StartCoroutine(PlayMusicCoroutine(musicEvent, fadeDuration, delayBeforeStart));
    }

    private IEnumerator PlayMusicCoroutine(EventReference musicEvent, float fadeDuration, float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        // Create new music instance
        EventInstance newMusic = RuntimeManager.CreateInstance(musicEvent);
        newMusic.setVolume(0f);
        newMusic.start();

        // Capture old music for crossfade
        EventInstance oldMusic = currentMusicInstance;
        currentMusicInstance = newMusic;

        float time = 0f;
        float startVolumeOld = 0f;
        float startVolumeNew = 0f;

        if (oldMusic.isValid())
            oldMusic.getVolume(out startVolumeOld);

        newMusic.getVolume(out startVolumeNew);

        // Crossfade loop
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            // Fade in new music
            newMusic.setVolume(Mathf.Lerp(startVolumeNew, defaultMusicVolume, t));

            // Fade out old music
            if (oldMusic.isValid())
                oldMusic.setVolume(Mathf.Lerp(startVolumeOld, 0f, t));

            yield return null;
        }

        newMusic.setVolume(defaultMusicVolume);

        // Stop old music after fade
        if (oldMusic.isValid())
        {
            oldMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            oldMusic.release();
        }
    }

    public void PlayMusicInstant(EventReference musicEvent)
    {
        if (musicEvent.IsNull) return;

        if (currentMusicInstance.isValid())
        {
            currentMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentMusicInstance.release();
        }

        currentMusicInstance = RuntimeManager.CreateInstance(musicEvent);
        currentMusicInstance.setVolume(defaultMusicVolume);
        currentMusicInstance.start();
    }

    public void SetMusicVolume(float volume)
    {
        defaultMusicVolume = Mathf.Clamp01(volume);
        if (currentMusicInstance.isValid())
            currentMusicInstance.setVolume(defaultMusicVolume);
    }

    public void FadeMusic(float targetVolume, float duration, bool stopAfterFade = false)
    {
        if (!currentMusicInstance.isValid()) return;

        if (musicFadeCoroutine != null)
            StopCoroutine(musicFadeCoroutine);

        musicFadeCoroutine = StartCoroutine(FadeMusicCoroutine(targetVolume, duration, stopAfterFade));
    }

    private IEnumerator FadeMusicCoroutine(float targetVolume, float duration, bool stopAfterFade)
    {
        currentMusicInstance.getVolume(out float startVolume);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            currentMusicInstance.setVolume(newVolume);
            yield return null;
        }

        currentMusicInstance.setVolume(targetVolume);

        if (stopAfterFade && currentMusicInstance.isValid())
        {
            currentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusicInstance.release();
        }
    }

    // -------------------- AMBIENT --------------------
    public void StartAmbient()
    {
        if (ambientInstance.isValid()) return;
        if (ambientEvent.IsNull) return;

        ambientInstance = RuntimeManager.CreateInstance(ambientEvent);
        ambientInstance.setVolume(0f); // always start silent
        ambientInstance.start();
    }

    public void FadeAmbient(float targetVolume, float duration)
    {
        if (!ambientInstance.isValid()) return;

        if (ambientFadeCoroutine != null)
            StopCoroutine(ambientFadeCoroutine);

        ambientFadeCoroutine = StartCoroutine(FadeAmbientCoroutine(targetVolume, duration));
    }

    private IEnumerator FadeAmbientCoroutine(float targetVolume, float duration)
    {
        ambientInstance.getVolume(out float startVolume);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            ambientInstance.setVolume(newVolume);
            yield return null;
        }

        ambientInstance.setVolume(targetVolume);
    }

    // -------------------- SCENE LOADING --------------------
    public void LoadSceneWithAmbientFade(string sceneName, float fadeDuration = 2f)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, fadeDuration));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, float fadeDuration)
    {
        // Fade out music & ambient simultaneously
        if (currentMusicInstance.isValid())
            StartCoroutine(FadeMusicCoroutine(0f, fadeDuration, stopAfterFade: true));

        if (ambientInstance.isValid())
            StartCoroutine(FadeAmbientCoroutine(0f, fadeDuration));

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene(sceneName);

        // Fade ambient back in if needed
        float targetVolume = (sceneName == "MainMenu") ? defaultAmbientVolume : 0f;
        if (ambientInstance.isValid())
            FadeAmbient(targetVolume, fadeDuration);
    }
}
