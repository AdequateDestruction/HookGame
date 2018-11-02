using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossSM : MonoBehaviour {

    public string currentState= "1stState";
    MiniVolcanoes thisVolcano;
    public bool coroutineInProgress = false, idleAnimationActive, moveAnimationActive, moveCoroutineInProgress = false;
    SpriteRenderer lavaSpriteRenderer;
    float risingAlpha = 0, lavaTimer = 0, magmaBreathTimer = 0, lavaOverflowTimer;
    ParticleSystem magmaParticle;
    PlayerMovement player;
    Pathfinding.AIPath aiPath;
    Pathfinding.AIDestinationSetter aiSetter;

    public float randomLavaTime, maxInvoke = 20, minInvoke = 10;
    public int secondPhaseHP = 3, thirdPhaseHP = 4, mBreathRandom = 10, mBreathRandMax = 20, mBreathRandMin= 10;
    public GameObject MiniVolcParent, LavaOverflowParticleSys, ProximityTrigger;
    public GameObject secondPhaseAmmunition, lava;
    public bool lavaActive = false, magmaBreathActive = false, doOnce = true, lavaOverflowActive;
    public float lavaFadeSpeed = 0.05f, lavaRiseSpeed = 0.01f;


    // Use this for initialization
    void Start () {
        thisVolcano = gameObject.GetComponent<MiniVolcanoes>();
        lavaSpriteRenderer = lava.GetComponent<SpriteRenderer>();
        randomLavaTime = Random.Range(minInvoke, maxInvoke);
        magmaParticle = gameObject.GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        aiPath = gameObject.GetComponent<Pathfinding.AIPath>();
        aiSetter = gameObject.GetComponent<Pathfinding.AIDestinationSetter>();
    }

    private void FixedUpdate()
    {
        if(currentState == "2ndState" || currentState =="3rdState")
        {        
            if(coroutineInProgress == false)
            {
                LavaHazard();
            }
        }

        if(currentState == "2ndState" || currentState == "3rdState")
        {
            if(coroutineInProgress == false && lavaActive == true)
            {
                lavaTimer += Time.deltaTime;
            }

            lavaOverflowTimer += Time.deltaTime;

            if(lavaOverflowTimer > 50)
            {
                StartCoroutine(LavaOverflow());
            }
        }

        if(currentState == "3rdState")
        {
            magmaBreathTimer += Time.deltaTime;
        }

        if(magmaBreathActive)
        {
            Vector2 dir = magmaParticle.transform.position - player.transform.position;
            magmaParticle.transform.forward = (dir * -1);
        }
    }

    // Update is called once per frame
    void Update () {

        switch (currentState)
        {
            case "1stState":
                CheckVolcanoes();
                break;
            case "BustinOut":
                if(coroutineInProgress == false)
                {
                    print("Bustin out coroutine started");
                    StartCoroutine("BustinOut");
                }
                break;
            case "2ndState":
                SecondState();
                break;
            case "3rdState":
                ThirdState();
                break;
            default:
                break;
        }

    }

    void CheckVolcanoes()
    {
        if(MiniVolcParent.GetComponentInChildren<MiniVolcanoes>() == null)//Kaikki mini volcanot on tuhottu
        {
            print("all mini volcanoes dead");
            //voi tuhota sen isomman volcanon
            thisVolcano.stopShooting = true;
            currentState = "BustinOut";
        }
    }

    IEnumerator BustinOut() //Boss busts out of the volcano
    {
        coroutineInProgress = true;
        print("BustinOut");
        yield return new WaitForSeconds(4);
        print("waited 4 seconds");
        currentState = "2ndState";
        StopCoroutine("BustinOut");
        coroutineInProgress = false;
        
        
    }

    void SecondState()
    {
        if(thisVolcano.projectileShadow != secondPhaseAmmunition)
        {
            thisVolcano.projectileShadow = secondPhaseAmmunition;
        }

        thisVolcano.stopShooting = false;

        if(secondPhaseHP < 1)
        {
            currentState = "3rdState";
        }
    }

    public void SecondPhaseTakeDmg()
    {
        secondPhaseHP = secondPhaseHP - 1;
        StartCoroutine(LavaOverflow());
    }

    IEnumerator LavaOverflow()
    {

        if(currentState == "3rdState")
        {
            /*
            while(aiPath.reachedEndOfPath == false)
            {
                aiSetter.target = LavaOverflowParticleSys.transform;
                aiPath.canSearch = true;
                aiPath.canMove = true;

            }*/
            //move to the middle first then do the overflow

        }

        lavaOverflowTimer = 0;
        lavaOverflowActive = true;
        StopMoving();
        LavaOverflowParticleSys.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(3);
        LavaOverflowParticleSys.GetComponent<ParticleSystem>().Stop();
        lavaOverflowActive = false;
        lavaOverflowTimer = 0;
    }

    void LavaHazard()
    {
        print("lava Hazard");
        if(risingAlpha < 0.93f)
        {
            risingAlpha = risingAlpha + lavaRiseSpeed;
            lavaSpriteRenderer.color = new Color(lavaSpriteRenderer.color.r, lavaSpriteRenderer.color.g, lavaSpriteRenderer.color.b, risingAlpha);
        }

        if(risingAlpha > 0.93f)
        {
            risingAlpha = 1;
            lavaSpriteRenderer.color = new Color(lavaSpriteRenderer.color.r, lavaSpriteRenderer.color.g, lavaSpriteRenderer.color.b, risingAlpha);
            lavaActive = true;


        }
        if (lavaTimer >3)
        {
            lavaActive = false;
            StartCoroutine("LavaDissipate");
        }  
    }

    IEnumerator LavaDissipate()
    {
        coroutineInProgress = true;
        print("lava dissapate");
        while (risingAlpha >= 0.10f)
        {
            risingAlpha = risingAlpha - lavaFadeSpeed;
            lavaSpriteRenderer.color = new Color(lavaSpriteRenderer.color.r, lavaSpriteRenderer.color.g, lavaSpriteRenderer.color.b, risingAlpha);
        }

        if (risingAlpha <= 0.10f)
        {
            risingAlpha = 0;
            lavaSpriteRenderer.color = new Color(lavaSpriteRenderer.color.r, lavaSpriteRenderer.color.g, lavaSpriteRenderer.color.b, risingAlpha);
            randomLavaTime = Random.Range(minInvoke, maxInvoke);
            yield return new WaitForSeconds(randomLavaTime);
            lavaTimer = 0;
            coroutineInProgress = false;
        }
    }

    void ThirdState()
    {
        if(thirdPhaseHP <= 0)
        {
            print("Boss dies");
        }
        thisVolcano.stopShooting = true;
        print("third state");

        if(moveCoroutineInProgress == false)
        {
           StartCoroutine(MoveSeconds(4));
        }

    }

    void MagmaBreath()
    {
        //do an animation here to warn the player first
        if(magmaBreathTimer < 7)
        {
            magmaBreathActive = true;
            magmaParticle.Play();
            StopMoving();


        }
        else if(doOnce)//set the random magma breath duration variable only once per cycle for the next cycle
        {
            doOnce = false;
            mBreathRandom = Random.Range(mBreathRandMin, mBreathRandMax);
            magmaBreathActive = false;
            magmaParticle.Stop();
        }

        if(magmaBreathTimer > mBreathRandom) //random magma breath duration
        {
            print("magma reset");
            doOnce = true;
            magmaBreathTimer = 0;
            magmaBreathActive = true;
        }
    }
    
    public void ThirdPhaseTakeDMG()
    {
        thirdPhaseHP = thirdPhaseHP - 1;
    }

    IEnumerator MoveSeconds(int seconds)
    {
        //if(lavaOverflowActive == false && magmaBreathActive == false)
        
            moveCoroutineInProgress = true;
            Move();
            yield return new WaitForSeconds(seconds);
            StopMoving();
            CheckPlayerProximity();
        
        yield return moveCoroutineInProgress = false;
        
    }

    private void Move()
    {
        if(lavaOverflowActive == false && magmaBreathActive == false)
        {
            print("move");
            idleAnimationActive = false;
            moveAnimationActive = true;

            aiPath.canSearch = true;
            aiPath.canMove = true;
        }

    }

    void StopMoving()
    {
        print("Stop move");
        aiPath.canMove = false;
        aiPath.canSearch = false;


        idleAnimationActive = true;
        moveAnimationActive = false;
    }

    void CheckPlayerProximity()
    {
        if(ProximityTrigger.GetComponent<Proximity>().playerInRange == true)
        {
            MagmaBreath();
        }

    }
}
