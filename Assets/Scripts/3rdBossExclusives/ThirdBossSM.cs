using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossSM : MonoBehaviour {

    public string currentState= "1stState";
    MiniVolcanoes thisVolcano;
    public bool coroutineInProgress = false;
    SpriteRenderer spriteRenderer;
    float risingAlpha = 0, lavaTimer = 0;
    ParticleSystem magmaParticle;
    PlayerMovement player;

    public float randomLavaTime, maxInvoke = 20, minInvoke = 10;
    public int secondPhaseHP = 3;
    public GameObject MiniVolcParent;
    public GameObject secondPhaseAmmunition, lava;
    public bool lavaActive = false;
    public float lavaFadeSpeed = 0.05f, lavaRiseSpeed = 0.01f;


    public float RotationSpeed = 5;
    private Quaternion _lookRotation;
    private Vector3 _direction;
    public Transform Target;
    // Use this for initialization
    void Start () {
        thisVolcano = gameObject.GetComponent<MiniVolcanoes>();
        spriteRenderer = lava.GetComponent<SpriteRenderer>();
        randomLavaTime = Random.Range(minInvoke, maxInvoke);
        magmaParticle = gameObject.GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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

        new WaitForSeconds(4f);
        currentState = "2ndState";
        StopCoroutine("BustinOut");
        coroutineInProgress = false;
        yield return new WaitForSeconds(0.1f);
        
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
    }

    void LavaHazard()
    {
        print("lava Hazard");
        if(risingAlpha < 0.93f)
        {
            risingAlpha = risingAlpha + lavaRiseSpeed;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
        }

        if(risingAlpha > 0.93f)
        {
            risingAlpha = 1;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
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
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
        }

        if (risingAlpha <= 0.10f)
        {
            risingAlpha = 0;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
            randomLavaTime = Random.Range(minInvoke, maxInvoke);
            yield return new WaitForSeconds(randomLavaTime);
            lavaTimer = 0;
            coroutineInProgress = false;
        }
    }

    void ThirdState()
    {
        thisVolcano.stopShooting = true;
        print("third state");
        MagmaBreath();
    }

    void MagmaBreath()
    {
        //do an animation here to warn the player first
        magmaParticle.Play();
        print(magmaParticle.transform.position + "magma pos and " + player.transform.position);
        //magmaParticle.transform.rota


        Vector2 dir = magmaParticle.transform.position - player.transform.position;
        magmaParticle.transform.right = dir;
    }
    
}
