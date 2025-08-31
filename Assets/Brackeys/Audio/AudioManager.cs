using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Default Volumes")]
    [SerializeField] private float defaultMusicVolume = 1f;
    [SerializeField] private float defaultAmbientVolume = 1f;

    public float DefaultMusicVolume => defaultMusicVolume;
    public float DefaultAmbientVolume => defaultAmbientVolume;

    [Header("VCAs")]
    [SerializeField] private VCA masterVCA;
    [SerializeField] private VCA musicVCA;
    [SerializeField] private VCA sfxVCA;

    private EventInstance musicInstance;
    private EventInstance ambientInstance;

    private Coroutine musicFadeCoroutine;
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

    // --- VCA Controls ---
    public void SetMasterVolume(float volume) => masterVCA.setVolume(volume);
    public void SetMusicVolume(float volume) => musicVCA.setVolume(volume);
    public void SetSFXVolume(float volume) => sfxVCA.setVolume(volume);

    // --- Music ---
    public void PlayMusicInstant(EventReference musicEvent)
    {
        StopMusicInstant();
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
        musicInstance.setVolume(defaultMusicVolume);
        musicInstance.setParameterByName("Minigame", 0f);
    }

    public void PlayMusic(EventReference musicEvent, float fadeTime, float delay = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusic(musicEvent, fadeTime, delay));
    }

    public void FadeMusic(float targetVolume, float fadeTime)
    {
        if (!musicInstance.isValid()) return;
        if (musicFadeCoroutine != null) StopCoroutine(musicFadeCoroutine);
        musicFadeCoroutine = StartCoroutine(FadeParameter(musicInstance, targetVolume, fadeTime));
    }

    private IEnumerator FadeInMusic(EventReference musicEvent, float fadeTime, float delay)
    {
        if (musicInstance.isValid())
            yield return FadeParameter(musicInstance, 0f, fadeTime);

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
        yield return FadeParameter(musicInstance, defaultMusicVolume, fadeTime);
    }

    private void StopMusicInstant()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
    }

    // --- Ambient ---
    public void StartAmbient(EventReference ambientEvent, float startVolume = 0f)
    {
        if (ambientInstance.isValid()) return;

        ambientInstance = RuntimeManager.CreateInstance(ambientEvent);
        ambientInstance.start();
        ambientInstance.setVolume(startVolume);
    }

    public void FadeAmbient(float targetVolume, float fadeTime)
    {
        if (!ambientInstance.isValid()) return;
        if (ambientFadeCoroutine != null) StopCoroutine(ambientFadeCoroutine);
        ambientFadeCoroutine = StartCoroutine(FadeParameter(ambientInstance, targetVolume, fadeTime));
    }

    // --- Minigame using FMOD parameter ---
    public void StartMinigame(EventReference minigameEvent)
    {
        if (musicInstance.isValid()) return;

        musicInstance = RuntimeManager.CreateInstance(minigameEvent);
        musicInstance.start();
        musicInstance.setParameterByName("Minigame", 0f); // start silent
    }

    public void FadeMinigame(float targetValue, float fadeTime)
    {
        if (!musicInstance.isValid()) return;
        if (musicFadeCoroutine != null) StopCoroutine(musicFadeCoroutine);
        musicFadeCoroutine = StartCoroutine(FadeParameterByName(musicInstance, "Minigame", targetValue, fadeTime));
    }

    // --- Generic FMOD fades ---
    private IEnumerator FadeParameter(EventInstance instance, float targetVolume, float duration)
    {
        if (!instance.isValid()) yield break;
        instance.getVolume(out float startVolume);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newVol = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            instance.setVolume(newVol);
            yield return null;
        }
        instance.setVolume(targetVolume);
    }

    private IEnumerator FadeParameterByName(EventInstance instance, string paramName, float targetValue, float duration)
    {
        if (!instance.isValid()) yield break;
        instance.getParameterByName(paramName, out float startValue);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            instance.setParameterByName(paramName, newValue);
            yield return null;
        }
        instance.setParameterByName(paramName, targetValue);
    }

    // --- Crossfade helper for scene transitions ---
    public void CrossfadeSceneAudio(EventReference newMusicEvent, float fadeTime)
    {
        if (musicInstance.isValid())
            StartCoroutine(FadeOutMusic(fadeTime));

        PlayMusic(newMusicEvent, fadeTime);
    }

    private IEnumerator FadeOutMusic(float fadeTime)
    {
        yield return FadeParameter(musicInstance, 0f, fadeTime);
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}
