using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carving : MonoBehaviour {

    [Tooltip("time between carving the grid")]
    public float repeatRate;

	void Start ()
    {
        InvokeRepeating("carving", 0.3f, repeatRate);
    }


    void carving()
    {
        AstarPath.active.Scan();
    }
}
