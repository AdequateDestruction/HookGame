using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBodyHitBox : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
        }
    }
}
