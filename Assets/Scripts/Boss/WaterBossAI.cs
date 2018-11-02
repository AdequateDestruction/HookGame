using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBossAI : MonoBehaviour {

    public ParticleSystem particle;
    public float WhirlpoolRotSpeed;

    [HideInInspector]
    public Collider2D col;
    [HideInInspector]
    public Transform rayCastPos;
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
        col = GetComponent<Collider2D>();
        rayCastPos = transform.GetChild(1);
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(AI());
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, rayCastPos.position, Color.red, 25f);
        //if enough minibosses killed starts to attack player
        if (minibossKilled>=killsToTrigger&&!frenzyed)
        {
            sm.SetNextState("Moving");
            frenzyed = true;
        }
    }

    private void FixedUpdate()
    {
        //when raycast hits player when moving, changes state to dash
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayCastPos.position);
        if (SM.CurrentState=="Moving"&&hit.collider!=null&&hit.collider.gameObject.name=="Player")
        {
            SM.SetNextState("Dash");
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
