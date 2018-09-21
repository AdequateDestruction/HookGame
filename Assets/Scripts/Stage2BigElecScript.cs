using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2BigElecScript : MonoBehaviour
{
    public float lifeTime;

    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(DeathCount());
    }

    private IEnumerator DeathCount()
    {
        yield return new WaitForSeconds(lifeTime / 4);
        GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(lifeTime / 4);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(lifeTime / 2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boss")
        {
            other.gameObject.GetComponent<BossScript>().Stage2TakeDamage();
        }
    }
}
