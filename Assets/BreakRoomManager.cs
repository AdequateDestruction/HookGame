using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakRoomManager : MonoBehaviour {


    public void LoadNextScene()
    {
        WorldSceneManager.LoadNextScene();
    }
    public void LoadInteractiveMenu()
    {
        WorldSceneManager.LoadInteractiveMenu();

    }

}
