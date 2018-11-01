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

        InvokeRepeating("SpawnEnemy", invokeStartTime, invokeRepeatTime);
	}

    public void SpawnEnemy()
    {
        newEnemy = GetPooledEnemies();
        if (newEnemy)
        {
            newEnemy.transform.position = transform.position;
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
