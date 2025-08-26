using UnityEngine;

[CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "ScriptableObjects/TowerScriptableObject")]
public class TowerScriptableObject : ScriptableObject
{
    public string towerName;
    public int towerCost;
    public float attackRadius;
    public float attackRate;
    public GameObject towerPrefab;
    public GameObject projectilePrefab;
}
