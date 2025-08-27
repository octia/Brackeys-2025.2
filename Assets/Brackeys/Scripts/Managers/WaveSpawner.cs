using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{

    public Transform spawnPoint;
    public Transform dogPrefab;

    public float timeBetweenWaves = 5f;
    public float countdown = 2f;

    private int waveIndex = 0;



    // Update is called once per frame
    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < waveIndex + 1; i++)
        {
            SpawnDog();
            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
        waveIndex++;
    }

    void SpawnDog()
    {
        Instantiate(dogPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
