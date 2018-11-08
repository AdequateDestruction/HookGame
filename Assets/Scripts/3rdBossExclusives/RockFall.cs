using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        transform.Translate((transform.up * -1)* 0.1f);
        Destroy(gameObject, 0.6f);
	}
}
