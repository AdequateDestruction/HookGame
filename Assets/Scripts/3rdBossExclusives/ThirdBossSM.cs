using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThirdBossSM : MonoBehaviour {

    public string currentState= "1stState";

    //Non-Public variables
    MiniVolcanoes thisVolcano;
    bool coroutineInProgress = false, idleAnimationActive, moveAnimationActive, moveCoroutineInProgress = false;
    bool playerRangeCheckInProg = false;
    SpriteRenderer lavaSpriteRenderer;
    float risingAlpha = 0, lavaTimer = 0, magmaBreathTimer = 0, lavaOverflowTimer; // all of the timer variables
    float trueAngle;
    ParticleSystem magmaParticle;
    PlayerMovement player;
    Pathfinding.AIPath aiPath;
    Pathfinding.AIDestinationSetter aiSetter;
    public static Animator animController;
    public static ThirdBossSM thirdBossRef;
    public int secondPhaseHP = 3, thirdPhaseHP = 4; //public so timer can check when boss dies on phase 4

    //Public variables, usually for balancing or for ease of use access to other objects
    public float randomLavaTime, maxInvoke = 20, minInvoke = 10, lavaFadeSpeed = 0.05f, lavaRiseSpeed = 0.01f, lavaActiveDuration = 2.5f; //public lava variables
    public float mBreathRandom = 10, mBreathRandMax = 15, mBreathRandMin= 10, magmaBreathDuration = 5; // public magma breath variables
    public float lavaOverflowDuration = 3, playerProximityCheckTimer = 1.5f; //Lavaoverflow duration and random other variables
    public GameObject MiniVolcParent, LavaOverflowParticleSys, ProximityTrigger;
    public GameObject secondPhaseAmmunition, lava;
    public bool lavaActive = false, magmaBreathActive = false, doOnce = false, lavaOverflowActive, playerDead = false;
    public GameObject debugText, bossSpriteChild;
    public Sprite walkingSprite, idleSprite;
    public GameObject walkingCollision, idleCollision;

    // Use this for initialization
    void Start () {
        thisVolcano = gameObject.GetComponent<MiniVolcanoes>();
        lavaSpriteRenderer = lava.GetComponent<SpriteRenderer>();
        randomLavaTime = Random.Range(minInvoke, maxInvoke);
        magmaParticle = gameObject.GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        aiPath = gameObject.GetComponent<Pathfinding.AIPath>();
        aiSetter = gameObject.GetComponent<Pathfinding.AIDestinationSetter>();
        thirdBossRef = gameObject.GetComponent<ThirdBossSM>();
        animController = gameObject.GetComponentInChildren<Animator>();
        //animController.Play()
    }

    //To ensure different framerates don't affect the boss
    private void FixedUpdate()
    {

        if(currentState == "2ndState" || currentState == "3rdState") //do when 2nd or 3rd state is in progress
        {

            if (coroutineInProgress == false) // Allow LavaHazard() to tick when no-other coroutines are in progress
            {
                LavaHazard();
            }

            if(coroutineInProgress == false && lavaActive == true)//counts time for the lava to remain active for a set duration, specified in Lavahazard()
            {
                lavaTimer += Time.deltaTime;
            }

            lavaOverflowTimer += Time.deltaTime;//LavaOverflow timer incrementing

            if(lavaOverflowTimer > 50)//once timer hits 50, do lavaoverflow, reset if boss takes dmg in phases 2 or 3
            {
                lavaOverflowActive = true;

                if(currentState == "3rdState")//move to the middle with A* pathing, then do the overflow in third phase
                {
                    aiSetter.target = LavaOverflowParticleSys.transform;//traverse to LavaOverflowParticleSystem, this object should always be kept at 0,0,0
                    aiPath.canSearch = true;
                    aiPath.canMove = true;

                    if(transform.position.x <= LavaOverflowParticleSys.transform.position.x + 1 && transform.position.x >= LavaOverflowParticleSys.transform.position.x - 1)
                    {
                        if (transform.position.y <= LavaOverflowParticleSys.transform.position.y + 1 && transform.position.y >= LavaOverflowParticleSys.transform.position.y - 1)
                        {
                            print("keskellä");
                            transform.position = LavaOverflowParticleSys.transform.position;
                            aiPath.canSearch = false;
                            aiPath.canMove = false;
                            StartCoroutine(LavaOverflow());
                        }

                    }
                }
                
                if(currentState != "3rdState") // 2nd state overflow is called in SecondPhaseTakeDMG()
                {
                    StartCoroutine(LavaOverflow());
                }
                
            }
        }
        
        if(currentState == "3rdState" && magmaBreathActive) // Magmabreath timer incrementing
        {
            magmaBreathTimer += Time.deltaTime;
        }

        if(magmaBreathActive) //if magmabreath is active, this is used to track the player with it
        {
            Vector2 dir = magmaParticle.transform.position - player.transform.position;
            magmaParticle.transform.forward = (dir * -1);
        }

        if (magmaBreathTimer > mBreathRandom) //random magma breath duration, once timer is over the random duration end it, and randomise a new duration
        {
            print("magma reset");
            doOnce = true;
            magmaBreathTimer = 0;
            mBreathRandom = Random.Range(mBreathRandMin, mBreathRandMax);
        }

        if (doOnce)//stop the MagmaBreath
        {
            doOnce = false;
            magmaBreathActive = false;
            magmaParticle.Stop();
        }
        
        if(currentState == "3rdState" && idleAnimationActive
            && bossSpriteChild.GetComponent<SpriteRenderer>().sprite != idleSprite) //change to idle animation once idleAnimationActive is set to true
        {
            /*idleCollision.SetActive(true);
            walkingCollision.SetActive(false);*/
            //bossSpriteChild.GetComponent<SpriteRenderer>().sprite = idleSprite;
            //animController.StartPlayback();
            animController.SetBool("Moving", false);
        }

        if(currentState == "3rdState" && moveAnimationActive
            && bossSpriteChild.GetComponent<SpriteRenderer>().sprite != walkingSprite) //Change to walking animation once walkingAnimationActive is true
        {
            /*walkingCollision.SetActive(true);
            idleCollision.SetActive(false);*/
            //bossSpriteChild.GetComponent<SpriteRenderer>().sprite = walkingSprite;
            animController.SetBool("Moving", true);
        }
        
        if(playerRangeCheckInProg == true && currentState == "3rdState")//if player range check is in progress tick the function
        {
            CheckPlayerProximity();
        }

        if(player.currentHealth <= 0)
        {
            print("Player dead");
            currentState = "PlayerDead";
            playerDead = true;
        }

        MovingAnimationVariants();
        BossVsPlayerPos();

        if(Input.GetKeyDown(KeyCode.H))//Cheat for easier testing, DISABLE ON RELEASE
        {
            player.currentHealth = 40;
        }

        if(Input.GetKeyDown(KeyCode.K))//Cheat for easier testing, DISABLE ON RELEASE
        {
            player.currentHealth = 1;
        }
    }

    // Update is called once per frame
    void Update () {

        switch (currentState)//Simple switch case "state machine"
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
            case "BossDeadState":
                BossDead();
            break;
            case "PlayerDead":

                break;
            default:
                break;
        }

    }

    /// <summary>
    /// When all minivolcanoes are dead move to 2ndState
    /// </summary>
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

    /// <summary>
    /// ienumerator that is reserved for some variable changes and the animation for the transition of 1st to 2nd phase
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// SecondState that runs in update, activates shooting for boss again as well as changes ammunition to ones that shatter, when secondPhaseHP is under 1, move to 3rdState
    /// </summary>
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

    /// <summary>
    /// Second state damage function, when damage is taken triggers LavaOverflow()
    /// </summary>
    public void SecondPhaseTakeDmg()
    {
        if(secondPhaseHP > 0)
        {
            secondPhaseHP = secondPhaseHP - 1;
            StartCoroutine(LavaOverflow());
        }
    }

    /// <summary>
    /// IEnumerator for LavaOverflow, triggered when taken damage at 2nd phase or when 50 seconds are up and dmg has not been taken in that time. Timer in FixedUpdate
    /// </summary>
    /// <returns></returns>
    IEnumerator LavaOverflow()
    {

        lavaOverflowTimer = 0;
        lavaOverflowActive = true;
        StopMoving();
        LavaOverflowParticleSys.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(lavaOverflowDuration);
        LavaOverflowParticleSys.GetComponent<ParticleSystem>().Stop();
        lavaOverflowActive = false;
        lavaOverflowTimer = 0;
    }

    /// <summary>
    /// LavaHazard is responsible for the lava cycles in phases 2 and 3, runs LavaDissapate() automatically after. coroutineInProgress is used to initiate the lava cooldown
    /// </summary>
    void LavaHazard()
    {
        print("lava Hazard");
        if(risingAlpha < 0.93f)//when lavasprites' alpha is under this value increment it over time
        {
            risingAlpha = risingAlpha + lavaRiseSpeed;
            lavaSpriteRenderer.color = new Color(lavaSpriteRenderer.color.r, lavaSpriteRenderer.color.g, lavaSpriteRenderer.color.b, risingAlpha);
        }

        if(risingAlpha > 0.93f)//when lavasprites' alpha is over this value, set it to max alpha and make it deal damage. lavaActive is checked in LavaDMG script
        {
            risingAlpha = 1;
            lavaSpriteRenderer.color = new Color(lavaSpriteRenderer.color.r, lavaSpriteRenderer.color.g, lavaSpriteRenderer.color.b, risingAlpha);
            lavaActive = true;


        }
        if (lavaTimer > lavaActiveDuration) //Active Lava duration, once this is over, deactivate lava and Start LavaDissapate()
        {
            lavaActive = false;
            StartCoroutine("LavaDissipate");
        }  
    }

    /// <summary>
    /// Is responsible for fading the lava away and the lava cooldown. Run automatically after LavaHazard() concludes
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// ThirdState tick function, responsible for detecting when boss dies and stops the boss shooting and initiating A* movement.
    /// </summary>
    void ThirdState()
    {
        if(thirdPhaseHP <= 0)
        {
            debugText.GetComponent<Text>().gameObject.SetActive(true);
            print("Boss dies");
            currentState = "BossDeadState";
        }
        thisVolcano.stopShooting = true;
        print("third state");

        if(moveCoroutineInProgress == false)
        {
           StartCoroutine(MoveSeconds(4));
        }

    }

    /// <summary>
    /// The Legendary Magma Breath is not a force to be trifled with. Is used by the boss to fry the player to a crisp in phase 3. Boss' best power as well as it's biggest downfall.
    /// </summary>
    void MagmaBreath()
    {
        //do an animation here to warn the player first
            magmaBreathActive = true;

 
            Vector2 dir = magmaParticle.transform.position - player.transform.position;
            magmaParticle.transform.forward = (dir * -1);
            
            magmaParticle.Play();
            StopMoving();
        
    }
    
    /// <summary>
    /// Use this to deal damage to the boss in 3rd phase, should only be used by the pillars that magmabreath melts, however.
    /// </summary>
    public void ThirdPhaseTakeDMG()
    {
        thirdPhaseHP = thirdPhaseHP - 1;
        if(lavaOverflowActive == false)
        {
            lavaOverflowTimer = 0;
        }
    }

    /// <summary>
    /// Quite literally responsible for the whole flow of the 3rd phase, Moves the boss for x seconds, then stops, checks if the player is in range, if player is then fires the magma breath
    /// </summary>
    /// <param name="seconds">the duration that the boss should move</param>
    /// <returns></returns>
    IEnumerator MoveSeconds(int seconds)
    {
        if (lavaOverflowActive == false && magmaBreathActive == false)//do not allow moving when lavaoverflow or magmabreath is active
        {
            moveCoroutineInProgress = true;
            Move();
            yield return new WaitForSeconds(seconds);
            StopMoving();
            playerRangeCheckInProg = true;
            yield return new WaitForSeconds(playerProximityCheckTimer);
            playerRangeCheckInProg = false;
        } 
        yield return moveCoroutineInProgress = false;
        
    }

    /// <summary>
    /// Called only by MoveSeconds() Sets the target to player and uses A* to move towards player for x amount determined in MoveSeconds()
    /// </summary>
    private void Move()
    {
        if(lavaOverflowActive == false && magmaBreathActive == false)
        {
            if(aiSetter.target != player.transform)
            aiSetter.target = player.transform;

            print("move");
            idleAnimationActive = false;
            moveAnimationActive = true;

            aiPath.canSearch = true;
            aiPath.canMove = true;
        }

    }

    /// <summary>
    /// Stops boss' movement by disabling CanMove and CanSearch in A* script
    /// </summary>
    void StopMoving()
    {
        animController.SetBool("MovingUp", false);
        animController.SetBool("MovingDown", false);
        animController.SetBool("MovingLeft", false);
        animController.SetBool("MovingRight", false);

        print("Stop move");
        aiPath.canMove = false;
        aiPath.canSearch = false;


        idleAnimationActive = true;
        moveAnimationActive = false;
    }

    /// <summary>
    /// Checks if the player is within the range of the notorious MagmaBreath()
    /// </summary>
    void CheckPlayerProximity()
    {
        if(ProximityTrigger.GetComponent<Proximity>().playerInRange == true && magmaBreathActive == false)
        {
            MagmaBreath();
        }
    }

    void BossDead()
    {
        magmaBreathTimer = 0;
        magmaBreathActive = false;
        magmaParticle.Stop();
        lavaOverflowTimer = 0;
        lavaOverflowActive = false;
        LavaOverflowParticleSys.GetComponent<ParticleSystem>().Stop();
        lavaActive = false;
        StartCoroutine(ChangeScene());
        //boss death animation here probably as well as some other stuff if neccessary


    }

    IEnumerator ChangeScene()
    {
        //Miron change scene functio tänne
        yield return new WaitForSeconds(4f);
        WorldSceneManager.LoadInteractiveMenu();
    }

    void MovingAnimationVariants()
    {
        /*
        if(moveAnimationActive)
        {
            trueAngle = transform.rotation.eulerAngles.z;
            Debug.Log(trueAngle);


            if (trueAngle > 30 && trueAngle < 120)
            {
                //up
                animController.SetBool("MovingDown", true);
                animController.SetBool("MovingLeft", false);
                animController.SetBool("MovingRight", false);//down
                animController.SetBool("MovingUp", false);
            }
            else if (trueAngle > 120 && trueAngle < 210)
            {
                //left
                animController.SetBool("MovingDown", false); // right
                animController.SetBool("MovingRight", true);
                animController.SetBool("MovingUp", false);
                animController.SetBool("MovingLeft", false);
            }
            else if (trueAngle > 210 && trueAngle < 300)
            {
                //down
                animController.SetBool("MovingLeft", true);//left
                animController.SetBool("MovingRight", false);
                animController.SetBool("MovingUp", false);
                animController.SetBool("MovingDown", false);
            }
            else if (trueAngle > 300 && trueAngle < 360 || trueAngle > 0 && trueAngle < 30)//up
            {
                //right
                animController.SetBool("MovingDown", false);
                animController.SetBool("MovingLeft", false);
                animController.SetBool("MovingUp", true);//up
                animController.SetBool("MovingRight", false);
            }
        }*/



        
        

    }


    void BossVsPlayerPos()
    {
        if(moveAnimationActive)
        {
            if((gameObject.transform.position.x + player.transform.position.x) > (gameObject.transform.position.y + player.transform.position.y))
            {
            
                if (gameObject.transform.position.x > player.transform.position.x)
                {
                    animController.SetBool("MovingDown", true);
                    animController.SetBool("MovingLeft", false);
                    animController.SetBool("MovingUp", false);
                    animController.SetBool("MovingRight", false);
                    print("Play down animation");
                }
                else if (gameObject.transform.position.x < player.transform.position.x)
                {
                    animController.SetBool("MovingDown", false);
                    animController.SetBool("MovingLeft", false);
                    animController.SetBool("MovingUp", false);
                    animController.SetBool("MovingRight", true);
                    print("Play right animation");
                }

            }
            else if((gameObject.transform.position.x + player.transform.position.x) < (gameObject.transform.position.y + player.transform.position.y))
            {
                if (gameObject.transform.position.y > player.transform.position.y)
                {
                    animController.SetBool("MovingDown", false);
                    animController.SetBool("MovingLeft", true);
                    animController.SetBool("MovingUp", false);
                    animController.SetBool("MovingRight", false);
                    print("Play left animation");
                }
                else if (gameObject.transform.position.y < player.transform.position.y)
                {
                    animController.SetBool("MovingDown", false);
                    animController.SetBool("MovingLeft", false);
                    animController.SetBool("MovingUp", true);
                    animController.SetBool("MovingRight", false);
                    print("Play up animation");
                }
            }

        }

    }

    /// <summary>
    /// Is used for the play again button on deathcanvas
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene("ThirdBoss");
    }


    /// <summary>
    /// Used for the quit button in deathcanvas, should be moved to load mainmenu once InteractiveMainMenu is done
    /// </summary>
    public void ToMenu()
    {
        WorldSceneManager.LoadInteractiveMenu();
    }
}
