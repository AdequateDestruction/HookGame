using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShotScript : MonoBehaviour
{
    public float moveSpeed;

    void Update()
    {
        transform.Translate(0f, moveSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boss")
        {
            other.gameObject.GetComponent<BossScript>().Stage2TakeDamage();
        }
        else if (other.tag == "OutOfBounds")
        {
            Destroy(gameObject);
        }
    }
}
