using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Variable Declaration
    public GameObject enemyPrefab;
    private int enemyCount;
    private float leftBound = 0;
    private float rightBound = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyAI>().Length;

        if (enemyCount == 0)
        {
            SpawnEnemies(3);
        }
    }

    //Generate random spawn position
    private Vector3 GenerateSpawnPosition()
    {
        float posX = Random.Range(leftBound, rightBound);
        float posZ = Random.Range(leftBound, rightBound);

        Vector3 spawnPos = new Vector3(posX, enemyPrefab.transform.position.y, posZ);

        return spawnPos;
    }

    //Spawn enemy at random position
    void SpawnEnemies(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
