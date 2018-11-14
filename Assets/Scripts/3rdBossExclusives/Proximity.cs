using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour {
    public bool playerInRange = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Used to detect when the player is within the range of MagmaBreat() information is relayed back to ThirdBossSM script

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       if(collision.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
