using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniBossAI : MonoBehaviour {

    GameObject Parent;
    WaterBossAI waterBossScript;
    Animator animator;
    GameObject visual;
    


    public bool isDead;
    bool doOnce=false;
    float timer=0;
    [SerializeField]
    float electricityTime=5;

    void Start ()
    {
        animator = GetComponent<Animator>();
        Parent = transform.parent.gameObject;
        visual = transform.parent.GetChild(1).gameObject;
        waterBossScript= GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();

      

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
                waterBossScript.minibossKilled++;
                visual.SetActive(false);
                animator.SetBool("Dead", true);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0f;
                //disables pathfinding/stops moving
                if (Parent.GetComponent<Pathfinding.AIPath>().enabled)
                {
                    Parent.GetComponent<Pathfinding.AIPath>().enabled = !Parent.GetComponent<Pathfinding.AIPath>().enabled;
                }
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
    }

    private void OnDisable()
    {
        if (isDead)
        {//reset enemy for pooling
            isDead = false;
            doOnce = false;
            timer = 0;
            visual.SetActive(true);
            Parent.GetComponent<Pathfinding.AIPath>().enabled = !Parent.GetComponent<Pathfinding.AIPath>().enabled;
            this.transform.localPosition = Vector3.zero;
        }
    }
    public void Animation()
    {

    }
}
