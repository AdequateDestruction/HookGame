using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillars : MonoBehaviour {
    public int pillarHP = 200;
    ThirdBossSM bossSM;
    bool doOnce = true;
	// Use this for initialization
	void Start () {
        bossSM = GameObject.FindGameObjectWithTag("Boss").GetComponent<ThirdBossSM>();
	}
	
	// Update is called once per frame
	void Update () {

        if (pillarHP < 0 && transform.position.y < 180)
        {
            transform.Rotate(Vector3.right * (55 * Time.deltaTime));
            Destroy(gameObject, 2.5f);
        }
            
        
    }
    public void TakeDamage()
    {
        pillarHP = pillarHP - 1;

        if(pillarHP < 0 && doOnce)
        {
            doOnce = false;
            bossSM.ThirdPhaseTakeDMG();
            //Pillar falling animation here

        }
    }
}
