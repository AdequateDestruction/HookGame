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
    public GameObject fallingRockSprite;
    float xMulti = 3f;

	// Use this for initialization
	void Start () {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if(big)
        {
            spriteRenderer.gameObject.transform.localScale = spriteRenderer.gameObject.transform.localScale * 3;
        }
        
        InvokeRepeating("AlphaChange", invokeStart , invokeRepeat);
        
	}
	
	// Update is called once per frame
	void Update () {


        
    }

    void AlphaChange()
    {
        if(risingAlpha <= 1)
        risingAlpha = risingAlpha + alphaMultiplier;

        if (risingAlpha >= 0.90f)
        {
            if(big)
            {
                xMulti = 5;
            }
                GameObject rockObject =  Instantiate(fallingRockSprite, new Vector2(transform.position.x + xMulti, transform.position.y), transform.rotation);

            if(big)
            {
                rockObject.transform.localScale = rockObject.transform.localScale * 3;
            }


            risingAlpha = 1;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);

            if (shatter)
            {
                Instantiate(rockCluster,transform.position,transform.rotation);
            }

            CancelInvoke();
            if(big)
            print("Game object destroyed");
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
        }
        

    }
}
