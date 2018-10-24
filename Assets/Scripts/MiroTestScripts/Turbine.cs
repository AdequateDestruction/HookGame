using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turbine : MonoBehaviour {

    WaterBossAI waterBossScript;
    Collider2D col;
    GameObject visual;
    float randomTime;
    bool turbineIsOn;
    float time;

    public float force;

    public float turbineActiveTime;
    [Tooltip("min&max to random activation time")]
    public float Min, Max;

    
    [System.Serializable]
    public class Direction
    {
        [Tooltip("direction which direction turbine pushes gameobjects that have rigidbody2D")]
        public bool up, down, left, right;
    }
    public Direction direction;

    private void Start()
    {
       
        //    waterBossScript = GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
        //    col = GetComponent<Collider2D>();
        //    visual = transform.GetChild(0).gameObject;
        //    turbineIsOn = false;
        //    randomTime = Random.Range(5, 10);
    }

    //private void Update()
    //{
    //    if (!turbineIsOn&&randomTime<time)
    //    {
    //        col.enabled = !col.enabled;
    //        visual.SetActive(true);
    //        time = 0f;
    //        turbineIsOn = true;
    //    }
    //    else if(turbineIsOn&&turbineActiveTime<time)
    //    {
    //        randomTime = Random.Range(5, 20);
    //        col.enabled = !col.enabled;
    //        visual.SetActive(false);
    //        time = 0f;
    //        turbineIsOn = false;
    //    }
    //    else
    //    {
    //        time += Time.deltaTime;
    //    }
    //}

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
                collision.transform.parent.GetComponent<Pathfinding.AIPath>().enabled = !collision.transform.parent.GetComponent<Pathfinding.AIPath>().enabled;
                collision.GetComponent<Rigidbody2D>().isKinematic = false;
                collision.GetComponent<ChildposToParent>().enabled=!collision.GetComponent<ChildposToParent>().enabled;
            }

            if (direction.down)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * force);

            }
            else if (direction.left)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);

            }
            else if (direction.right)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force);

            }
            else if (direction.up)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);

            }
            else
            {

            }
        }     
    }
}
