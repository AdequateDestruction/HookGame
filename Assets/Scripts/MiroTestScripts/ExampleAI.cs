using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExampleAI : MonoBehaviour
{

    StateMachine sm = new StateMachine();
    public StateMachine SM { get { return sm; } }

    private void Awake()
    {
        //states
        sm.AddState(new ExampleIdle("Idle", this));
        sm.AddState(new ExampleMoving("Moving", this));
        sm.AddState(new ExampleJumping("Jumping", this));

        
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

}