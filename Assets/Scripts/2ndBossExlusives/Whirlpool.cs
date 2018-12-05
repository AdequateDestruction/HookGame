using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlpool : MonoBehaviour {

    float tempPlayerSpeed, timer, repeateTime, explosionTimer;
    bool somethingInWhirlpool, exploded;

    PlayerMovement playerMovementScript;
    WaterBossAI waterBossAIScript;
    WaterStageManager waterStagemanagerScript;
    SpriteRenderer spriteRenderer;
    public GameObject explosion;
    Animator propellerAnim;
    Animator explosionAnim;
    Collider2D col;
    public bool active, destroyed;
    [SerializeField]
    float speedMax=1,speedMin=10;
    bool doOnce;
    

    [SerializeField]
    float suckSpeed, repeateTimeMax = 5, repeateTimeMin=1, explosionTime = 3;

    void Start ()
    {
        tempPlayerSpeed = GameObject.Find("Player").GetComponent<PlayerMovement>().moveSpeed;
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        waterBossAIScript = GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
        waterStagemanagerScript = GameObject.Find("WaterStageManager").GetComponent<WaterStageManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        explosion = transform.GetChild(1).gameObject;
        propellerAnim = transform.GetChild(0).GetComponent<Animator>();
        explosionAnim = transform.Find("UnderwaterExplosion").GetComponent<Animator>();

        explosion.SetActive(false);
        repeateTime = Random.Range(repeateTimeMin, repeateTimeMax);
        //destroy();
    }

    void Update ()
    {


        //Random whirlpool activation
        timer += Time.deltaTime;
        if (timer>repeateTime&&!somethingInWhirlpool)
        {
            if (!destroyed)
            {
                if (active)
                {
                    active = false;
                    propellerAnim.speed = speedMin;

                }
                else
                {
                    active = true;
                    propellerAnim.speed = speedMax;
                }
                col.enabled = !col.enabled;
                timer = 0;
            }
            //spriteRenderer.enabled = !spriteRenderer.enabled;
   
        }
        if (exploded)
        {
            speedMax = speedMax - 0.005f;
            propellerAnim.speed = speedMax;

        }

        //explosion activation
        if (explosionTimer>explosionTime&&somethingInWhirlpool)
        {
            if (!doOnce)
            {
                explosionAnim.SetBool("Exploded", true);
                //propellerAnim.speed = 0;
                //explosion.SetActive(true);
            }

            exploded = true;

        }
        else if (somethingInWhirlpool)
        {

            explosionTimer += Time.deltaTime;
        }
        if (exploded)
        {

            explosionTimer += Time.deltaTime;

        }
        if (explosionTimer>explosionTime*2&&exploded&&!doOnce)
        {
            destroy();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            collision.GetComponent<PlayerMovement>().moveSpeed = tempPlayerSpeed/2;
            playerMovementScript.TakeDamage();
        }
        if (collision.tag=="MiniBoss")
        {
            collision.GetComponent<miniBossAI>().isDead = true;
        }
        if (collision.tag=="Boss")
        {
            somethingInWhirlpool = true;
            waterBossAIScript.SM.SetNextState("Whirlpool");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            collision.transform.position = Vector2.MoveTowards(collision.transform.position, this.transform.position,suckSpeed*Time.deltaTime);
        }
        if (collision.tag=="Player")
        {
            collision.transform.position = Vector2.MoveTowards(collision.transform.position, this.transform.position, suckSpeed * Time.deltaTime);
            //Debug.Log(Vector2.Distance(collision.transform.position, this.transform.position));
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {           
            collision.GetComponent<PlayerMovement>().moveSpeed = tempPlayerSpeed;
        }
    }


    public void destroy()
    {

        destroyed = true;
        active = false;
        explosion.SetActive(false);
        if (col.enabled)
        {
            col.enabled = !col.enabled;

        }
        doOnce = true;
        propellerAnim.SetBool("Destroyed", true);
        waterBossAIScript.SM.SetNextState("Idle");
        waterStagemanagerScript.whirlpoolDestroyed++;
    }
}
