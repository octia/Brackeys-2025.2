using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TowerLevel
{
    public float towerCost;
    public float attackRadius;
    public float attackRate;
    public float attackHeight;
    public float damageAmount;
    public float damageDuration;
    public float enemySpeed;
    public float towerHealth;
    public float repairCost;

}
[CreateAssetMenu(fileName = "Base Tower", menuName = "ScriptableObjects/Tower")]
public class TowerScriptableObject : ScriptableObject
{
    public string towerName;
    public bool isAttackAOE;
    public bool isDamageOverTime;
    public bool shouldLookAtTarget;
    public float animationDelay;
    public GameObject towerPrefab;
    public GameObject projectilePrefab;

    [Header("Levels")]
    public List<TowerLevel> levels = new List<TowerLevel>();


    public Texture icon;
}
