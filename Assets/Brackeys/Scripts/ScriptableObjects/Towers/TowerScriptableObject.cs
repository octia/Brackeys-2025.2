using UnityEngine;

[CreateAssetMenu(fileName = "Base Tower", menuName = "ScriptableObjects/Tower")]
public class TowerScriptableObject : ScriptableObject
{
    public string towerName;
    public int towerCost;
    public float attackRadius;
    public float attackRate;
    public bool isAttackAOE;
    public bool isDamageOverTime;
    public float damageAmount;
    public float damageDuration;
    public GameObject towerPrefab;
    public GameObject projectilePrefab;
}
