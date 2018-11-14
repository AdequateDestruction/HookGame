﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterBossAI : MonoBehaviour {

    WaterStageManager waterStagemanagerScript;
    public ParticleSystem particle;
    [SerializeField]
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
    [HideInInspector]
    public int minibossKilled;
    [HideInInspector]
    public bool playerHit = false, frenzyed = false;

    public float movementSpeed=1, dashSpeed=5,rotateSpeed = 1;

    [SerializeField]
    public int killsToTrigger=1;


    int invulFlashCounter;
    SpriteRenderer bossVisual;
    [SerializeField]
    Color defaultColor, damageColor;
    [HideInInspector]
    public bool invulnerable;
    public float invulnerableTimer;


    public GameObject inHale;
    public List<Transform> corners;
    public bool turned;

    StateMachine sm = new StateMachine();
    public StateMachine SM { get { return sm; } }

    private void Awake()
    {
        States();
        particle.Stop();
    }

    void Start()
    {
        particle= transform.GetChild(4).gameObject.GetComponent<ParticleSystem>();
        bossVisual = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        waterStagemanagerScript = GameObject.Find("WaterStageManager").GetComponent<WaterStageManager>();
        col = GetComponent<Collider2D>();
        rayCastEnd = transform.GetChild(1);
        rayCastStart = transform.GetChild(2);
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        corners = new List<Transform>();

        StartCoroutine(AI());

        //Find corners in phase 3
        if (SceneManager.GetActiveScene().name == waterStagemanagerScript.PHASE3)
        {
            for (int i = 0; i < GameObject.Find("RandomCorners").transform.childCount; i++)
            {
                corners.Add(GameObject.Find("RandomCorners").transform.GetChild(i));
            }
            sm.SetNextState("ToCorner");
        }
    }

    private void Update()
    {
        //Debug.DrawRay(rayCastStart.position, rayCastEnd.position- rayCastStart.position, Color.red, 1f);

        //if enough minibosses killed starts to attack player
        if (SceneManager.GetActiveScene().name == waterStagemanagerScript.PHASE2&&minibossKilled >= killsToTrigger&&!frenzyed)
        {
            sm.SetNextState("Moving");
            frenzyed = true;
        }

        //visual when taking damage
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
        //when raycast hits player when moving, changes state
        RaycastHit2D hit = Physics2D.Raycast(rayCastStart.position, rayCastEnd.position - rayCastStart.position);
        if (SM.CurrentState=="Moving"&&hit.collider!=null&&hit.collider.gameObject.name=="Player")
        {
            if (SceneManager.GetActiveScene().name==waterStagemanagerScript.PHASE2)
            {
                SM.SetNextState("Breath");
                playerHit = true;
            }
            else if (SceneManager.GetActiveScene().name == waterStagemanagerScript.PHASE3)
            {
                SM.SetNextState("InHale");
            }

        }

        if (SM.CurrentState== "ToCorner"&& hit.collider != null && hit.collider.gameObject.name == "Center")
        {
            turned = true;
            SM.SetNextState("InHale");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when waterboss hits wall when not in moving/tocorner state
        if (collision.gameObject.tag == "StaticBlock"&&sm.CurrentState!="Moving"|| sm.CurrentState != "ToCorner")
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void States()
    {
        //states
        sm.AddState(new WaterIdle("Idle", this));
        sm.AddState(new WaterWaiting("Waiting", this));
        sm.AddState(new WaterDash("Dash", this));
        sm.AddState(new WaterTurbine("Turbine", this));
        sm.AddState(new WaterMoving("Moving", this));
        sm.AddState(new WaterBreath("Breath", this));
        sm.AddState(new WaterWhirlpool("Whirlpool", this));
        sm.AddState(new WaterInHale("InHale", this));
        sm.AddState(new WaterToCorner("ToCorner", this));
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
}