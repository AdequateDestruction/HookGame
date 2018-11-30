using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayFabSM : MonoBehaviour
{
    List<string> scoreList;
    //string userGivenName = "Pertti Jorma"; //name must be between 3-15 characters
    //int playerScoreInt = 7;

    GameObject leaderboardScoresObj;
    GameObject leaderboardNamesObj;
    public GameObject leaderboardPanel;
    List<Text> texts = new List<Text>();
    List<PlayerLeaderboardEntry> playerScores = new List<PlayerLeaderboardEntry>();
    List<PlayerLeaderboardEntry> playerScoresBoss1 = new List<PlayerLeaderboardEntry>();
    List<PlayerLeaderboardEntry> playerScoresBoss2 = new List<PlayerLeaderboardEntry>();
    List<PlayerLeaderboardEntry> playerScoresBoss3 = new List<PlayerLeaderboardEntry>();
    List<PlayerLeaderboardEntry> playerCenteredScore = new List<PlayerLeaderboardEntry>();

    GameSettingsScript gameSettings;

    public Text submitScreenName;
    public Text mainMenuUserName;
    public Text totalScoreText;
    public string userName;
    public Text centeredPosition, centeredScore, centeredName;
    public float totalScore = 100000;

    public void Start()
    {
        if (GameObject.FindGameObjectWithTag("LeaderboardPanel") != null)
        {
            leaderboardPanel = GameObject.FindGameObjectWithTag("LeaderboardPanel");
            leaderboardScoresObj = GameObject.FindGameObjectWithTag("LeaderboardScores");
            leaderboardNamesObj = GameObject.FindGameObjectWithTag("LeaderboardNames");
            //leaderboardPanel.SetActive(false);
        }


        gameSettings = gameObject.GetComponent<GameSettingsScript>();
        //gameSettings = GameObject

        LoginToPlayfab();
    }

    public void LoginToPlayfab()
    {
        var request = new LoginWithCustomIDRequest { CustomId = "AdequateDestruction", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }


    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you have logged into playfab");
        //UpdatePlayerStatistics();
        //GetPlayerStatistics();
        //UpdateDisplayName();
        //GetLeaderboardScores();
        GetLeaderboardBoss1();
        GetLeaderboardBoss2();
        GetLeaderboardBoss3();
        GetLeaderboardTotalScores();
        ShowSpecificPlayerScore("TotalScore");
    }


    //Debug for if the login fails
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    //fetches leaderboard information for the specified board and version. Should only be called if you want to show the scores
    public void GetLeaderboardTotalScores()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            ProfileConstraints = null,
            Version = 0,
            StatisticName = "TotalScore",
            StartPosition = 0,
            MaxResultsCount = 20,

        },
            result => GetLeaderboard(result.Leaderboard),
            error => Debug.Log(error.GenerateErrorReport())            
        );  
    }
    //sets the 'results' leaderboard info to a non-local variable so that it can be used elsewhere
    public void GetLeaderboard(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {
        print("playerleaderboardsentires count " + playerLeaderboardEntries.Count);
        playerScores = playerLeaderboardEntries;
        print("Playerscores count " + playerScores.Count);

        ShowTotalScores();
    }
    //shows the scores, altering the MaxResultCount will determine how many highscores are shown.
    public void ShowTotalScores()
    {
        print("TotalplayerScores count " + playerScores.Count);

        if (playerScores != null)
        {
            for (int i = 0; i < playerScores.Count; i++)
            {
                             
                leaderboardScoresObj.GetComponentsInChildren<Text>()[i].text = playerScores[i].StatValue.ToString();
                leaderboardNamesObj.GetComponentsInChildren<Text>()[i].text = playerScores[i].DisplayName;
                

            }
            leaderboardPanel.SetActive(true);
        }
    }

    //BOSS 1
    public void UpdatePlayerStatisticsBoss1()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate() {
                    StatisticName = "Boss1",
                    Version = null,
                    Value = (int)gameSettings.boss1Time
                }
                }
            },
            result => Debug.Log("Leaderboard UI score change "/* + playerScoresBoss1[0].StatValue*/),
        error => Debug.Log(error.GenerateErrorReport())
        );
    }

    public void GetLeaderboardBoss1()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            ProfileConstraints = null,
            Version = 0,
            StatisticName = "Boss1",
            StartPosition = 0,
            MaxResultsCount = 20,

        },
            result => GetBoss1Scores(result.Leaderboard),
            error => Debug.Log(error.GenerateErrorReport())
        );
    }
    //sets the 'results' leaderboard info to a non-local variable so that it can be used elsewhere
    public void GetBoss1Scores(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {
        print("playerleaderboardsentires count " + playerLeaderboardEntries.Count);
        playerScoresBoss1 = playerLeaderboardEntries;
        print("Playerscores count " + playerScoresBoss1.Count);

        ShowScoresBoss1();
    }

    public void ShowScoresBoss1()
    {
        print("playerScoresBoss1 count " + playerScoresBoss1.Count);

        if (playerScoresBoss1 != null)
        {
            for (int i = 0; i < playerScoresBoss1.Count; i++)
            {
                print("Boss1 showscores");
                leaderboardScoresObj.GetComponentsInChildren<Text>()[i].text = playerScoresBoss1[i].StatValue.ToString();
                leaderboardNamesObj.GetComponentsInChildren<Text>()[i].text = playerScoresBoss1[i].DisplayName;


            }
            leaderboardPanel.SetActive(true);
        }
    }
    //BOSS 1 END

    //BOSS 2
    public void UpdatePlayerStatisticsBoss2()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate() {
                    StatisticName = "Boss2",
                    Version = null,
                    Value = (int)gameSettings.boss2Time
                }
                }
            },
            result => Debug.Log("Leaderboard UI score change " /*+ playerScoresBoss2[0].StatValue*/),
        error => Debug.Log(error.GenerateErrorReport())
        );
    }

    public void GetLeaderboardBoss2()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            ProfileConstraints = null,
            Version = 0,
            StatisticName = "Boss2",
            StartPosition = 0,
            MaxResultsCount = 20,

        },
            result => GetBoss2Scores(result.Leaderboard),
            error => Debug.Log(error.GenerateErrorReport())
        );
    }
    //sets the 'results' leaderboard info to a non-local variable so that it can be used elsewhere
    public void GetBoss2Scores(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {
        print("playerleaderboardsentires count " + playerLeaderboardEntries.Count);
        playerScoresBoss2 = playerLeaderboardEntries;
        print("Playerscores count " + playerScoresBoss2.Count);

        ShowScoresBoss2();
    }

    public void ShowScoresBoss2()
    {
        print("playerScoresBoss2 count " + playerScoresBoss2.Count);

        if (playerScoresBoss2 != null)
        {
            for (int i = 0; i < playerScoresBoss2.Count; i++)
            {

                leaderboardScoresObj.GetComponentsInChildren<Text>()[i].text = playerScoresBoss2[i].StatValue.ToString();
                leaderboardNamesObj.GetComponentsInChildren<Text>()[i].text = playerScoresBoss2[i].DisplayName;


            }
            leaderboardPanel.SetActive(true);
        }
    }
    //BOSS 2 END

    //BOSS 3
    public void UpdatePlayerStatisticsBoss3()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate() {
                    StatisticName = "Boss3",
                    Version = null,
                    Value = (int)gameSettings.boss3Time
                }
                }
            },
            result => Debug.Log("Leaderboard UI score change " /*+ playerScoresBoss3[0].StatValue*/),
        error => Debug.Log(error.GenerateErrorReport())
        );
    }

    public void GetLeaderboardBoss3()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            ProfileConstraints = null,
            Version = 0,
            StatisticName = "Boss3",
            StartPosition = 0,
            MaxResultsCount = 20,

        },
            result => GetBoss3Scores(result.Leaderboard),
            error => Debug.Log(error.GenerateErrorReport())
        );
    }
    //sets the 'results' leaderboard info to a non-local variable so that it can be used elsewhere
    public void GetBoss3Scores(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {
        print("playerleaderboardsentires count " + playerLeaderboardEntries.Count);
        playerScoresBoss3 = playerLeaderboardEntries;
        print("Playerscores count " + playerScoresBoss3.Count);
        
        ShowScoresBoss3();
    }

    public void ShowScoresBoss3()
    {
        print("playerScoresBoss3 count " + playerScoresBoss3.Count);

        if (playerScoresBoss3 != null)
        {
            for (int i = 0; i < playerScoresBoss3.Count; i++)
            {

                leaderboardScoresObj.GetComponentsInChildren<Text>()[i].text = playerScoresBoss3[i].StatValue.ToString();
                leaderboardNamesObj.GetComponentsInChildren<Text>()[i].text = playerScoresBoss3[i].DisplayName;


            }
            leaderboardPanel.SetActive(true);
        }
    }
    //BOSS 3 END
    //getting player specific statistic/score
    public void GetPlayerStatistics() //not used
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest()
            {
                StatisticNames = new List<string> { "PlayerName" },
            },
            result =>Debug.Log("Complete " + result.Statistics[0].Value),
            error => Debug.Log(error.GenerateErrorReport())
        );
            leaderboardPanel.SetActive(true);
        
    }

    //for saving a new score, manual check needed so only the highest score is submitted to playfab?
    public void UpdatePlayerStatistics()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate() {
                    StatisticName = "TotalScore",
                    Version = null,
                    Value = (int)totalScore
                }
                }
            },
            result => Debug.Log("Leaderboard UI score change "/* + playerScores[0].StatValue*/),
        error => Debug.Log(error.GenerateErrorReport())
        );   
    }


    //sets the playername for the user so that it can be shown in the leaderboard, name must be between 3 -25 characters
    public void UpdateDisplayName(Text userGivenName)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = userGivenName.text
            },
            result => Debug.Log("Displayname changed to " + userGivenName.text),
            error => Debug.Log(error.GenerateErrorReport() + userGivenName.text)
        );
    }

    private void Update()
    {
        if(leaderboardPanel == null) //make this aggressive if needed(it instantiates the canvas if it is missing)
        {
            leaderboardPanel = GameObject.FindGameObjectWithTag("LeaderboardPanel");
        }

        if (SceneManager.GetActiveScene().name == WorldSceneManager.MAINMENU)
        {
            userName = mainMenuUserName.text;
        }

        if(totalScoreText != null) //for submit score scene
        {
            totalScore = (gameSettings.boss1Time + gameSettings.boss2Time + gameSettings.boss3Time) * 2;
            totalScoreText.text = totalScore.ToString();
            submitScreenName.text = userName;
        }

        if(leaderboardNamesObj == null)
        {
            leaderboardNamesObj = GameObject.FindGameObjectWithTag("LeaderboardNames");
        }

        if(leaderboardScoresObj == null)
        {
            leaderboardScoresObj = GameObject.FindGameObjectWithTag("LeaderboardScores");
        }

    }

    void FindLeaderboardComponents()
    {
        
        
        leaderboardPanel = GameObject.FindGameObjectWithTag("LeaderboardPanel");
        leaderboardScoresObj = GameObject.FindGameObjectWithTag("LeaderboardScores");
        leaderboardNamesObj = GameObject.FindGameObjectWithTag("LeaderboardNames");

        
    }

    public void ShowSpecificPlayerScore(string whichLeaderboard)
    {
        PlayFabClientAPI.GetLeaderboardAroundPlayer(
            new GetLeaderboardAroundPlayerRequest()
            {
                MaxResultsCount = 1,
                StatisticName = whichLeaderboard,
                Version = 0
            },
            result => GetCenteredScore(result.Leaderboard),
            error => Debug.Log("unable to get player centered score")


            );
    }

    public void GetCenteredScore(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {
        print("playerleaderboardsentires count " + playerLeaderboardEntries.Count);
        playerCenteredScore = playerLeaderboardEntries;
        print("Playerscores count " + playerCenteredScore.Count);

        ShowPlayerCenteredScore();
    }

    public void ShowPlayerCenteredScore()
    {
        centeredPosition.text = (playerCenteredScore[0].Position +1).ToString();
        centeredScore.text = playerCenteredScore[0].StatValue.ToString();
        centeredName.text = playerCenteredScore[0].DisplayName;
    }

}
