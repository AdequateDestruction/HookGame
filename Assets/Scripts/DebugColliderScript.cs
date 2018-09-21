using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugColliderScript : MonoBehaviour
{
    public SpriteRenderer[] colliderVisuals;

    bool visualsShown;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!visualsShown)
            {
                visualsShown = true;

                foreach (SpriteRenderer spriteR in colliderVisuals)
                {
                    spriteR.enabled = true;
                }
            }
            else
            {
                visualsShown = false;

                foreach (SpriteRenderer spriteR in colliderVisuals)
                {
                    spriteR.enabled = false;
                }
            }
        }
    }
}
