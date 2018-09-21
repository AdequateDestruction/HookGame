using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPrefabScript : MonoBehaviour
{

    public float laserLifeTime;

    void Start()
    {
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(laserLifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
        }
    }
}
