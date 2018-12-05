using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

    WaterStageManager waterstageManagerScript;
    Animator anim;
    bool doOnce;

	// Use this for initialization
	void Start ()
    {
        doOnce = true;
        waterstageManagerScript = GameObject.Find("WaterStageManager").GetComponent<WaterStageManager>();
        anim = this.gameObject.GetComponent<Animator>();
	}
	
	void Update ()
    {
        //if lever is activated with Hook, Sprite is changed from HookScript
       //if (doOnce && transform.GetChild(1).gameObject.activeSelf)
       // {
       //     waterstageManagerScript.LeversActivated++;
       //     doOnce = false;
       // }
    }

    public void Inactive()
    {
        if (doOnce)
        {
            anim.SetBool("Inactive", true);
            waterstageManagerScript.LeversActivated++;
            doOnce = false;
        }
    }

}
