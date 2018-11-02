using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class miniBossAI : MonoBehaviour {

    GameObject Parent;
    WaterBossAI waterBossScript;
    Animator animator;
    GameObject visual;

    WaterStageManager waterStageManagerScript;

    bool phase2;


    public bool isDead;
    bool doOnce=false;
    float timer=0;
    [SerializeField]
    float electricityTime=5;

    void Start ()
    {
        waterStageManagerScript = GameObject.Find("WaterStageManager").GetComponent<WaterStageManager>();
        animator = GetComponent<Animator>();
        Parent = transform.parent.gameObject;
        visual = transform.parent.GetChild(1).gameObject;
        if (SceneManager.GetActiveScene().name==waterStageManagerScript.PHASE2)
        {
            waterBossScript= GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
            phase2 = true;
        }
    }
    private void OnEnable()
    {
        this.transform.localPosition = Vector3.zero;
    }
    void Update ()
    {
        //electricity on corpse after death
        if (isDead)
        {
            if (!doOnce)
            {
                if (phase2)
                {
                    waterBossScript.minibossKilled++;
                    animator.SetBool("Dead", true);
                }
                visual.SetActive(false);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0f;

                //disables pathfinding/stops moving
                Parent.GetComponent<Pathfinding.AIPath>().canMove = false;
                //if (Parent.GetComponent<Pathfinding.AIPath>().enabled)
                //{
                //    Parent.GetComponent<Pathfinding.AIPath>().enabled = !Parent.GetComponent<Pathfinding.AIPath>().enabled;
                //}
                doOnce = true;
            }

            timer += Time.deltaTime;
            if (timer > electricityTime)
            {
                Parent.SetActive(false);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "StaticBlock")
        {
            GetComponent<ChildposToParent>().enabled = !GetComponent<ChildposToParent>().enabled;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0f;
            this.transform.localPosition = Vector3.zero;
            isDead = true;
        }
        if (collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage();
        }
    }

    private void OnDisable()
    {
        if (isDead)
        {//reset enemy for pooling
            isDead = false;
            doOnce = false;
            timer = 0;
            visual.SetActive(true);
            Parent.GetComponent<Pathfinding.AIPath>().canMove = true;

            //Parent.GetComponent<Pathfinding.AIPath>().enabled = !Parent.GetComponent<Pathfinding.AIPath>().enabled;
            this.transform.localPosition = Vector3.zero;
        }
    }
    public void Animation()
    {

    }
}
