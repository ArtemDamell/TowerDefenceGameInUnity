using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemies : MonoBehaviour
{
    public List<GameObject> enemiesInRange;

    private float lastShootTime;
    private TowerPrefabScript towerPrefabScript;

    void Start()
    {
        enemiesInRange = new List<GameObject>();
        lastShootTime = Time.time;
        towerPrefabScript = gameObject.GetComponent<TowerPrefabScript>();
    }

    void Update()
    {
        if (towerPrefabScript.TowerValues.isAOE == false)
        {
            GameObject target = null;

            float minimalEnemyDistance = float.MaxValue;

            foreach (var enemy in enemiesInRange)
            {
                float distanceToGoal = enemy.GetComponent<MoveEnemy>().DistanceToGoal();

                if (distanceToGoal < minimalEnemyDistance)
                {
                    target = enemy;
                    minimalEnemyDistance = distanceToGoal;
                }
            }

            if (target != null)
            {
                if (Time.time - lastShootTime > towerPrefabScript.TowerValues.towerAttackSpeed)
                {
                    Shoot(target.GetComponent<Collider2D>());
                    lastShootTime = Time.time;
                }
            }
        }

        else if (towerPrefabScript.TowerValues.isAOE == true)
        {
            if (Time.time - lastShootTime > towerPrefabScript.TowerValues.towerAttackSpeed)
            {
                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    Shoot(enemiesInRange[i].GetComponent<Collider2D>());
                }
                lastShootTime = Time.time;
            }
        }
    }

    /// <summary>
    /// Removes the specified enemy from the list of enemies in range.
    /// </summary>
    /// <param name="_enemy">The enemy to remove.</param>
    private void OnEnemyDestroy(GameObject _enemy)
    {
        enemiesInRange.Remove(_enemy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate += OnEnemyDestroy;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
            EnemyDestructionDelegate del =
                other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate -= OnEnemyDestroy;
        }
    }

    /// <summary>
    /// This method is used to shoot a bullet from a tower towards a target. It instantiates a bullet prefab, sets the start and target positions, and plays an animation and sound.
    /// </summary>
    private void Shoot(Collider2D target)
    {
        GameObject bulletPrefab = towerPrefabScript.TowerValues.bullet;

        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = bulletPrefab.transform.position.z;
        targetPosition.z = bulletPrefab.transform.position.z;

        GameObject newBullet = Instantiate(bulletPrefab);

        // Bullet rotation
        Vector2 direction = target.transform.position - startPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        newBullet.transform.position = startPosition;
        newBullet.transform.rotation = rotation;
        BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;
    }
}
