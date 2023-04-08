using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text goldLabel;
    [SerializeField] private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            goldLabel.GetComponent<Text>().text = $"{gold}";
        }
    }

    public Text waveLabel;

    public bool gameOver = false;

    private int wave;
    public int Wave
    {
        get { return wave; }
        set
        {
            wave = value;
            if (!gameOver)
            {
                //TODO: Next wave lable animation
            }

            int wavesCount = GameObject.Find("WaypointsALL").GetComponent<SpawnEnemy>().waves.Length;

            if (wave == wavesCount)
                waveLabel.text = $"{wave} / {wavesCount}";
            else
                waveLabel.text = $"{wave + 1} / {wavesCount}";
        }
    }

    public Text healthLabel;

    [SerializeField]private int health;
    public int Health
    {
        get { return health; }
        set
        {
            // 1
            if (value < health)
            {
                //Camera.main.GetComponent<CameraShake>().Shake();
            }
            // 2
            health = value;
            healthLabel.text = $"{health}";
            // 2
            if (health <= 0 && !gameOver)
            {
                gameOver = true;
                //TODO: Game over logic
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        Gold = gold;
        Wave = 0;
        Health = 5;
    }
}