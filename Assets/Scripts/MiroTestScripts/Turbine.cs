using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : MonoBehaviour {

    public float force;
    public WaterBossAI waterBossScript;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>())
        {
            if (collision.name=="WaterBoss")
            {
                waterBossScript.SM.SetNextState("Idle");
            }
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * force);
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position-this.transform.position) * force);
        }
    }
}
