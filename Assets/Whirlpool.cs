using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlpool : MonoBehaviour {

    float tempPlayerSpeed;
    public float suckSpeed;

    public PlayerMovement playerMovementScript;
    public WaterBossAI waterBossAIScript;

    public bool useSlow;
    public bool useTeleport;

    public bool teleported;
    public List<GameObject> pools;

    public float timer;
    public float repeateTime=3;
    public SpriteRenderer spriteRenderer;
    public Collider2D col;
    public ParticleSystem explosion;

    public bool somethingInWhirlpool, exploded;
    public float explosionTimer;
    public float explosionTime=3;
    public float invulnerableCD;

    // Use this for initialization
    void Start ()
    {
        tempPlayerSpeed = GameObject.Find("Player").GetComponent<PlayerMovement>().moveSpeed;
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        waterBossAIScript = GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        playerMovementScript.notTeleported = true;
        explosion = GetComponent<ParticleSystem>();

    }


    void Update ()
    {
        //Random activation
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
            if (useSlow)
            {
                collision.GetComponent<PlayerMovement>().moveSpeed = tempPlayerSpeed;

            }
        }
    }


}
