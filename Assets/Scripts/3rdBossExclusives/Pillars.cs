using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillars : MonoBehaviour {
    public int pillarHP = 200;
    ThirdBossSM bossSM;
    bool doOnce = true;
    Animator pillarAnimator;
    float pillarTimer;
    bool animTimerOn;
	// Use this for initialization
	void Start () {
        bossSM = GameObject.FindGameObjectWithTag("Boss").GetComponent<ThirdBossSM>();
        pillarAnimator = gameObject.GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {

        if (pillarHP < 0)
        {
            Destroy(gameObject, 0.5f);
        }
            
        if(animTimerOn)
        {
            pillarTimer = pillarTimer - 0.1f;

            if(pillarTimer < 0)
            {
                animTimerOn = false;
            }
        }

    }

    public void TakeDamage()
    {
        pillarHP = pillarHP - 1;
        pillarAnimator.SetBool("PillarTakesDMG", true);
        animTimerOn = true;
        pillarTimer = 1;

        if (pillarHP < 0 && doOnce)
        {
            doOnce = false;
            bossSM.ThirdPhaseTakeDMG();

            //Pillar falling animation here

        }
    }
}
