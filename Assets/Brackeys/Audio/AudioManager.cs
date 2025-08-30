using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Default Volumes")]
    public float DefaultMusicVolume = 1f;
    public float DefaultAmbientVolume = 1f;
    public float DefaultMinigameVolume = 1f;

    private EventInstance musicInstance;
    private EventInstance ambientInstance;
    private EventInstance minigameInstance;

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

    // --- MUSIC ---
    public void PlayMusic(EventReference musicEvent, float fadeTime = 1f, float delay = 0f)
    {
        StartCoroutine(StartMusicCoroutine(musicEvent, fadeTime, delay));
    }

    private IEnumerator StartMusicCoroutine(EventReference musicEvent, float fadeTime, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (musicInstance.isValid())
            StartCoroutine(FadeOutAndRelease(musicInstance, fadeTime));

        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.setVolume(0f);
        musicInstance.start();

        StartCoroutine(FadeVolume(musicInstance, DefaultMusicVolume, fadeTime));
    }

    public void PlayMusicInstant(EventReference musicEvent)
    {
        if (musicInstance.isValid())
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.setVolume(DefaultMusicVolume);
        musicInstance.start();
    }

    // --- AMBIENT ---
    public void StartAmbient(EventReference ambientEvent)
    {
        if (ambientInstance.isValid())
            ambientInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        ambientInstance = RuntimeManager.CreateInstance(ambientEvent);
        ambientInstance.setVolume(0f); // start muted
        ambientInstance.start();
    }

    // --- MINIGAME ---
    public void StartMinigame(EventReference minigameEvent)
    {
        if (minigameInstance.isValid())
            minigameInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        minigameInstance = RuntimeManager.CreateInstance(minigameEvent);
        minigameInstance.setVolume(0f); // start muted
        minigameInstance.start();
    }

    // --- FADE HELPERS ---
    public void FadeMusic(float target, float time) => StartCoroutine(FadeVolume(musicInstance, target, time));
    public void FadeAmbient(float target, float time) => StartCoroutine(FadeVolume(ambientInstance, target, time));
    public void FadeMinigame(float target, float time) => StartCoroutine(FadeVolume(minigameInstance, target, time));

    private IEnumerator FadeOutAndRelease(EventInstance instance, float time)
    {
        yield return FadeVolume(instance, 0f, time);
        instance.release();
    }

    private IEnumerator FadeVolume(EventInstance instance, float targetVolume, float time)
    {
        if (!instance.isValid()) yield break;

        instance.getVolume(out float startVol);
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            float newVol = Mathf.Lerp(startVol, targetVolume, t);
            instance.setVolume(newVol);
            yield return null;
        }

        instance.setVolume(targetVolume);
    }

    private void OnDestroy()
    {
        if (musicInstance.isValid()) musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if (ambientInstance.isValid()) ambientInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if (minigameInstance.isValid()) minigameInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
