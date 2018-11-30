using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterBossAI : MonoBehaviour {

    public PlayerMovement playerMovementScript;
    public WaterStageManager waterStagemanagerScript;
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

    public float movementSpeed=1, rotateSpeed = 1 /*dashSpeed=5,*/;

    [SerializeField]
    int killsToTrigger=1;

    SpriteRenderer bossVisual;
    [SerializeField]
    Color defaultColor, damageColor;
    //[HideInInspector]

    //Inhale&goingintotherorner
    public GameObject inHale;
    public inhale inhaleScript;
    public List<Transform> corners;
    public Transform lastCorner;
    public bool turned;

    //invulnerable
    [SerializeField]
    float invulnerableCD;
    bool inPool, invulnerable;
    float invulnerableTimer;
    int invulFlashCounter;



    Animator waterBossAnimator;
    //rotation in 360 degrees
    float trueAngle;

    public List<Collider2D> polyCols;
    int tempLast=0;

    StateMachine sm = new StateMachine();
    public StateMachine SM { get { return sm; } }

    private void Awake()
    {
        States();
        particle.Stop();
    }

    void Start()
    {
        inHale = transform.GetChild(5).gameObject;
        inHale.gameObject.SetActive(false);
        particle= transform.GetChild(4).gameObject.GetComponent<ParticleSystem>();
        bossVisual = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        waterStagemanagerScript = GameObject.Find("WaterStageManager").GetComponent<WaterStageManager>();
        waterBossAnimator = transform.GetChild(0).GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rayCastEnd = transform.GetChild(1);
        rayCastStart = transform.GetChild(2);
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        corners = new List<Transform>();
        inhaleScript = inHale.GetComponent<inhale>();
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();

        StartCoroutine(AI());

            for (int i = 0; i < GameObject.Find("RandomCorners").transform.childCount; i++)
            {
                corners.Add(GameObject.Find("RandomCorners").transform.GetChild(i));
            }
    }
    
    private void Update()
    {
        //Debug.DrawRay(rayCastStart.position, rayCastEnd.position- rayCastStart.position, Color.red, 1f);
        //DEBUG
        if (Input.GetKey(KeyCode.U))
        {
            Debug.Log(Vector2.Distance(this.transform.position, corners[waterStagemanagerScript.randomIndex[waterStagemanagerScript.index]].transform.position));   
        }

        //if enough minibosses killed starts to attack player
        if (SceneManager.GetActiveScene().name == waterStagemanagerScript.PHASE2_3&&minibossKilled >= killsToTrigger&&!frenzyed)
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

        if (inPool)
        {
            invulnerableTimer = Time.time + invulnerableCD;
            invulnerable = true;
        }
    }

    private void FixedUpdate()
    {
        WaterBossAnimations();
        //when raycast hits player when moving, changes state
        RaycastHit2D hit = Physics2D.Raycast(rayCastStart.position, rayCastEnd.position - rayCastStart.position);
        if (SM.CurrentState=="Moving"&&hit.collider!=null&&hit.collider.gameObject.name=="Player")
        {
            if (waterStagemanagerScript.whirlpoolDestroyed<2)
            {
                SM.SetNextState("Breath");
                playerHit = true;         
            }
            else if (waterStagemanagerScript.whirlpoolDestroyed>=2)
            {
                SM.SetNextState("Breath");
            }
        }
        //uses inhale state always when hitting center. Checks if next corner is reached before using inhale again 
        if (Vector2.Distance(this.transform.position, corners[waterStagemanagerScript.randomIndex[waterStagemanagerScript.index]].transform.position)<1)
        {
            if (SM.CurrentState == "ToCorner" && hit.collider != null && hit.collider.gameObject.name == "Center")
            {
                turned = true;
                SM.SetNextState("InHale");
            }
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
        if (collision.gameObject.tag=="Player")
        {
            playerMovementScript.TakeDamage();
        }
        if (collision.gameObject.tag=="Whirlpool")
        {
            inPool = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Whirlpool")
        {
            inPool = false;
        }
    }

    void States()
    {
        //states
        sm.AddState(new WaterIdle("Idle", this));
        sm.AddState(new WaterWaiting("Waiting", this));
        //sm.AddState(new WaterDash("Dash", this));
        sm.AddState(new WaterTurbine("Turbine", this));
        sm.AddState(new WaterMoving("Moving", this));
        sm.AddState(new WaterBreath("Breath", this));
        sm.AddState(new WaterWhirlpool("Whirlpool", this));
        sm.AddState(new WaterInHale("InHale", this));
        sm.AddState(new WaterToCorner("ToCorner", this));
    }

    void WaterBossAnimations()
    {
        //calculates angle in 360 degrees and sets it in animator
        trueAngle = transform.rotation.eulerAngles.z;
        waterBossAnimator.SetFloat("Angle", trueAngle);

        //changes collider with current animator state name
        if (waterBossAnimator.GetCurrentAnimatorStateInfo(0).IsName("WaterBossMovementLeft"))
        {
            if (!polyCols[2].isActiveAndEnabled)
            {
                polyCols[tempLast].enabled = !polyCols[tempLast].enabled;
                polyCols[2].enabled = !polyCols[2].enabled;
                tempLast = 2;
            }
            
        }
        else if (waterBossAnimator.GetCurrentAnimatorStateInfo(0).IsName("WaterBossMovementDown"))
        {
            if (!polyCols[1].isActiveAndEnabled)
            {
                polyCols[tempLast].enabled = !polyCols[tempLast].enabled;
                polyCols[1].enabled = !polyCols[1].enabled;
                tempLast = 1;
            }
        }
        else if (waterBossAnimator.GetCurrentAnimatorStateInfo(0).IsName("WaterBossMovementRight"))
        {
            if (!polyCols[0].isActiveAndEnabled)
            {
                polyCols[tempLast].enabled = !polyCols[tempLast].enabled;
                polyCols[0].enabled = !polyCols[0].enabled;
                tempLast = 0;
            }
        }
        else if (waterBossAnimator.GetCurrentAnimatorStateInfo(0).IsName("WaterBossMovementUp"))
        {
            if (!polyCols[3].isActiveAndEnabled)
            {
                polyCols[tempLast].enabled = !polyCols[tempLast].enabled;
                polyCols[3].enabled = !polyCols[3].enabled;
                tempLast = 3;
            }
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
}
