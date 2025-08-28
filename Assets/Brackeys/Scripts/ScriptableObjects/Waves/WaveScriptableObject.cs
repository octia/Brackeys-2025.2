using UnityEngine;

[System.Serializable]
public class DogSpawnEntry
{
    public DogScriptableBehaviour dogType;
    public int count;
}

[CreateAssetMenu(fileName = "Base Wave", menuName = "ScriptableObjects/Wave")]
public class WaveScriptableObject : ScriptableObject
{
    public string waveName;

    [Tooltip("X = Min spawn interval, Y = Max spawn interval")]
    public Vector2 spawnRateRange = new Vector2(4f, 7f);
    public DogSpawnEntry[] dogs;

}
