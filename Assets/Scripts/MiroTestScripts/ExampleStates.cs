using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleIdle : State
{
    ExampleAI exampleAI;

    public ExampleIdle(string stateID, ExampleAI exampleAI) : base(stateID)
    {
        this.exampleAI = exampleAI;
    }

    public override void Enter()
    {
        Debug.Log("ExampleIdle: Enter");
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            exampleAI.SM.SetNextState("Moving");
        }
    }

    public override void Exit()
    {
        Debug.Log("ExampleIdle: Exit");

    }

}

public class ExampleMoving : State
{
    ExampleAI exampleAI;

    public ExampleMoving(string stateID, ExampleAI exampleAI) : base(stateID)
    {
        this.exampleAI = exampleAI;
    }

    public override void Enter()
    {
        Debug.Log("ExampleMoving: Enter");

    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            exampleAI.SM.SetNextState("Jumping");
        }
    }

    public override void Exit()
    {
        Debug.Log("ExampleMoving: Exit");

    }

}

public class ExampleJumping : State
{
    ExampleAI exampleAI;

    public ExampleJumping(string stateID, ExampleAI exampleAI) : base(stateID)
    {
        this.exampleAI = exampleAI;
    }

    public override void Enter()
    {
        Debug.Log("ExampleJumping: Enter");

    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            exampleAI.SM.SetNextState("Idle");
        }
    }

    public override void Exit()
    {
        Debug.Log("ExampleJumping: Exit");

    }

}