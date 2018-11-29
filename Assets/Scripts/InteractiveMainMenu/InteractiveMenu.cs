using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractiveMenu : MonoBehaviour {

    public Slider slider;
    private bool onButton = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (onButton)
        {
            if (gameObject.tag =="BtnRight")
            {
                Debug.Log("oikealle");
                slider.value = slider.value + 0.2f;
            }

            if (gameObject.tag == "BtnLeft")
            {
                Debug.Log("oikealle");
                slider.value = slider.value - 0.2f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "BtnRight")
        {
            onButton = true; 
        }

        if (collision.gameObject.tag == "Player" && gameObject.tag == "BtnLeft")
        {
            onButton = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onButton = false;
        }

        if (collision.gameObject.tag == "Player")
        {
            onButton = false;
        }
    }
}
