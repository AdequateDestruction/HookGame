using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullBlockScript : MonoBehaviour
{
    public float fallSpeed;
    public float moveSpeed;
    public bool hooked;
    public Vector3 fallToLocation;
    public GameObject shadowObject;
    public GameObject visualObject;

    Transform hookTransform;
    Vector3 buttonPos;
    bool initialFall;
    bool beingConsumed;
    
    void Update()
    {
        // Delete from new pullblock code
        if (beingConsumed)
        {
            transform.position = Vector3.MoveTowards(transform.position, buttonPos, 0.5f * Time.deltaTime);

            if (visualObject.transform.lossyScale.x > 1f)
            {
                visualObject.transform.localScale -= new Vector3(0.2f * Time.deltaTime, 0.2f * Time.deltaTime, 0);
            }
            
            if (transform.position == buttonPos)
            {
                Destroy(gameObject);
            }
        }

        // Necessary for new script
        if (hooked)
        {
            transform.position = Vector3.MoveTowards(transform.position, hookTransform.position, moveSpeed * 0.95f * Time.deltaTime);
        }

        // Delete from new pullblock code
        if (initialFall)
        {
            transform.position = Vector3.MoveTowards(transform.position, fallToLocation, fallSpeed * Time.deltaTime);
            shadowObject.transform.position = Vector3.MoveTowards(shadowObject.transform.position, transform.position, fallSpeed * Time.deltaTime);
            shadowObject.transform.localScale += new Vector3(0.1f * Time.deltaTime, 0.1f * Time.deltaTime, 0);

            if (transform.position == fallToLocation)
            {
                initialFall = false;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    // Necessary for new script
    public void gotHooked(float _speed, Transform _hookTransform)
    {
        if (!beingConsumed)
        {
            moveSpeed = _speed;
            hookTransform = _hookTransform;
            hooked = true;
        }
    }

    // Necessary for new script
    public void releaseHooked()
    {
        hooked = false;
    }

    // Delete from new pullblock code
    public void FallFromCeiling(Vector3 _fallToLocation)
    {
        fallToLocation = _fallToLocation;
        initialFall = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Delete from new pullblock code
    public void ConsumeBlock(Vector3 _buttonPos)
    {
        buttonPos = _buttonPos;
        beingConsumed = true;
        hooked = false;
    }
}
