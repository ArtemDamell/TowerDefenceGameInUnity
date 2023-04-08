using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType
{
    public enum EnemyArmorType
    {
        Physical,
        Magical,
        True
    };

    public enum EnemyElement
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

public class MoveEnemy : MonoBehaviour
{
    [HideInInspector] public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;
    public float armor = 0.1f;
    public float attack = 1.0f;
    private GameManager gameManager;
    [SerializeField] private int killingPrice;
    public EnemyType.EnemyArmorType enemyTypeArmor;
    public EnemyType.EnemyElement enemyElement;


    public int KillingPrice
    {
        get { return killingPrice; }
    }

    void Start()
    {
        lastWaypointSwitchTime = Time.time;
        gameManager = GameObject.Find("GameManagerBehaviour").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gameManager.gameOver)
        {
            // Из массива точек маршрута мы получаем начальную и конечную позиции текущего сегмента маршрута.
            Vector3 startPosition = waypoints[currentWaypoint].transform.position;
            Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;

            // Вычисляем время, необходимое для прохождения всего расстояния с помощью формулы время = расстояние / скорость, а затем определяем текущее время на маршруте. С помощью Vector2.Lerp, мы интерполируем текущую позицию врага между начальной и конечной точной сегмента.
            float pathLength = Vector2.Distance(startPosition, endPosition);
            float totalTimeForPath = pathLength / speed;
            float currentTimeOnPath = Time.time - lastWaypointSwitchTime;

            gameObject.transform.position =
                Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

            if (gameObject.transform.position.Equals(endPosition))
            {
                if (currentWaypoint < waypoints.Length - 2)
                {
                    // Враг пока не дошёл до последней точки маршрута, поэтому увеличиваем значение currentWaypoint и обновляем lastWaypointSwitchTime. Позже мы добавим код для поворота врага, чтобы он смотрел в направлении своего движения.
                    currentWaypoint++;
                    lastWaypointSwitchTime = Time.time;

                    //RotateIntoMoveDirection();
                }
                else
                {
                    // Враг достиг последней точки маршрута, тогда мы уничтожаем его и запускаем звуковой эффект. Позже мы добавим код уменьшающий health игрока.
                    Destroy(gameObject);

                    //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                    //AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

                    GameManager gameManager =
                        GameObject.Find("GameManagerBehaviour").GetComponent<GameManager>();
                    gameManager.Health -= 1;
                }
            }
        }
    }

    /// <summary>
    /// Calculates the distance from the current waypoint to the goal.
    /// </summary>
    /// <returns>The distance from the current waypoint to the goal.</returns>
    public float DistanceToGoal()
    {
        float distance = 0;
        distance += Vector2.Distance(
            gameObject.transform.position,
            waypoints[currentWaypoint + 1].transform.position);
        for (int i = currentWaypoint + 1; i < waypoints.Length - 1; i++)
        {
            Vector3 startPosition = waypoints[i].transform.position;
            Vector3 endPosition = waypoints[i + 1].transform.position;
            distance += Vector2.Distance(startPosition, endPosition);
        }
        return distance;
    }
}
