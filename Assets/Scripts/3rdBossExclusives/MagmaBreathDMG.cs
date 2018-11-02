using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaBreathDMG : MonoBehaviour {

    PlayerMovement player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnParticleCollision(GameObject other)
    {
        if(other.transform.tag == "Player")
        {
            player.TakeDamage();
        }

        if(other.transform.tag == "Pillar")
        {
            other.transform.GetComponent<Pillars>().TakeDamage();
        }
        
    }

}

