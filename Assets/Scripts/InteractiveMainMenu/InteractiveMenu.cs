using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractiveMenu : MonoBehaviour {

    public Slider slider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        slider.value = slider.value + 0.5f;
    }
}
