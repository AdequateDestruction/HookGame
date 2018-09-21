using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterShotScript : MonoBehaviour
{
    public float moveSpeed;
    public float shotSpawnCooldown;
    public Transform[] shotSpawnTransforms;
    public GameObject basicShotPrefab;

    void Start()
    {
        StartCoroutine(SpawnSmallShots());
    }

    void Update()
    {
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
    }

    private IEnumerator SpawnSmallShots()
    {
        yield return new WaitForSeconds(shotSpawnCooldown);
        SpawnShots();
        yield return new WaitForSeconds(shotSpawnCooldown);
        SpawnShots();
        yield return new WaitForSeconds(shotSpawnCooldown);
        SpawnShots();
        yield return new WaitForSeconds(shotSpawnCooldown);
        SpawnShots();
        yield return new WaitForSeconds(shotSpawnCooldown);
        SpawnShots();
    }

    private void SpawnShots()
    {
        foreach (Transform trans in shotSpawnTransforms)
        {
            Instantiate(basicShotPrefab, trans.position, trans.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
            StopAllCoroutines();
            Destroy(gameObject);
        }
        else if (other.tag == "PullBlock" || other.tag == "StaticBlock" || other.tag == "DeflectBlock")
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
