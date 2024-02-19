using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject Goblin;
    public bool isSpawn;

    private void Start()
    {
        isSpawn = false;
    }

    public void doorSpawnEnemy()
    {
        Instantiate(Goblin, transform.position, Quaternion.identity);
        Debug.Log("spawned enemy");
        isSpawn = false;
    }
}
