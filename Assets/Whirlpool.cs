using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlpool : MonoBehaviour {

    float tempPlayerSpeed;
    public float suckSpeed;

    public PlayerMovement playerMovementScript;
    public WaterBossAI waterBossAIScript;

    public bool useSlow;
    public bool useTeleport;

    public bool teleported;
    public List<GameObject> pools;

    public float timer;

	// Use this for initialization
	void Start ()
    {
        tempPlayerSpeed = GameObject.Find("Player").GetComponent<PlayerMovement>().moveSpeed;
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        waterBossAIScript = GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
        playerMovementScript.notTeleported = true;

    }


    void Update ()
    {
        if (useTeleport)
        {
            if (teleported == false)
            {
                timer += Time.deltaTime;
                if (timer > 3)
                {
                    playerMovementScript.notTeleported = true;
                    teleported = true;
                    timer = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            if (useSlow)
            {
                collision.GetComponent<PlayerMovement>().moveSpeed = tempPlayerSpeed / 2;
            }
            if (useTeleport)
            {
                if (teleported && collision.GetComponent<PlayerMovement>().notTeleported)
                {
                    teleported = false;
                    collision.GetComponent<PlayerMovement>().notTeleported = false;
                    collision.transform.position = pools[Random.Range(0, 1)].transform.position;

                }
            }
        }
        if (collision.tag=="MiniBoss")
        {
            collision.GetComponent<miniBossAI>().isDead = true;
        }

        if (collision.tag=="Boss")
        {
            Debug.Log("spotted");
            waterBossAIScript.SM.SetNextState("Whirlpool");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            collision.transform.position = Vector2.MoveTowards(collision.transform.position, this.transform.position,suckSpeed*Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (useSlow)
            {
                collision.GetComponent<PlayerMovement>().moveSpeed = tempPlayerSpeed;

            }
        }
    }
}
