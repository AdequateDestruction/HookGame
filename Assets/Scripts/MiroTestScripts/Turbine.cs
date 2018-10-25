using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Inspector DropDown
public enum Direction
{
    down,up,left,right
}

public class Turbine : MonoBehaviour {

    WaterBossAI waterBossScript;
    Collider2D col;
    GameObject visual;
    float randomTime;
    bool turbineIsOn;
    float time;

    [Tooltip("direction which direction turbine pushes gameobjects that have rigidbody2D")]
    public Direction direction;
    public float force;
    public float turbineActiveTime;
    [Tooltip("min&max to random activation time")]
    public float Min, Max;

    private void Start()
    {
        waterBossScript = GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
        col = GetComponent<Collider2D>();
        visual = transform.GetChild(0).gameObject;
        turbineIsOn = false;
        col.enabled = !col.enabled;
        visual.SetActive(false);
        randomTime = Random.Range(Min, Max);
    }

    private void Update()
    {
        //random activation
        if (!turbineIsOn && randomTime < time)
        {
            col.enabled = !col.enabled;
            visual.SetActive(true);
            time = 0f;
            turbineIsOn = true;
        }
        else if (turbineIsOn && turbineActiveTime < time)
        {
            randomTime = Random.Range(Min, Max);
            col.enabled = !col.enabled;
            visual.SetActive(false);
            time = 0f;
            turbineIsOn = false;
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {       
        if (collision.GetComponent<Rigidbody2D>())
        {
            if (collision.gameObject.name=="WaterBoss")
            {
                waterBossScript.SM.SetNextState("Turbine");
            }
            if (collision.tag == "MiniBoss")
            {    
                //If MiniBoss collides with turbine disables pathfinding and changes rigidbody to dynamic.
                //ChildposToParent scripts job is to change parents gameobject position when child gameobjects position changes. Rigidbody is attached to child object
                collision.transform.parent.GetComponent<Pathfinding.AIPath>().enabled = !collision.transform.parent.GetComponent<Pathfinding.AIPath>().enabled;
                collision.GetComponent<Rigidbody2D>().isKinematic = false;
                collision.GetComponent<ChildposToParent>().enabled=!collision.GetComponent<ChildposToParent>().enabled;
            }

            switch (direction)
            {
                case Direction.down:
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * force);
                    break;
                case Direction.up:
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);
                    break;
                case Direction.left:
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);
                    break;
                case Direction.right:
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force);
                    break;
                default:
                    break;
            }
        }     
    }
}
