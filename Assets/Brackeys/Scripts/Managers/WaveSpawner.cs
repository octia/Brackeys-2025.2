using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public List<WaveScriptableObject> waves;
    public Transform spawnPoint;
    public GameObject dogPrefab;

    public float timeBetweenWaves = 5f;
    public float timeBeforeFirstWave = 10f;
    public float countdown = 2f;

    public int waveIndex = 0;


    [SerializeField] RhythmController rhythmController;

    [Inject, HideInInspector]
    public TimerManager timerManager;

    [Inject, HideInInspector]
    public BiscuitManager biscuitManager;

    private float currentTimeBetweenWaves;
    private float currentDogWaitTime;

    private bool inGame;
    private bool inWave;

    private List<DogScriptableBehaviour> spawnPool = new List<DogScriptableBehaviour>();
    public TMP_Text wavesText;
    public Image waveBar;

    void Start()
    {
        RunAllWaves();
    }

    private void Update()
    {
        if (timerManager.IsPaused || !inGame)
        {
            return;
        }

        // Current wave

        if (inWave)
        {
            currentDogWaitTime -= Time.deltaTime;

            if (currentDogWaitTime < 0)
            {
                if (spawnPool.Count <= 0)
                {
                    EndWave();
                    return;
                }

                float waitTime = Random.Range(waves[waveIndex].spawnRateRange.x, waves[waveIndex].spawnRateRange.y);
                currentDogWaitTime = waitTime;

                SpawnDog(spawnPool);

                if (spawnPool.Count <= 0)
                {
                    EndWave();
                    return;
                }
            }

            return;
        }

        // New waves

        currentTimeBetweenWaves -= Time.deltaTime;

        if (currentTimeBetweenWaves < 0)
        {
            RunWave();

            currentTimeBetweenWaves = timeBetweenWaves;
            wavesText.text = (waveIndex + 1) + " / " + (waves.Count - 1);
            float fillAmount = (float)(waveIndex + 1) / (waves.Count - 1);
            waveBar.fillAmount = fillAmount;
        }
    }

    private void RunAllWaves()
    {
        inGame = true;
        currentTimeBetweenWaves = timeBeforeFirstWave;
        wavesText.text = waveIndex + " / " + (waves.Count - 1);
        float fillAmount = (float)waveIndex / (waves.Count - 1);
        waveBar.fillAmount = fillAmount;
    }

    private void RunWave()
    {
        inWave = true;
        WaveScriptableObject wave = waves[waveIndex];

        if (wave)
        {
            Debug.Log($"Starting wave: {wave.waveName}");

            foreach (DogSpawnEntry entry in wave.dogs)
            {
                for (int i = 0; i < entry.count; i++)
                {
                    spawnPool.Add(entry.dogType);
                }
            }
        }
    }

    private void EndWave()
    {
        inWave = false;
        waveIndex++;

        if (waveIndex >= waves.Count)
        {
            inGame = false;
            SceneManager.LoadScene("EndScene");
        }
    }

    private void SpawnDog(List<DogScriptableBehaviour> spawnPool)
    {
        int randomIndex = Random.Range(0, spawnPool.Count);
        DogScriptableBehaviour dog = spawnPool[randomIndex];
        spawnPool.RemoveAt(randomIndex);

        //GameObject dogGO = new GameObject(dog.dogName);
        //DogManager dogManager = dogGO.AddComponent<DogManager>();
        GameObject dogGO = Instantiate(dogPrefab);
        var dogManager = dogGO.GetComponent<DogManager>();
        dogManager.dogData = dog;
        dogManager.waypointLineIndex = Random.Range(0, Waypoints.pointsList.Count);
        dogManager.timerManager = timerManager;
        dogManager.biscuitManager = biscuitManager;
        dogManager.rhythmController = rhythmController;

        //dogGO.transform.SetParent(Waypoints.pointsList[dogManager.waypointLineIndex][0], false);
        dogGO.transform.localPosition = Vector3.zero;
        dogGO.transform.localRotation = Quaternion.identity;
        dogGO.transform.position = Waypoints.pointsList[dogManager.waypointLineIndex][0].position;
    }
}
