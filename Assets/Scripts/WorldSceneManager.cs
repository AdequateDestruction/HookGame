using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSceneManager : MonoBehaviour {

    static WorldSceneManager instance;

    Fade fade;
    public bool Out;
    public bool In;
    

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
    }


    void Start ()
    {

        fade = this.transform.GetChild(0).GetComponent<Fade>();
	}


    void Update ()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            In = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            In = true;
        }
        Debug.Log(fade.fade.a);

        if (In)
        {
            fade.FadeIn();
            if (fade.fade.a >= 1)
            {
                NextScene();
                In = false;
                Out = true;
                fade.elapsedTime = 0;
            }
        }
        if (Out)
        {
            fade.FadeOut();
            if (fade.fade.a <=0.1f)
            {
                Out = false;
                fade.elapsedTime = 0;
            }
        }

    }

    
    public static void  NextScene()
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
    void PreviousScene()
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
}
