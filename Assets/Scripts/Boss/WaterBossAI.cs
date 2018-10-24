using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBossAI : MonoBehaviour {


    public GameObject player;
    public Rigidbody2D rb;
    public float movementSpeed;
    public float rotateSpeed;


    StateMachine sm = new StateMachine();
    public StateMachine SM { get { return sm; } }

    private void Awake()
    {
        //states
        sm.AddState(new WaterIdle("Idle", this));
        sm.AddState(new WaterTurbine("Turbine", this));
        sm.AddState(new WaterMoving("Moving", this));

    }

    void Start()
    {
        StartCoroutine(AI());
    }

    private void Update()
    {

    }

    IEnumerator AI()
    {

        while (true)
        {
            sm.Update();
            yield return null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StaticBlock")
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0f;
            SM.SetNextState("Idle");
        }
    }

}
