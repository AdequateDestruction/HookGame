using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractiveMenu : MonoBehaviour {

    public Slider slider;
    private bool onButton = false;
    public GameObject buttondown;

	
	// Update is called once per frame
	void Update ()
    {
        if (onButton)
        {
            if (gameObject.tag =="BtnRight")
            {
                Debug.Log("oikealle");
                slider.value = slider.value + 0.08f;
            }

            if (gameObject.tag == "BtnLeft")
            {
                Debug.Log("oikealle");
                slider.value = slider.value - 0.08f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onButton = true;
            buttondown.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onButton = false;
            buttondown.SetActive(false);
        }
    }
}
