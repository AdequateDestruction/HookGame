using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

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

    public bool spawnEnemy=true;

    void Start ()
    {
        //pooling
        enemy = new List<GameObject>();
        for (int i = 0; i < enemyAmountToPool; i++)
        {
            newEnemy = (GameObject)Instantiate(enemyPrefab);
            newEnemy.SetActive(false);           
            enemy.Add(newEnemy);
            newEnemy.transform.parent = null;
        }

        if (spawnEnemy)
        {
            InvokeRepeating("SpawnEnemy", invokeStartTime, invokeRepeatTime);

        }
        else 
        {
            Invoke("SpawnEnemy", 1f);

        }
    }

    public void invoketentacle(float time)
    {
        Invoke("SpawnEnemy", time);
    }

    public void SpawnEnemy()
    {
        newEnemy = GetPooledEnemies();
        if (newEnemy)
        {
            newEnemy.transform.position = transform.position;
            newEnemy.transform.rotation = transform.rotation;
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
