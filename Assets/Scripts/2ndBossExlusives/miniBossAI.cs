using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class miniBossAI : MonoBehaviour {

    GameObject Parent;
    Animator animator;
    GameObject visual;
    WaterBossAI waterBossScript;
    WaterStageManager waterStageManagerScript;

    bool canDo, doOnce = false;
    float timer = 0;
    [SerializeField]
    float electricityTime = 5;

    public bool isDead;
    
    void Start ()
    {
        waterStageManagerScript = GameObject.Find("WaterStageManager").GetComponent<WaterStageManager>();
        animator = GetComponent<Animator>();
        Parent = transform.parent.gameObject;
        visual = transform.parent.GetChild(1).gameObject;

        //if phase is NOT phase1
        if (SceneManager.GetActiveScene().name!=waterStageManagerScript.PHASE1)
        {
            waterBossScript= GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
            canDo = true;
        }
 
    }
    private void OnEnable()
    {
        //when enabled change location back to spawner
        this.transform.localPosition = Vector3.zero;
    }
    void Update ()
    {
        //electricity on corpse after death
        if (isDead)
        {
            if (!doOnce)
            {
                if (canDo)
                {
                    waterBossScript.minibossKilled++;
                    animator.SetBool("Dead", true);
                }
                visual.SetActive(false);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0f;

                //disables pathfinding/stops moving
                Parent.GetComponent<Pathfinding.AIPath>().canMove = false;
                doOnce = true;
            }

            //how long electricity stays on corpse
            timer += Time.deltaTime;
            if (timer > electricityTime)
            {
                Parent.SetActive(false);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //kills miniboss when collided with StaticBlock
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
        {
            //reset enemy for pooling
            isDead = false;
            doOnce = false;
            timer = 0;
            visual.SetActive(true);
            Parent.GetComponent<Pathfinding.AIPath>().canMove = true;
            this.transform.localPosition = Vector3.zero;
        }
    }

    public void Animation()
    {

    }
}
