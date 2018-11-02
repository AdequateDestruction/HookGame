using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

    public WaterStageManager waterstageManagerScript;
    bool doOnce;

	// Use this for initialization
	void Start ()
    {
        doOnce = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (doOnce && transform.GetChild(1).gameObject.activeSelf)
        {
            waterstageManagerScript.LeversActivated++;
            doOnce = false;
        }
    }

}
