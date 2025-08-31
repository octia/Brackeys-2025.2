using UnityEngine;
using System.Collections.Generic;
using FMODUnity;

[System.Serializable]
public class TowerLevel
{
    public float towerCost;
    public float attackRadius;
    public float attackRate;
    public float attackHeight;
    public float attackCost;
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
    [Multiline(5)]
    public string towerDescription = "...";
    public bool isAttackAOE;
    public bool isDamageOverTime;
    public bool shouldLookAtTarget;
    public float animationDelay;
    public GameObject towerPrefab;
    public GameObject projectilePrefab;
    public EventReference towerAttack;
    public EventReference towerDown;

    [Header("Levels")]
    public List<TowerLevel> levels = new List<TowerLevel>();


    public Texture icon;
}
