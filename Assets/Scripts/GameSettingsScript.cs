using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsScript : MonoBehaviour
{
    // Mainly used to store difficulty select across scenes (from menu to game scenes)
    // Called by PlayerMovement on Start to set the Health of the player based on selected difficulty

    public int difficultyLevel;

    private static GameSettingsScript instance = null;

    public static GameSettingsScript Instance
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
    
    public void SetDifficulty(int _difficulty)
    {
        difficultyLevel = _difficulty;
    }

    public int GetDifficulty()
    {
        return difficultyLevel;
    }
}
