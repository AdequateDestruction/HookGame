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
    int playerScoreInt = 7;

    GameObject leaderboardScoresObj;
    GameObject leaderboardNamesObj;
    public GameObject leaderboardPanel;
    List<Text> texts = new List<Text>();
    List<PlayerLeaderboardEntry> playerScores = new List<PlayerLeaderboardEntry>();

    GameSettingsScript gameSettings;

    public Text submitScreenName;
    public Text mainMenuUserName;
    public Text totalScoreText;
    public string userName;

    public void Start()
    {
        if (GameObject.FindGameObjectWithTag("LeaderboardPanel") != null)
        {
            leaderboardPanel = GameObject.FindGameObjectWithTag("LeaderboardPanel");
            leaderboardScoresObj = GameObject.FindGameObjectWithTag("LeaderboardScores");
            leaderboardNamesObj = GameObject.FindGameObjectWithTag("LeaderboardNames");
            leaderboardPanel.SetActive(false);
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
    }


    //Debug for if the login fails
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    //fetches leaderboard information for the specified board and version. Should only be called if you want to show the scores
    public void GetLeaderboardScores()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            ProfileConstraints = null,
            Version = 0,
            StatisticName = "PlayerName",
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

        ShowScores();
    }
    //shows the scores, altering the MaxResultCount will determine how many highscores are shown.
    public void ShowScores()
    {
        print("playerScores count " + playerScores.Count);

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


    //getting player specific statistic/score
    public void GetPlayerStatistics()
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
                    StatisticName = "PlayerName",
                    Version = null,
                    Value = playerScoreInt
                }
                }
            },
            result => Debug.Log("Leaderboard UI score change " + playerScores[0].StatValue),
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

        if(totalScoreText != null)
        {
            float totalScore = (gameSettings.boss1Time + gameSettings.boss2Time + gameSettings.boss3Time) * 3;
            totalScoreText.text = totalScore.ToString();
            submitScreenName.text = userName;
        }
    }

    void FindLeaderboardComponents()
    {
        
        
        leaderboardPanel = GameObject.FindGameObjectWithTag("LeaderboardPanel");
        leaderboardScoresObj = GameObject.FindGameObjectWithTag("LeaderboardScores");
        leaderboardNamesObj = GameObject.FindGameObjectWithTag("LeaderboardNames");

        
    }
}
