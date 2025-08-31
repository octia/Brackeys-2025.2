using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "Base Dog", menuName = "ScriptableObjects/Dog")]
public class DogScriptableBehaviour : ScriptableObject
{
    public string dogName;
    public float dogMaxHealth;
    public float dogSpeed;
    public float dogDamage;
    public GameObject dogPrefab;
    public EventReference distracted;
    public EventReference runAway;
    public EventReference chomp;
}
