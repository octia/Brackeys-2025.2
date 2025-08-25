using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class PathEntry
    {
        public TDPath path;
        [Min(0f)] public float weight = 1f;
    }

    public GameObject enemyPrefab;
    public List<PathEntry> paths = new();
    public float spawnEverySeconds = 1.5f;
    public int burstCount = 5;

    float timer;

    private void Update()
    {
        if (!enemyPrefab || paths.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= spawnEverySeconds)
        {
            timer = 0f;
            for (int i = 0; i < burstCount; i++)
                SpawnOne();
        }
    }

    void SpawnOne()
    {
        var path = PickPathWeighted();
        if (!path) return;

        var go = Instantiate(enemyPrefab, path.Sample(0f), Quaternion.identity);
        var pf = go.GetComponent<PathFollower>();
        if (!pf) pf = go.AddComponent<PathFollower>();
        pf.path = path;
    }

    TDPath PickPathWeighted()
    {
        float total = 0f;
        foreach (var e in paths) total += Mathf.Max(0f, e.weight);
        if (total <= 0f) return paths[0].path;

        float r = Random.value * total;
        float acc = 0f;
        foreach (var e in paths)
        {
            acc += Mathf.Max(0f, e.weight);
            if (r <= acc) return e.path;
        }
        return paths[^1].path;
    }
}
