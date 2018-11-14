using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildposToParent : MonoBehaviour {

    void Update ()
    {
        transform.parent.position = transform.position;
	}

}
