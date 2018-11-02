using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillars : MonoBehaviour {
    public int pillarHP = 200;
    ThirdBossSM bossSM;
	// Use this for initialization
	void Start () {
        bossSM = GameObject.FindGameObjectWithTag("Boss").GetComponent<ThirdBossSM>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void TakeDamage()
    {
        pillarHP = pillarHP - 1;

        if(pillarHP < 0)
        {
            bossSM.ThirdPhaseTakeDMG();
            //Pillar falling animation here
            Destroy(gameObject);
        }
    }
}
