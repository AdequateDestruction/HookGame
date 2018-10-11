using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterIdle : State
{
    WaterBossAI waterBossAI;

    public WaterIdle(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }

    public override void Enter()
    {
        Debug.Log("ExampleIdle: Enter");
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            waterBossAI.SM.SetNextState("Moving");
        }
    }

    public override void Exit()
    {
        Debug.Log("ExampleIdle: Exit");

    }

}

public class WaterMoving : State
{
    WaterBossAI waterBossAI;

    public WaterMoving(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }

    public override void Enter()
    {
        Debug.Log("ExampleIdle: Enter");
    }

    public override void Update()
    {



        Vector3 dir = waterBossAI.player.transform.position - waterBossAI.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        waterBossAI.transform.rotation = Quaternion.Slerp(waterBossAI.transform.localRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * waterBossAI.rotateSpeed);

        waterBossAI.rb.velocity = waterBossAI.transform.right * waterBossAI.movementSpeed;



        if (Input.GetKeyDown(KeyCode.Space))
        {
            waterBossAI.SM.SetNextState("Idle");
        }
    }

    public override void Exit()
    {
        Debug.Log("ExampleIdle: Exit");

    }

}


