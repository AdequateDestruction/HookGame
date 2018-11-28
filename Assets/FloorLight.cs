using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLight : MonoBehaviour {

    public Animator floorlight;
    public Whirlpool whirlpool;
    bool doOnce;


    void Start()
    {
        if (transform.GetChild(0).GetComponent<Animator>())
            floorlight = transform.GetChild(0).GetComponent<Animator>();
        if(transform.root.GetComponent<Whirlpool>())
        whirlpool = transform.root.GetComponent<Whirlpool>();
    }

	
    public void Active(bool active)
    {
        floorlight.SetBool("Active", active);
    }
 

	void Update ()
    {
        //handles animator state change for floorlights to propeller
        if (whirlpool.active&&!doOnce)
        {
            Active(true);
            doOnce = true;
        }
        else if (!whirlpool.active&&doOnce)
        {
            Active(false);
            doOnce = false;
            
        }


    }
}
