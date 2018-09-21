using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShotScript : MonoBehaviour
{
    public float moveSpeed;
    public Transform[] shotSpawnTransforms;
    public GameObject laserShotPrefab;
    public Transform spawnTransform;
    public GameObject laserWarning;

    Transform playerTransform;
    Vector3 playerPos;
    bool trackingPlayer;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        laserWarning.SetActive(false);
        StartCoroutine(LaserSpawnRoutine());
    }

    void Update()
    {
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);

        if (trackingPlayer)
        {
            playerPos = playerTransform.position;
            spawnTransform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((playerPos.y - transform.position.y), (playerPos.x - transform.position.x)) * Mathf.Rad2Deg - 90);
        }
    }

    private IEnumerator LaserSpawnRoutine()
    {
        yield return new WaitForSeconds(2f);
        ShowWarning();
        yield return new WaitForSeconds(2f);
        LockInTargetPos();
        yield return new WaitForSeconds(1.5f);
        SpawnLaserShot();
    }

    private void ShowWarning()
    {
        trackingPlayer = true;
        laserWarning.SetActive(true);
        laserWarning.GetComponent<Animator>().enabled = true;
        laserWarning.GetComponent<Animator>().Play(0);
    }

    private void LockInTargetPos()
    {
        trackingPlayer = false;
    }

    private void SpawnLaserShot()
    {
        trackingPlayer = false;
        laserWarning.SetActive(false);
        Instantiate(laserShotPrefab, spawnTransform.position, spawnTransform.rotation, null);
        StopAllCoroutines();
        Destroy(gameObject);
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
