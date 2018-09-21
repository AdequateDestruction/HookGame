using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeShotScript : MonoBehaviour
{
    public float moveSpeed;
    public float shotLifeTime;

    void Start()
    {
        StartCoroutine(LifeTime());
    }

    void Update()
    {
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(shotLifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
            Destroy(gameObject);
        }
        else if (other.tag == "PullBlock" || other.tag == "StaticBlock" || other.tag == "DeflectBlock")
        {
            Destroy(gameObject);
        }
    }
}
