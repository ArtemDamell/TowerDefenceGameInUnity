using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tower creation class
/// </summary>
[Serializable]
public class TowerValues
{
    /// <summary>
    /// Tower Image
    /// </summary>
    public Sprite towerIcon;

    /// <summary>
    /// Current tower force
    /// </summary>
    [Range(1, 10)]
    public float towerForce;

    /// <summary>
    /// Attack speed
    /// </summary>
    [Range(0.5f, 10f)]
    public float towerAttackSpeed;

    /// <summary>
    /// Attack range
    /// </summary>
    [Range(3, 5)]
    public float towerAttackRange;


    public enum TowerAttackType
    {
        Physical,
        Magical,
        True
    };

    

    /// <summary>
    /// Attack type enumerator
    /// </summary>
    public TowerAttackType AttackType;

    /// <summary>
    /// Tower price
    /// </summary>
    public int towerCost;

    private int upgradeButtonIndex;

    public int UpgradeButtonIndex
    {
        get { return upgradeButtonIndex; }
        set { upgradeButtonIndex = value; }
    }

    public GameObject bullet;

    public bool isAOE = false;
}

public class TowerPrefabScript : MonoBehaviour
{
    /// <summary>
    /// Inicialize Tower Values class
    /// </summary>
    public TowerValues TowerValues;
}
