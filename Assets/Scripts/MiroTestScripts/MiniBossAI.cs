using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniBossAI : MonoBehaviour
{

    public GameObject player;
    public float rotateSpeed;
    public float movementSpeed;
    public Rigidbody2D rb;




    StateMachine sm = new StateMachine();
    public StateMachine SM { get { return sm; } }

    private void Awake()
    {
        //states
        sm.AddState(new MiniBossIdle("Idle", this));
        sm.AddState(new MiniBossMoving("Moving", this));



    }

    void Start()
    {
        StartCoroutine(AI());

        sm.SetNextState("Moving");
    }

    private void Update()
    {

        //debug
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, transform.right, Color.red);
    }

    IEnumerator AI()
    {

        while (true)
        {
            sm.Update();
            yield return null;
        }
    }

}