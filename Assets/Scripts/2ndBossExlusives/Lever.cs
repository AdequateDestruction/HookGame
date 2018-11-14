using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

    WaterStageManager waterstageManagerScript;
    bool doOnce;

	// Use this for initialization
	void Start ()
    {
        doOnce = true;
        waterstageManagerScript = GameObject.Find("WaterStageManager").GetComponent<WaterStageManager>();
	}
	
	void Update ()
    {
        //if lever is activated with Hook, Sprite is changed from HookScript
        if (doOnce && transform.GetChild(1).gameObject.activeSelf)
        {
            waterstageManagerScript.LeversActivated++;
            doOnce = false;
        }
    }

}
