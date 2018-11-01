using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InvokeRepeating("AllChildGone", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AllChildGone()
    {
        if (gameObject.transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
