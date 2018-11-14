using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollisionDMG : MonoBehaviour {

    //Used to make colliding with the boss deal damage

    PlayerMovement player;
    bool touchingBoss = false;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {

		if(touchingBoss)
        player.TakeDamage();
	}

    private void OnCollisionEnter2D(Collision2D collision) //Boss deals damage to player on contact
    {
        if (collision.transform.tag == "Player")
        {
            touchingBoss = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) //Boss deals damage to player on contact
    {
        if (collision.transform.tag == "Player")
        {
            touchingBoss = false;
        }
    }
}
