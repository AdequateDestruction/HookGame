using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    public float hookSpeed;         // How fast the hook should move
    public float flightTime;        // The time the hook will fly forward before returning, use this and hookSpeed to determine how long a hook is "active"
    public bool goingOut;           // Is the hook "active", should only be true when the hook is launched and you want it to be able to interact
    public bool pullingPlayer;      // Is the player being pulled along by this hook
    public bool ripping;            // Is the hook in ripping mode
    public AudioSource hitAS;

    int activeCounter;              // The number of frames since the hook was launched, used to stop the hook from interacting with objects that are too close and causing bugs due to the hook being "inside" objects,
                                    // change this to a time.time based timer to account for frame rate changes.
    float stopTime;
    GameObject[] TrapPitColliders;  // For boss1 stage
    GameObject ripBlock;
    Transform playerTransform;
    PullBlockScript pullBlock;
    Vector3 ripOffSet;

    void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().gameObject.transform;
        TrapPitColliders = GameObject.FindGameObjectsWithTag("TrapBlockColliders");
        stopTime = Time.time + flightTime;
        goingOut = true;
        ripping = false;
        activeCounter = 0;
    }

    void Update()
    {
        if (goingOut)                           // When the hook is launched, move forward until max flightTime is achieved
        {
            if (Time.time <= stopTime)
            {
                transform.position += transform.up * hookSpeed * Time.deltaTime;
            }
            else
            {
                goingOut = false;
            }

            activeCounter++;
        }
        else if (!goingOut && pullingPlayer)    // Remain stationary when the player is being pulled
        {

        }
        else if (!goingOut && ripping)          // If the hook is in the ripping state, move with the object being ripped
        {
            transform.position = Vector3.MoveTowards(transform.position, ripBlock.transform.position - ripOffSet, hookSpeed * Time.deltaTime);
        }
        else                                    // If goingOut is false, return to player
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, hookSpeed * Time.deltaTime);
        }
    }

    // Check what to do when colliding with tagged objects, make sure to tag anything you want the hook to interact with appropriately
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (goingOut)
        {

            switch (other.tag)
            {
                case "PullBlock":
                    if (activeCounter > 3)
                    {
                        goingOut = false;
                        FindObjectOfType<PlayerMovement>().PullHooked();                    // Let playerMovement know that something has been hooked, stopping the player
                        pullBlock = other.GetComponent<PullBlockScript>();                  // Store the pullBlock
                        pullBlock.gotHooked(hookSpeed, gameObject.transform);               // Initiate the pullBlock getting pulled
                        GetComponentInChildren<Animator>().Play("HookAttachedAnimation");   // Show visual for hook attaching
                    }
                    else
                    {
                        goingOut = false;
                    }
                    hitAS.Play();
                    break;

                case "Boss":
                    goingOut = false;
                    hitAS.Play();
                    break;

                case "DeflectBlock":
                    goingOut = false;
                    hitAS.Play();
                    break;

                case "StaticBlock":
                    if (activeCounter > 1)
                    {
                        goingOut = false;
                        pullingPlayer = true;
                        FindObjectOfType<PlayerMovement>().StaticHooked();
                        GetComponentInChildren<Animator>().Play("HookAttachedAnimation");

                        if (other.name == "BigMushroom")                                    // Show animation if the block is a big mushroom
                        {
                            other.gameObject.GetComponentInChildren<Animator>().Play("BigMushroomFlashAnimation");
                        }

                        if (TrapPitColliders.Length != 0)                                   // Workaround to let the player pass through the solid colliders around the trap pits in boss1 stage. Not necessary for other scenes.
                        {
                            foreach (GameObject go in TrapPitColliders)
                            {
                                go.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        goingOut = false;
                    }
                    hitAS.Play();
                    break;

                case "TentacleBlock":
                    goingOut = false;
                    other.transform.parent.GetComponent<TentacleScript>().hookHit();
                    hitAS.Play();
                    break;

                case "Core":
                    ripBlock = other.gameObject;
                    ripOffSet = other.transform.position - transform.position;
                    goingOut = false;
                    ripping = true;
                    other.GetComponent<CoreScript>().StartRipping(this);
                    FindObjectOfType<PlayerMovement>().StartRipping();
                    hitAS.Play();
                    GetComponentInChildren<Animator>().Play("HookAttachedAnimation");
                    break;

                case "TennisShot":
                    goingOut = false;
                    other.GetComponentInParent<TennisShotScript>().HookShotPong((playerTransform.position + transform.position) * 0.5f);
                    break;

                case "Switch":
                    if (activeCounter > 1)
                    {
                        goingOut = false;
                        hitAS.Play();
                        Debug.Log("Switch");

                    }
                    break;

                case "MiniBoss":
                    hitAS.Play();
                    goingOut = false;
                    other.transform.parent.gameObject.SetActive(false);
                    break;

                default:
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)                                      // Once the hook returns to the player, wait for a frame before destroying the hook. OnTriggerStay instead of Enter to prevent instant despawn.
    {
        if (other.tag == "Player" && !goingOut)
        {
            FindObjectOfType<PlayerMovement>().HookReturned();

            if (pullBlock != null)
            {
                pullBlock.releaseHooked();
            }

            if (TrapPitColliders.Length != 0)
            {
                foreach (GameObject go in TrapPitColliders)
                {
                    go.SetActive(true);
                }
            }

            Destroy(gameObject);
        }
    }

    public void PlayerDamaged()                                                         // Stop the hook and return it if the player is damaged while a hook is going out. Can be changed for balance reasons.
    {
        goingOut = false;
        pullingPlayer = false;
        ripping = false;

        if (pullBlock != null)
        {
            pullBlock.releaseHooked();
        }
    }

    public void RippingEnd()
    {
        ripping = false;
    }
}
