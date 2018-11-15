using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPortal : MonoBehaviour {

    public bool IsStart = false;
    public bool IsQuit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsStart == true)
        {
            if (collision.gameObject.tag == "Player")
            {
                //SceneManager.LoadScene("Main");
                WorldSceneManager.NextScene();
            }
        }

        if (IsQuit == true)
        {
            if (collision.gameObject.tag == "Player")
            {
                Application.Quit();
            }
        }
    }
}
