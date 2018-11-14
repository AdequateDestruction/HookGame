using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour {

    //Used for the cosmetic effect of rock falling on the shadows

    Vector2 destroyPos = new Vector2(100,100);
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        transform.Translate((transform.up * -1)* 0.1f);

        if(gameObject.transform.position.y < destroyPos.y)
        {
            Destroy(gameObject);
        }
        
	}

    public void ShadowsPos(Vector2 pos)
    {
        destroyPos = pos;
    }
}
