using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pole : MonoBehaviour {

    Collider2D col;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
