using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2;
    public int maxEnemies = 20;
}

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] waypoints;

    public Wave[] waves;
    public int timeBetweenWaves = 5;

    private GameManager gameManager;

    private float lastSpawnTime;
    private int enemiesSpawned = 0;

    void Start()
    {
        lastSpawnTime = Time.time;
        gameManager = GameObject.Find("GameManagerBehaviour").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gameManager.gameOver)
        {
            // Получаем индекс текущей волны и проверяем, последняя ли она.
            int currentWave = gameManager.Wave;
            if (currentWave < waves.Length)
            {
                // Если да, то вычисляем время, прошедшее после предыдущего спауна врагов и проверяем, настало ли время создавать врага. Здесь мы учитываем два случая. Если это первый враг в волне, то мы проверяем, больше ли timeInterval, чем timeBetweenWaves. В противном случае мы проверяем, больше ли timeInterval, чем spawnInterval волны. В любом случае мы проверяем, что не создали всех врагов в этой волне.
                float timeInterval = Time.time - lastSpawnTime;
                float spawnInterval = waves[currentWave].spawnInterval;

                if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || timeInterval > spawnInterval) &&
                    enemiesSpawned < waves[currentWave].maxEnemies)
                {
                    // При необходимости спауним врага, создавая экземпляр enemyPrefab. Также увеличиваем значение enemiesSpawned.
                    lastSpawnTime = Time.time;
                    GameObject newEnemy = (GameObject)
                        Instantiate(waves[currentWave].enemyPrefab);
                    newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
                    enemiesSpawned++;
                }

                // Проверяем количество врагов на экране. Если их нет, и это был последний враг в волне, то создаём следующую волну. Также в конце волны мы даём игроку 10 процентов всего оставшегося золота.
                if (enemiesSpawned == waves[currentWave].maxEnemies &&
                    GameObject.FindGameObjectWithTag("Enemy") == null)
                {
                    gameManager.Wave++;
                    gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                    enemiesSpawned = 0;
                    lastSpawnTime = Time.time;
                }

                // После победы над последней волной здесь воспроизводится анимация победы в игре.
                //TODO: Win animation and logic
            }
        }
    }
}
