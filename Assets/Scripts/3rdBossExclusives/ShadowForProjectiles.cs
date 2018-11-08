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
    
    float yMulti = 3f;
    bool doOnce = true;
    GameObject rockObject;
    PlayerMovement playerScript;
    bool insideShadow = false;

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

        if(insideShadow)
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            playerScript.TakeDamage();
        }
        
    }

    void AlphaChange()
    {
        if(risingAlpha <= 1)
        risingAlpha = risingAlpha + alphaMultiplier;

        if (risingAlpha >= 0.85f)
        {
            if(big)
            {
                yMulti = 4;
            }

            if(doOnce == true)
            {
                rockObject = Instantiate(fallingRockSprite, new Vector2(transform.position.x, transform.position.y + yMulti), transform.rotation);
            }

            if(big && doOnce)
            {
                rockObject.transform.localScale = rockObject.transform.localScale * 3;
                doOnce = false;
            }


            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);

            if (shatter)
            {
                Instantiate(rockCluster,transform.position,transform.rotation);
            }

            if(risingAlpha >= 0.90f)
            {
                risingAlpha = 1;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
                CancelInvoke();
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
        if(risingAlpha >= 0.85f) //Does not work if put in the same if with && for some reason
        {
            if(collision.tag == "Player")
            {
                insideShadow = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (risingAlpha >= 0.85f) //Does not work if put in the same if with && for some reason
        {
            if (collision.tag == "Player")
            {
                insideShadow = true;
            }
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
