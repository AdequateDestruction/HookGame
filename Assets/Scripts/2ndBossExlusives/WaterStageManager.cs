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


    //RandomIndex for toCorner WaterBossState
    [SerializeField]
    int randomAmount=10;
    public List<int> randomIndex;
    public int index;
    int temp=0;
    int numberInsert;

    private void Awake()
    {
        PHASE1 = "Stage2Phase1"; PHASE2_3 = "Stage2Phase2"; PHASE3 = "Stage2Phase3";
    }

    // Use this for initialization
    void Start ()
    {
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();

        if (SceneManager.GetActiveScene().name == PHASE1)
        {
            cols = GameObject.Find("Cols");
        }
        else if (SceneManager.GetActiveScene().name == PHASE2_3)
        {
            inhaleScript = GameObject.Find("WaterBoss").transform.GetChild(5).GetComponent<inhale>();
            GameObject.Find("WaterBoss").transform.GetChild(5).gameObject.SetActive(false);
            waterBossAiScript = GameObject.Find("WaterBoss").GetComponent<WaterBossAI>();

            //RandomIndex for toCorner WaterBossState
            for (int i = 0; i < randomAmount; i++)
            {
                do
                {
                    numberInsert = Random.Range(0, waterBossAiScript.corners.Count);
                } while (numberInsert == temp);

                temp = numberInsert;
                randomIndex.Add(numberInsert);
            }
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
        Debug();
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
        if (whirlpoolDestroyed>=3&&!doOnce)
        {
            UnityEngine.Debug.Log("here");
            waterBossAiScript.SM.SetNextState("ToCorner");
            doOnce = true;
        }
        if (inhaleScript.inhaledEnemies >= 10)
        {
            //WorldSceneManager.LoadBreakRoom();
            waterBossAiScript.SM.SetNextState("Death");

        }
    }
        //DEBUG
    public void Debug()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.P))
        {
            // whirlpoolDestroyed++;
            waterBossAiScript.SM.SetNextState("ToCorner");

        }
    }

}
