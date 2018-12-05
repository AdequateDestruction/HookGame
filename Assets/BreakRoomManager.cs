using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakRoomManager : MonoBehaviour {


    public Image electric;
    public Image water;
    public Image fire;


    private void Start()
    {
        electric = GameObject.Find("Normal").GetComponent<Image>();
        water = GameObject.Find("Water").GetComponent<Image>();
        fire = GameObject.Find("Fire").GetComponent<Image>();

        if (WorldSceneManager.LASTSCENEVISITEDSTRING== WorldSceneManager.ELECTRICSTAGE)
        {
            electric.gameObject.SetActive(false);
            water.gameObject.SetActive(true);
            fire.gameObject.SetActive(false);
        }
        else if(WorldSceneManager.LASTSCENEVISITEDSTRING == WorldSceneManager.WATERSTAGEPHASE1|| WorldSceneManager.LASTSCENEVISITEDSTRING == WorldSceneManager.WATERSTAGEPHASE2)
        {
            electric.gameObject.SetActive(false);
            water.gameObject.SetActive(false);
            fire.gameObject.SetActive(true);
        }
        else
        {
            electric.gameObject.SetActive(true);
            water.gameObject.SetActive(false);
            fire.gameObject.SetActive(false);
        }

    }


    public void LoadNextScene()
    {
        WorldSceneManager.LoadNextScene();
    }
    public void LoadInteractiveMenu()
    {
        WorldSceneManager.LoadInteractiveMenu();

    }

}
