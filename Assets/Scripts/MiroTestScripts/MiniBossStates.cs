using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossIdle : State
{
    MiniBossAI miniBossAI;

    public MiniBossIdle(string stateID, MiniBossAI miniBossAI) : base(stateID)
    {
        this.miniBossAI = miniBossAI;
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {


    }

    public override void Exit()
    {

    }

}

public class MiniBossMoving : State
{
    MiniBossAI miniBossAI;

    public MiniBossMoving(string stateID, MiniBossAI miniBossAI) : base(stateID)
    {
        this.miniBossAI = miniBossAI;
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        Vector3 dir = miniBossAI.player.transform.position - miniBossAI.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        miniBossAI.transform.rotation = Quaternion.Slerp(miniBossAI.transform.localRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * miniBossAI.rotateSpeed);

        miniBossAI.rb.velocity = miniBossAI.transform.right * miniBossAI.movementSpeed;


    }

    public override void Exit()
    {

    }

}