using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeChoise
{
    public enum BulletType
    {
        Physical,
        Magical,
        True
    };

    public enum AttackElement
    {
        None,
        Fire,
        Water,
        Light,
        Earth,
        Nature,
        Wind
    };
}

public class BulletBehavior : MonoBehaviour
{
    public BulletTypeChoise.BulletType bulletType;
    public BulletTypeChoise.AttackElement bulletElement;
    [SerializeField] private float speed = 10.0f;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] private float damage;

    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;

    private float distance;
    private float startTime;

    private GameManager gameManager;

    void Start()
    {
        startTime = Time.time;
        distance = Vector2.Distance(startPosition, targetPosition);
        GameObject gm = GameObject.Find("GameManagerBehaviour");
        gameManager = gm.GetComponent<GameManager>();
    }

    void Update()
    {
        float timeInterval = Time.time - startTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval * speed / distance);

        if (gameObject.transform.position.Equals(targetPosition))
        {
            if (target != null)
            {
                Transform healthBarTransform = target.transform.Find("HealthBar");
                HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();

                // TODO: Damage formula
                // Get enemy damage formula method
                float tempDamage = ChoiseDamageFormula(target, damage);

                float finalDamage = FinalDamage(target, tempDamage);


                print($"Damage is: {finalDamage}");
                healthBar.CurrentHealth -= Mathf.Max(finalDamage, 0);

                if (healthBar.CurrentHealth <= 0)
                {
                    gameManager.Gold += target.GetComponent<MoveEnemy>().KillingPrice;
                    Destroy(target);

                    //TODO: Play sounds enemy death
                }
                Destroy(gameObject);
            }
            else if (target == null)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// Calculates the damage to be applied to an enemy based on its armor type and the type of bullet used.
    /// </summary>
    /// <param name="_target">The target enemy.</param>
    /// <param name="_damage">The base damage.</param>
    /// <returns>The calculated damage.</returns>
    float ChoiseDamageFormula(GameObject _target, float _damage)
    {
        MoveEnemy enemyScript = _target.transform.gameObject.GetComponent<MoveEnemy>();

        if (enemyScript.enemyTypeArmor == EnemyType.EnemyArmorType.Physical &&
            bulletType == BulletTypeChoise.BulletType.Physical)
        {
            float tempDamage = _damage - (_damage * enemyScript.armor);
            return tempDamage;
        }
        else if (enemyScript.enemyTypeArmor == EnemyType.EnemyArmorType.Magical &&
                 bulletType == BulletTypeChoise.BulletType.Physical)
        {
            float tempDamage = _damage + (_damage * enemyScript.armor);
            return tempDamage;
        }
        else if (enemyScript.enemyTypeArmor == EnemyType.EnemyArmorType.Magical &&
                 bulletType == BulletTypeChoise.BulletType.Magical)
        {
            float tempDamage = _damage - (_damage * enemyScript.armor);
            return tempDamage;
        }

        return 20f;
    }

    /// <summary>
    /// Calculates the final damage to an enemy based on the enemy's element and the bullet's element.
    /// </summary>
    /// <param name="_target">The enemy object.</param>
    /// <param name="_tempDamage">The initial damage.</param>
    /// <returns>The final damage.</returns>
    float FinalDamage(GameObject _target, float _tempDamage)
    {
        MoveEnemy enemyScript = _target.transform.gameObject.GetComponent<MoveEnemy>();

        if (enemyScript.enemyElement == EnemyType.EnemyElement.Fire && bulletElement == BulletTypeChoise.AttackElement.Water)
        {
            _tempDamage *= 1.5f;
            return _tempDamage;
        }
        else if (enemyScript.enemyElement == EnemyType.EnemyElement.Fire &&
                 bulletElement == BulletTypeChoise.AttackElement.Nature)
        {
            _tempDamage *= 0.5f;
            return _tempDamage;
        }
        else
        {
            return _tempDamage;
        }
    }
}
