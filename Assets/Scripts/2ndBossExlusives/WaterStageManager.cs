using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterStageManager : MonoBehaviour {
    public string PHASE1, PHASE2, PHASE3;

    PlayerMovement playerMovementScript;
    WorldSceneManager worldSceneManagerScript;
    inhale inhaleScript;
    //Phase1
    GameObject cols;
    public int LeversActivated;
    //phase2
    public int whirlpoolDestroyed=0;

    string toBeLoaded;
    bool loadingOut;
    bool loadingIn;
    Fade fadeScript;


    // Use this for initialization
    void Start ()
    {
        worldSceneManagerScript = GameObject.Find("WorldSceneManager").GetComponent<WorldSceneManager>();
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        fadeScript = GameObject.Find("fade").GetComponent<Fade>();

        PHASE1 = "Stage2Phase1"; PHASE2 = "Stage2Phase2"; PHASE3 = "Stage2Phase3";

        if (SceneManager.GetActiveScene().name == PHASE1)
        {
            cols = GameObject.Find("Cols");
        }
        else if (SceneManager.GetActiveScene().name == PHASE2)
        {

        }
        else if (SceneManager.GetActiveScene().name == PHASE3)
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
        else if (SceneManager.GetActiveScene().name == PHASE2)
        {
            Phase2();
        }
        else if (SceneManager.GetActiveScene().name == PHASE3)
        {
            Phase3();
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
            worldSceneManagerScript.NextScene();
        }
    }

    public void Phase2()
    {
        if (whirlpoolDestroyed>=2)
        {
            worldSceneManagerScript.NextScene();

        }
    }
    
    public void Phase3()
    {
        if (inhaleScript.inhaledEnemies >= 20)
        {
            worldSceneManagerScript.NextScene();

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
