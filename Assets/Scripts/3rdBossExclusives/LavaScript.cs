using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour {

    PlayerMovement player;
    ThirdBossSM BossSM;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        BossSM = GameObject.FindGameObjectWithTag("Boss").GetComponent<ThirdBossSM>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && BossSM.lavaActive)
        {
            print("Player burns in lava");
            player.TakeDamage();
        }
    }
}
