using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowForProjectiles : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    float risingAlpha = 0;
    //public for customization
    public float invokeStart = 0.1f;
    public float invokeRepeat = 0.05f;
    public float alphaMultiplier = 0.01f;
    public bool big = false;
    public bool shatter = false;
    public GameObject rockCluster;
    public GameObject fallingRockSprite;
    
    float yMulti = 5.5f;
    bool doOnce = true;
    GameObject rockObject;
    PlayerMovement playerScript;
    public bool insideShadow = false;

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
	void FixedUpdate () {

        if(risingAlpha >= 0.85f)
        {



            if(insideShadow)
            {
                playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                playerScript.TakeDamage();
            }


        }
        
    }

    void AlphaChange()
    {
        if(risingAlpha <= 1)
        risingAlpha = risingAlpha + alphaMultiplier;

        if (risingAlpha >= 0.80f)
        {
            if(big && !shatter)
            {
                yMulti = 6f;
            }

            if(shatter)
            {
                yMulti = 3.5f;
            }

            if(doOnce == true)
            {
                rockObject = Instantiate(fallingRockSprite, new Vector2(transform.position.x, transform.position.y + yMulti), transform.rotation);
                rockObject.GetComponent<RockFall>().ShadowsPos(gameObject.transform.position);
            }

            if(doOnce)
            {
                doOnce = false;

                if(big)
                {
                    rockObject.transform.localScale = rockObject.transform.localScale * 3;
                }
            }


            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);



            if(risingAlpha >= 0.86f)
            {

                risingAlpha = 1;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
                CancelInvoke();

                if (shatter)
                {
                    Instantiate(rockCluster, transform.position, transform.rotation);
                }
                Destroy(gameObject);
            }

        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Player")
        {
            insideShadow = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            insideShadow = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            insideShadow = false;
        }
        
    }
}
