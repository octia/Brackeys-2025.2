using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaveSpawner : MonoBehaviour
{
    public List<WaveScriptableObject> waves;
    public Transform spawnPoint;
    public GameObject dogPrefab;

    public float timeBetweenWaves = 5f;
    public float countdown = 2f;

    public int waveIndex = 0;

    [Inject]
    public TimerManager timerManager;

    [Inject]
    public BiscuitManager biscuitManager;

    private float currentTimeBetweenWaves;
    private float currentDogWaitTime;

    private bool inGame;
    private bool inWave;

    private List<DogScriptableBehaviour> spawnPool = new List<DogScriptableBehaviour>();

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
                SpawnDog(spawnPool);

                float waitTime = Random.Range(waves[waveIndex].spawnRateRange.x, waves[waveIndex].spawnRateRange.y);
                currentDogWaitTime = waitTime;

                if (spawnPool.Count <= 0)
                {
                    inWave = false;
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
            waveIndex++;

            if (waveIndex >= waves.Count)
            {
                inGame = false;
            }
        }
    }

    private void RunAllWaves()
    {
        inGame = true;
        currentTimeBetweenWaves = timeBetweenWaves;
    }

    private void RunWave()
    {
        inWave = true;

        WaveScriptableObject wave = waves[waveIndex];
        Debug.Log($"Starting wave: {wave.waveName}");

        foreach (DogSpawnEntry entry in wave.dogs)
        {
            for (int i = 0; i < entry.count; i++)
            {
                spawnPool.Add(entry.dogType);
            }
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

        //dogGO.transform.SetParent(Waypoints.pointsList[dogManager.waypointLineIndex][0], false);
        dogGO.transform.localPosition = Vector3.zero;
        dogGO.transform.localRotation = Quaternion.identity;
        dogGO.transform.position = Waypoints.pointsList[dogManager.waypointLineIndex][0].position;
    }
}
