using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowForProjectiles : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    float risingAlpha = 0;
    //public for customization
    public float invokeStart = 0;
    public float invokeRepeat = 0.5f;
    public float alphaMultiplier = 0.05f;
    public bool big = false;
    public bool shatter = false;
    public GameObject rockCluster;

	// Use this for initialization
	void Start () {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();


        if(big)
        {
            spriteRenderer.gameObject.transform.localScale = spriteRenderer.gameObject.transform.localScale * 3;
        }
        
        InvokeRepeating("AlphaChange", 0 , 0.5f);
        
	}
	
	// Update is called once per frame
	void Update () {

        
    }

    void AlphaChange()
    {
        
        risingAlpha += alphaMultiplier;
        if (risingAlpha >= 0.85f)
        {
            if(big && shatter)
            {
                Instantiate(rockCluster);
                rockCluster.transform.position = transform.position;
            }

            CancelInvoke();
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
        }
        

    }
}
