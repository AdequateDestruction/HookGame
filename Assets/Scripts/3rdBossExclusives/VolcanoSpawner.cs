using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoSpawner : MonoBehaviour {

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    int enemyAmountToPool;

    List<GameObject> enemy;
    GameObject newEnemy;

    [Header("Invoke repeating")]
    [SerializeField]
    float invokeStartTime;
    [SerializeField]
    float invokeRepeatTime;

    PlayerMovement playerMovement;
    


    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        //pooling
        enemy = new List<GameObject>();
        for (int i = 0; i < enemyAmountToPool; i++)
        {
            newEnemy = (GameObject)Instantiate(enemyPrefab);
            newEnemy.SetActive(false);
            enemy.Add(newEnemy);
            newEnemy.transform.parent = transform;
        }

        InvokeRepeating("SpawnEnemy", invokeStartTime, invokeRepeatTime);
    }

    public void SpawnEnemy()
    {
        newEnemy = GetPooledEnemies();
        if (newEnemy)
        {
            float xRandom = Random.Range(-7, 7);
            float yRandom = Random.Range(-7, 7);
            Vector2 spawnPos = new Vector2(playerMovement.transform.position.x + xRandom, playerMovement.transform.position.y + yRandom);
            newEnemy.transform.position = spawnPos;
            newEnemy.SetActive(true);
        }

    }

    //get next enemy from list
    public GameObject GetPooledEnemies()
    {

        for (int i = 0; i < enemy.Count; i++)
        {

            if (!enemy[i].activeInHierarchy) 
            {
                return enemy[i];
            }
        }

        return null;
    }
}
