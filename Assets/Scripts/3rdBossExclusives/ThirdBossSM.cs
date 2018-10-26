using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossSM : MonoBehaviour {

    public string currentState= "1stState";
    MiniVolcanoes thisVolcano;
    bool coroutineInProgress = false;
    SpriteRenderer spriteRenderer;
    float risingAlpha = 0, timer = 0;

    public float randomLavaTime, maxInvoke = 20, minInvoke = 10;
    public int secondPhaseHP = 3;
    public GameObject MiniVolcParent;
    public GameObject secondPhaseAmmunition, lava;
    public bool lavaActive = false;

	// Use this for initialization
	void Start () {
        thisVolcano = gameObject.GetComponent<MiniVolcanoes>();
        spriteRenderer = lava.GetComponent<SpriteRenderer>();
        randomLavaTime = Random.Range(minInvoke, maxInvoke);
    }

    private void FixedUpdate()
    {
        if(coroutineInProgress == false && currentState == "2ndState" || currentState =="3rdState" && risingAlpha < 0.93f)
        {
            print("if lause");
            //InvokeRepeating(, 0.5f, randomLavaTime);
            LavaHazard();
            //invokeInProgress = true;
        }

        if(currentState == "2ndState" || currentState == "3rdState" && coroutineInProgress == false && lavaActive == true)
        {
            timer += Time.deltaTime;
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
            print("3rd state");
            currentState = "3rdState";
        }
    }

    public void SecondPhaseTakeDmg()
    {
        secondPhaseHP = secondPhaseHP - 1;
    }

    void LavaHazard()
    {
        print("Flame on!");
        if(risingAlpha < 0.93f)
        {
            risingAlpha = risingAlpha + 0.01f;//replace 0.3 with alpha multiplier variable
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
        }

        if(risingAlpha > 0.93f && coroutineInProgress == false)
        {
            print("flame in full force");
            //CancelInvoke("LavaHazard");
            lavaActive = true;

            if(timer >3 && coroutineInProgress == false)
            {
                lavaActive = false;
                StartCoroutine("LavaDissipate");
                print("Start lava dissapate");
            }

        }

        
    }

    IEnumerator LavaDissipate()
    {
        coroutineInProgress = true;
        print("lava dissapate");
        while (risingAlpha >= 0.10f)
        {
            risingAlpha = risingAlpha - 0.5f;//replace 0.5 with alpha minus variable
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
        }

        if (risingAlpha <= 0.10f)
        {
            print("lava dissapate second stage");
            risingAlpha = 0;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, risingAlpha);
            randomLavaTime = Random.Range(minInvoke, maxInvoke);
            //StopCoroutine("LavaDissipate");
            print("wait " +randomLavaTime);
            new WaitForSecondsRealtime(1000);
            print("wait done");
            timer = 0;
            coroutineInProgress = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
