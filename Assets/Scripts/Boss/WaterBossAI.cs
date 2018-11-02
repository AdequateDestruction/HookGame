using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBossAI : MonoBehaviour {

    public ParticleSystem particle;
    public float WhirlpoolRotSpeed;

    [HideInInspector]
    public Collider2D col;
    [HideInInspector]
    public Transform rayCastEnd;
    [HideInInspector]
    public Transform rayCastStart;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Rigidbody2D rb;

    public float movementSpeed=1, dashSpeed=5,rotateSpeed = 1;


    public int killsToTrigger=1;
    [HideInInspector]
    public int minibossKilled;
    [HideInInspector]
    public bool playerHit=false, frenzyed = false;



    public int invulFlashCounter;
    public SpriteRenderer bossVisual;
    public Color defaultColor, damageColor;
    public bool invulnerable;
    public float invulnerableTimer;


    StateMachine sm = new StateMachine();
    public StateMachine SM { get { return sm; } }

    private void Awake()
    {
        //states
        sm.AddState(new WaterIdle("Idle", this));
        sm.AddState(new WaterWaiting("Waiting", this));
        sm.AddState(new WaterDash("Dash", this));
        sm.AddState(new WaterTurbine("Turbine", this));
        sm.AddState(new WaterMoving("Moving", this));
        sm.AddState(new WaterBreath("Breath", this));
        sm.AddState(new WaterWhirlpool("Whirlpool", this));


        particle.Stop();
    }

    void Start()
    {
        //bossVisual = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        col = GetComponent<Collider2D>();
        rayCastEnd = transform.GetChild(1);
        rayCastStart = transform.GetChild(2);
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(AI());
    }

    private void Update()
    {
        //Debug.DrawRay(rayCastStart.position, rayCastEnd.position- rayCastStart.position, Color.red, 1f);
        //if enough minibosses killed starts to attack player
        if (minibossKilled>=killsToTrigger&&!frenzyed)
        {
            sm.SetNextState("Moving");
            frenzyed = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            bossVisual.color = damageColor;
        }

        if (invulnerable)
        {
            if (invulFlashCounter % 10 == 0 || invulFlashCounter % 10 == 1)
            {
                bossVisual.color = damageColor;
            }
            else
            {
                bossVisual.color = defaultColor;
            }
            invulFlashCounter++;

            if (Time.time >= invulnerableTimer)
            {
                invulnerable = false;
                bossVisual.color = defaultColor;
            }
        }
    }

    private void FixedUpdate()
    {
        //when raycast hits player when moving, changes state to dash
        RaycastHit2D hit = Physics2D.Raycast(rayCastStart.position, rayCastEnd.position - rayCastStart.position);
        if (SM.CurrentState=="Moving"&&hit.collider!=null&&hit.collider.gameObject.name=="Player")
        {
            SM.SetNextState("Breath");
            playerHit = true;
        }
    }

    //"threading"
    IEnumerator AI()
    {
        while (true)
        {
            sm.Update();
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when waterboss hits wall when not in moving state
        if (collision.gameObject.tag == "StaticBlock"&&sm.CurrentState!="Moving")
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            SM.SetNextState("Idle");
        }
    }
}
