using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LBScrollBar : MonoBehaviour {

    public Scrollbar scrollbar;
    private bool onButton;
    public GameObject buttondown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (onButton)
        {
            if (gameObject.tag == "BtnUp")
            {
                scrollbar.value = scrollbar.value + 0.005f;
            }

            if (gameObject.tag == "BtnDown")
            {
                scrollbar.value = scrollbar.value - 0.005f;
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
