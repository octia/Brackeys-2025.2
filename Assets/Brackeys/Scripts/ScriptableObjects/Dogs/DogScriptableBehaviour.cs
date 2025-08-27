using UnityEngine;

[CreateAssetMenu(fileName = "Base Dog", menuName = "ScriptableObjects/Dog")]
public class DogScriptableBehaviour : ScriptableObject
{
    public string dogName;
    public float dogMaxHealth;
    public float dogSpeed;
    public float dogDamage;
    public GameObject dogPrefab;
}
