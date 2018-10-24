using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniBossAI : MonoBehaviour {

    public GameObject Parent;
    public Vector2 spawnpos;

	void Start ()
    {
        Parent = transform.parent.gameObject;
        spawnpos = new Vector2(Parent.transform.position.x, Parent.transform.position.y);
	}
	
	void Update () {
		
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "StaticBlock")
        {            
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0f;
            GetComponent<ChildposToParent>().enabled = !GetComponent<ChildposToParent>().enabled;
            Parent.GetComponent<Pathfinding.AIPath>().enabled = !Parent.GetComponent<Pathfinding.AIPath>().enabled;
            Parent.transform.position = spawnpos;
            GetComponent<Rigidbody2D>().isKinematic = true;
            Parent.SetActive(false);            
        }
    }

    
}
