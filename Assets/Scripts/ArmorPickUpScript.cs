using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickUpScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("RIPPING!!!!");
            GetComponentInParent<CoreScript>().ArmorPickUp();
        }
        if (other.tag == "Hook")
        {
            Debug.Log("RIPPING!!!!");
            GetComponentInParent<CoreScript>().ArmorPickUp();
        }
    }
}
