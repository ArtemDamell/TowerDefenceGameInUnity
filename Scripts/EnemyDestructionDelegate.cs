using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestructionDelegate : MonoBehaviour
{
    public delegate void EnemyDelegate(GameObject enemy);
    public EnemyDelegate enemyDelegate;

    /// <summary>
    /// Invokes the enemy delegate when the game object is destroyed.
    /// </summary>
    /// <returns>
    /// Void.
    /// </returns>
    void OnDestroy()
    {
        enemyDelegate?.Invoke(gameObject);
    }
}
