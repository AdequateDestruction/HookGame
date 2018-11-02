﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IDLE
public class WaterIdle : State
{
    WaterBossAI waterBossAI;
    float timer;

    public WaterIdle(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }

    public override void Enter()
    {
        Debug.Log(waterBossAI.SM.CurrentState);
    }

    public override void Update()
    {
        if (waterBossAI.frenzyed)
        {
            timer += Time.deltaTime;

            if (timer > 3)
            {
                waterBossAI.SM.SetNextState("Moving");
            }
        }

    }

    public override void Exit()
    {
        Debug.Log("ExampleIdle: Exit");
        timer = 0;
    }

}

//WAITING
public class WaterWaiting : State
{
    WaterBossAI waterBossAI;

    public WaterWaiting(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }

    public override void Enter()
    {
        Debug.Log(waterBossAI.SM.CurrentState);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("ExampleIdle: Exit");

    }

}



//DASH
    public class WaterDash : State
{
    WaterBossAI waterBossAI;
    Vector2 dir;
    float angle;

    public WaterDash(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }
    public override void Enter()
    {
        //Debug.Log(waterBossAI.SM.CurrentState);
        //dir = waterBossAI.player.transform.position - waterBossAI.transform.position;
        //angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    public override void Update()
    {
        //When waterboss has found player with raycast dashes towards player where player is at that moment.
        if (waterBossAI.playerHit)
        {
            waterBossAI.rb.velocity =  waterBossAI.transform.right * waterBossAI.dashSpeed;
        }
        else
        {
            waterBossAI.transform.rotation = Quaternion.Slerp(waterBossAI.transform.localRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * waterBossAI.rotateSpeed);
        }
    }
    public override void Exit()
    {
        waterBossAI.minibossKilled = 0;
        waterBossAI.playerHit = false;
    }
}

//TURBINE
//this state is activated from turbine.cs, when boss hits turbine.
public class WaterTurbine: State
{
    WaterBossAI waterBossAI;

    public WaterTurbine(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }

    public override void Enter()
    {
        Debug.Log(waterBossAI.SM.CurrentState);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }

}

//MOVING
public class WaterMoving : State
{
    WaterBossAI waterBossAI;

    public WaterMoving(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }

    public override void Enter()
    {
        Debug.Log(waterBossAI.SM.CurrentState);
    }

    public override void Update()
    {
        //moves and rotates towards player.
        Vector3 dir = waterBossAI.player.transform.position - waterBossAI.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        waterBossAI.transform.rotation = Quaternion.Slerp(waterBossAI.transform.localRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * waterBossAI.rotateSpeed);
        waterBossAI.rb.velocity = waterBossAI.transform.right * waterBossAI.movementSpeed;
    }

    public override void Exit()
    {
    }

}


//WATERBREATH
public class WaterBreath : State
{
    WaterBossAI waterBossAI;
    float timer;

    public WaterBreath(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }

    public override void Enter()
    {
        Debug.Log(waterBossAI.SM.CurrentState);
        waterBossAI.particle.Play();
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer>3)
        {
            waterBossAI.SM.SetNextState("Moving");
        }
    }

    public override void Exit()
    {
        waterBossAI.particle.Stop();
        timer = 0;

    }

}


//WHIRLPOOL
public class WaterWhirlpool : State
{
    WaterBossAI waterBossAI;

    public WaterWhirlpool(string stateID, WaterBossAI waterBossAI) : base(stateID)
    {
        this.waterBossAI = waterBossAI;
    }

    public override void Enter()
    {
        Debug.Log(waterBossAI.SM.CurrentState);
        waterBossAI.rb.velocity = Vector2.zero;

    }

    public override void Update()
    {
       
        waterBossAI.transform.Rotate(0, 0, waterBossAI.WhirlpoolRotSpeed*Time.deltaTime);
    }

    public override void Exit()
    {
        waterBossAI.particle.Stop();

    }

}



