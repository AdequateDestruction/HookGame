using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inhale : MonoBehaviour {

    GameObject Player;
    bool suckPlayer;

    [SerializeField]
    float suckSpeed;
    public float suckTime;

    public int inhaledEnemies;

	void Start ()
    {
        Player = GameObject.Find("Player");
    }
	
	void Update ()
    {
        //moves player towards this gameobjects position
        if (suckPlayer)
        {
            Player.transform.position= Vector2.MoveTowards(Player.transform.position, this.transform.position, suckSpeed * Time.deltaTime);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MiniBoss")
        {
            //gives pathfinding new target
            collision.transform.parent.GetComponent<Pathfinding.AIDestinationSetter>().target = this.gameObject.transform.GetChild(0);
            collision.transform.parent.GetComponent<Pathfinding.AIPath>().maxSpeed = 1;
        }
        else if (collision.tag=="Player")
        {
            Player = collision.gameObject;
            suckPlayer = true;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag=="MiniBoss")
        {
            collision.transform.parent.position = Vector2.MoveTowards(collision.transform.position, this.transform.position, suckSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            suckPlayer = false;
        }
        if (collision.tag == "MiniBoss")
        {
            //gives pathfinding new target
            collision.transform.parent.GetComponent<Pathfinding.AIDestinationSetter>().target = Player.transform;
            collision.transform.parent.GetComponent<Pathfinding.AIPath>().maxSpeed = 2;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MiniBoss")
        {
            collision.gameObject.GetComponent<miniBossAI>().isDead = true;
            inhaledEnemies++;
        }
        if (collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage();
        }
    }
}
