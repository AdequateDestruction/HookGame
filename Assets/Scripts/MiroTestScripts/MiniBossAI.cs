using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniBossAI : MonoBehaviour
{


    //public GameObject player;
    //public float rotateSpeed;
    //public float movementSpeed;
    //public Rigidbody2D rb;




    //StateMachine sm = new StateMachine();
    //public StateMachine SM { get { return sm; } }

    //private void Awake()
    //{
    //    //states
    //    sm.AddState(new MiniBossIdle("Idle", this));
    //    sm.AddState(new MiniBossMoving("Moving", this));



    //}

    void Start()
    {
        InvokeRepeating("carving",0.3f,0.3f);
        //StartCoroutine(AI());
    //    sm.SetNextState("Moving");
    }

    private void Update()
   {
        
        //    //debug
        //    Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        //    Debug.DrawRay(transform.position, transform.right, Color.red);
    }

    //IEnumerator AI()
    //{
    //            AstarPath.active.Scan();
    //    //    while (true)
    //    //    {
    //    //        sm.Update();
    //    Debug.Log("hello");
    //           yield return new WaitForSeconds(0.3f);
    //    //    }
    //}

    void carving()
    {
        AstarPath.active.Scan();
    }

}