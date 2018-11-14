using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public float boss1Time, boss2Time, boss3Time;
    ThirdBossSM thirdBoss;
    BossScript firstBoss;
    Text timerText;

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

    private void FixedUpdate()
    {
        if(timerText == null)
        {
            timerText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<Text>();
        }

        //third boss time management
        if (thirdBoss == null && SceneManager.GetActiveScene().name == "ThirdBoss")
        {
            thirdBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<ThirdBossSM>();
            print(thirdBoss);
        }

        if (thirdBoss != null)
        {
            if(thirdBoss.thirdPhaseHP > 0)
            {
                print(boss3Time);
                boss3Time += Time.deltaTime;
                //timerText.text = (int)Mathf.RoundToInt(boss3Time).ToString();

            }

            if(thirdBoss.playerDead == true && thirdBoss.thirdPhaseHP > 0)
            {
                boss3Time = 0;
            }
        }
        //////////////////////////////////////////////

        if(firstBoss == null && SceneManager.GetActiveScene().name == "Main")
        {
            firstBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>();
        }

        if (firstBoss != null)
        {
            if (firstBoss.currentStage2HP >= 0)
            {
                print(boss1Time);
                boss1Time += Time.deltaTime;
            }

            if (firstBoss.pMoveScript.currentHealth <= 0 && thirdBoss.thirdPhaseHP > 0)
            {
                boss1Time = 0;
            }
        }
    }
}
