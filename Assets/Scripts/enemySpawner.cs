using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject Goblin;

    public void doorSpawnEnemy(int numEnemies)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            Instantiate(Goblin, transform.position, Quaternion.identity);

            Debug.Log("spawned enemy, " + i);
        }
    }
}
