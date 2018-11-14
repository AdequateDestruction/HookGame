using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class filterMovement : MonoBehaviour {

    public float speed;
    float moveAmount;

	void Update ()
    {
        //moves filter image sideways
        transform.Translate(+speed*Time.deltaTime,0,0);
        moveAmount += speed * Time.deltaTime;
        if (moveAmount>2||moveAmount<0)
        {
            speed *= -1;
        }
	}
}
