using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterStageManager : MonoBehaviour {
    public string PHASE1;
    public string PHASE2;
    public string PHASE3;


    PlayerMovement playerMovementScript;
    inhale inhaleScript;
    //Phase1
    public GameObject cols;
    public int LeversActivated;
    //phase2
    public int whirlpoolDestroyed=0;

	// Use this for initialization
	void Start ()
    {
        PHASE1 = "Stage2Phase1"; PHASE2 = "Stage2Phase2"; PHASE3 = "Stage2Phase3";
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();

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
	
	// Update is called once per frame
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


        Nextphase();

    }


    public void Phase1()
    {
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

    public void Nextphase()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(PHASE1);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(PHASE2);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(PHASE3);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void reloadPhase()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
