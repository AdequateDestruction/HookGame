using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowForProjectiles : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    float risingAlpha = 0;
    //public for customization
    public float invokeStart = 0.1f;
    public float invokeRepeat = 0.5f;
    public float alphaMultiplier = 1f;
    public bool big = false;
    public bool shatter = false;
    public GameObject rockCluster;

    public Vector2 tmp_pos;

	// Use this for initialization
	void Start () {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        tmp_pos = transform.position;

        if(big)
        {
            spriteRenderer.gameObject.transform.localScale = spriteRenderer.gameObject.transform.localScale * 3;
        }
        
        InvokeRepeating("AlphaChange", invokeStart , invokeRepeat);
        
	}
	
	// Update is called once per frame
	void Update () {

        if(rockCluster != null && shatter)
        {
            //rockCluster.transform.position = transform.position; //Epic band aid fix for rockclusters exploding in the previous shadows' spot
            //print(rockCluster.transform.position + " and " + transform.position);
        }
        
    }

    void AlphaChange()
    {
        if(risingAlpha <= 1)
        risingAlpha = risingAlpha + alphaMultiplier;

        if(big)
        {
            //print(spriteRenderer.color.a);
        }

        if (risingAlpha >= 0.90f)
        {
            risingAlpha = 1;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);

            if (/*big && */shatter)
            {
                Instantiate(rockCluster,transform.position,transform.rotation);
                //rockCluster.transform.position = tmp_pos;
            }

            CancelInvoke();
            if(big)
            print("Game object destroyed");
            //risingAlpha = 0;
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
        }
        

    }
}
