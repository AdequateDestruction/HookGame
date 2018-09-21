using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public Color activeColor, defaultColor, waitingColor;
    public SpriteRenderer activeLight;
    public bool buttonActive;

    void Start()
    {
        activeLight.color = defaultColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PullBlock")
        {
            activeLight.color = activeColor;
            other.GetComponent<PullBlockScript>().ConsumeBlock(transform.position);
            buttonActive = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void ActivateButton()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        activeLight.color = waitingColor;
    }

    public void DeactivateButton()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        activeLight.color = defaultColor;
        buttonActive = false;
    }
}
