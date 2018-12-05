using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    Animator anim;

    void Start ()
    {
        //pooling
        enemy = new List<GameObject>();
        anim = GetComponent<Animator>();

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
    private void Update()
    {
            
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
            //if (SceneManager.GetActiveScene)
            //{

            //}
            anim.SetTrigger("Spawn");
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
