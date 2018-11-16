using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterStageManager : MonoBehaviour {
    public string PHASE1, PHASE2_3, PHASE3;

    PlayerMovement playerMovementScript;
    WorldSceneManager worldSceneManagerScript;
    WaterBossAI waterBossAiScript;

    inhale inhaleScript;
    //Phase1
    GameObject cols;
    public int LeversActivated;
    //phase2
    public int whirlpoolDestroyed=0;

    string toBeLoaded;
    bool loadingOut;
    bool loadingIn;

    bool doOnce;
    //Fade fadeScript;


    // Use this for initialization
    void Start ()
    {
        waterBossAiScript = GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        PHASE1 = "Stage2Phase1"; PHASE2_3 = "Stage2Phase2"; PHASE3 = "Stage2Phase3";

        if (SceneManager.GetActiveScene().name == PHASE1)
        {
            cols = GameObject.Find("Cols");
        }
        else if (SceneManager.GetActiveScene().name == PHASE2_3)
        {
            inhaleScript = GameObject.Find("WaterBoss").transform.GetChild(5).GetComponent<inhale>();
            GameObject.Find("WaterBoss").transform.GetChild(5).gameObject.SetActive(false);
        }
    }
	
	void Update ()
    {
        if (SceneManager.GetActiveScene().name==PHASE1)
        {
            Phase1();
        }
        else if (SceneManager.GetActiveScene().name == PHASE2_3)
        {
            Phase2_3();
        }

        //DEBUG
        Nextphase();



    }

    public void Phase1()
    {
        //if player is pulled by hook sets colliders in cols off
        if (playerMovementScript.currentState == 1)
        {
            cols.gameObject.SetActive(false);
        }
        else
        {
            cols.gameObject.SetActive(true);
        }

        if (LeversActivated == 3)
        {
            WorldSceneManager.NextScene();
        }
    }

    public void Phase2_3()
    {
        if (whirlpoolDestroyed>=2&&!doOnce)
        {
            //WorldSceneManager.NextScene();
            waterBossAiScript.SM.SetNextState("ToCorner");
            waterBossAiScript.whirlpools.SetActive(false);
            doOnce = true;
        }
        if (inhaleScript.inhaledEnemies >= 20)
        {
            WorldSceneManager.LoadBreakRoom();

        }
    }
        //DEBUG
    public void Nextphase()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
