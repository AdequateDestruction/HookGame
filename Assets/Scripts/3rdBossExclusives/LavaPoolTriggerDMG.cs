using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPoolTriggerDMG : MonoBehaviour {

    PlayerMovement player;
    bool touchingBoss = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (touchingBoss)
            player.TakeDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision) //Boss deals damage to player on contact
    {
        if (collision.transform.tag == "Player")
        {
            touchingBoss = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            
        if (collision.transform.tag == "Player")
        {
            touchingBoss = false;
        }
    
    }  //Boss deals damage to player on contact

}
