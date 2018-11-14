using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterStageManager : MonoBehaviour {
    public string PHASE1, PHASE2, PHASE3;

    PlayerMovement playerMovementScript;
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
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        fadeScript = GameObject.Find("fade").GetComponent<Fade>();

        PHASE1 = "Stage2Phase1"; PHASE2 = "Stage2Phase2"; PHASE3 = "Stage2Phase3";
        loadingIn = true;

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

        //Fade between sceneloads
        if (loadingOut)
        {
            fadeScript.FadeIn();
            if (fadeScript.fade.a>=1)
            {
                SceneManager.LoadScene(toBeLoaded);
            }
        }
        if (loadingIn)
        {
            fadeScript.FadeOut();
            if (fadeScript.fade.a <= 0)
            {
                fadeScript.elapsedTime = 0f;
                loadingIn = false;
            }
        }

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
            SceneManager.LoadScene(PHASE2);
        }
    }

    public void Phase2()
    {
        if (whirlpoolDestroyed>=2)
        {
            SceneManager.LoadScene(PHASE3);
        }
    }
    
    public void Phase3()
    {
        if (inhaleScript.inhaledEnemies >= 20)
        {
            SceneManager.LoadScene(PHASE1);
        }
    }

        //DEBUG
    public void Nextphase()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            toBeLoaded = PHASE1;
            loadingOut = true;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            toBeLoaded = PHASE2;
            loadingOut = true;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            toBeLoaded = PHASE3;
            loadingOut = true;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
