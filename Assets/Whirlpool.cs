using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlpool : MonoBehaviour {

    float tempPlayerSpeed, timer, repeateTime, explosionTimer;
    [SerializeField]
    float suckSpeed;

    PlayerMovement playerMovementScript;
    WaterBossAI waterBossAIScript;
    WaterStageManager waterStagemanagerScript;
    SpriteRenderer spriteRenderer;
    ParticleSystem explosion;
    Collider2D col;

    [SerializeField]
    float repeateTimeMax=5, repeateTimeMin=1;

    bool somethingInWhirlpool, exploded;
    [SerializeField]
    float explosionTime=3, invulnerableCD;

    // Use this for initialization
    void Start ()
    {
        tempPlayerSpeed = GameObject.Find("Player").GetComponent<PlayerMovement>().moveSpeed;
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        waterBossAIScript = GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
        waterStagemanagerScript = GameObject.Find("WaterStageManager").GetComponent<WaterStageManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        explosion = GetComponent<ParticleSystem>();

        playerMovementScript.notTeleported = true;
        repeateTime = Random.Range(repeateTimeMin, repeateTimeMax);
    }

    void Update ()
    {
        //Random whirlpool activation
        timer += Time.deltaTime;
        if (timer>repeateTime&&!somethingInWhirlpool)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            col.enabled = !col.enabled;
            timer = 0;
        }

        //explosion activation
        if (explosionTimer>explosionTime&&somethingInWhirlpool)
        {
            explosion.Play();
            exploded = true;
            waterBossAIScript.invulnerableTimer = Time.time + invulnerableCD;
            waterBossAIScript.invulnerable = true;

        }
        else if (somethingInWhirlpool)
        {
            explosionTimer += Time.deltaTime;
        }
        if (exploded)
        {
            explosionTimer += Time.deltaTime;

        }

        if (explosionTimer>explosionTime*2&&exploded)
        {
            waterBossAIScript.SM.SetNextState("Idle");
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            collision.GetComponent<PlayerMovement>().moveSpeed = tempPlayerSpeed/2;


        }
        if (collision.tag=="MiniBoss")
        {
            collision.GetComponent<miniBossAI>().isDead = true;
        }

        if (collision.tag=="Boss")
        {
            Debug.Log("spotted");
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {           
            collision.GetComponent<PlayerMovement>().moveSpeed = tempPlayerSpeed;
        }
    }

    private void OnDisable()
    {
        waterStagemanagerScript.whirlpoolDestroyed++;
    }
}
