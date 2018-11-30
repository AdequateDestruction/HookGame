using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSceneManager : MonoBehaviour {

    static WorldSceneManager instance;

    Fade fade;
    public bool Out;
    public bool In;

    public static string INTERACTIVEMENU, BREAKROOM, MAINMENU, SUBMITSCORESCENE;
    static int LASTSCENEVISITED;



    // Setting a static instance and checking it for DontDestroyOnLoad purposes
    public static WorldSceneManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        INTERACTIVEMENU = "InteractiveMainMenu";
        BREAKROOM = "BreakRoom";
        MAINMENU = "MainMenu";
        SUBMITSCORESCENE = "SubmitScoreScene";
    }


    void Start ()
    {

        //fade = this.transform.GetChild(0).GetComponent<Fade>();
	}


    void Update ()
    {



        //DEBUG
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextScene();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DebugPreviousScene();
        }

        //TODO function for fade that uses coroutine for fading
        //if (In)
        //{
        //    fade.FadeIn();
        //    if (fade.fade.a >= 1)
        //    {
        //        NextScene();
        //        In = false;
        //        Out = true;
        //        fade.elapsedTime = 0;
        //    }
        //}
        //if (Out)
        //{
        //    fade.FadeOut();
        //    if (fade.fade.a <=0.1f)
        //    {
        //        Out = false;
        //        fade.elapsedTime = 0;
        //    }
        //}

    }

    /// <summary>
    /// Always loads next scene in buildindex
    /// </summary>
    public static void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex==SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    void DebugPreviousScene()
    {
        if (SceneManager.GetActiveScene().buildIndex ==0)
        {
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);

        }
        if (SceneManager.GetActiveScene().buildIndex>=0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    /// <summary>
    /// Loads InteractiveMenu scene
    /// </summary>
    public static void LoadInteractiveMenu()
    {
        SceneManager.LoadScene(INTERACTIVEMENU);

    }

    /// <summary>
    /// Loads Breakroom scene & saves currentscene buildIndex to variable LASTSCENEVISITED.
    /// </summary>
    public static void LoadBreakRoom()
    {
        LASTSCENEVISITED = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(BREAKROOM);

    }


    /// <summary>
    /// When function LoadBreakRoom() is used saves currentscene index. LoadNextScene() uses index+1 to load next scene.
    /// </summary>
    public static void LoadNextScene()
    {
        SceneManager.LoadScene(LASTSCENEVISITED+1);

    }

}
