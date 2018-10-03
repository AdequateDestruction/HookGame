using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;



public class PlayFabSM : MonoBehaviour
{
    List<string> scoreList;
    string userGivenName = "Pertti Jorma"; //name must be between 3-15 characters
    int playerScoreInt = 7;
    public double time= 00.2045;

    GameObject leaderboardScoresObj;
    GameObject leaderboardTitleObj;
    GameObject leaderboardNamesObj;
    public List<Text> texts = new List<Text>();
    public List<PlayerLeaderboardEntry> playerScores = new List<PlayerLeaderboardEntry>();


    public void Start()
    {
        leaderboardScoresObj = GameObject.FindGameObjectWithTag("LeaderboardScores");
        leaderboardTitleObj = GameObject.FindGameObjectWithTag("LeaderboardTitle");
        leaderboardNamesObj = GameObject.FindGameObjectWithTag("LeaderboardNames");
        
        var request = new LoginWithCustomIDRequest { CustomId = "AdequateDestruction", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        //UpdatePlayerStatistics();
        //GetPlayerStatistics();
        //UpdateDisplayName();
        GetLeaderboardScores();
    }


    //Debug for if the login fails
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    //fetches leaderboard information for the specified board and version.
    public void GetLeaderboardScores()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            ProfileConstraints = null,
            Version = 1,
            StatisticName = "Score",
            StartPosition = 0,
            MaxResultsCount = 10,

        },
            result => setplayerScores(result.Leaderboard),
            error => Debug.Log(error.GenerateErrorReport())            
        );

        
    }
    //sets the 'results' leaderboard info to a non-local variable so that it can be used elsewhere
    public void setplayerScores(List<PlayerLeaderboardEntry> playerLeaderboardEntries)
    {
        print("playerleaderboardsentires count " + playerLeaderboardEntries.Count);
        playerScores = playerLeaderboardEntries;
        print("Playerscores count " + playerScores.Count);

        showScores();
    }
    //shows the scores, altering the MaxResultCount will determine how many highscores are shown.
    public void showScores()
    {
        print("playerScores count " + playerScores.Count);

        if (playerScores != null)
        {
            leaderboardTitleObj.GetComponent<Text>().text = "halp halp"; //make this somehow search the name automatically

            for (int i = 0; i < playerScores.Count; i++)
            {
                print("Leaderboard UI score change " + playerScores[0].StatValue);
                leaderboardScoresObj.GetComponentsInChildren<Text>()[i].text = playerScores[i].StatValue.ToString();
                leaderboardNamesObj.GetComponentsInChildren<Text>()[i].text = playerScores[i].DisplayName;

            }
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
    }

    //for saving a new score, manual check needed so only the highest score is submitted to playfab
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
            result => Debug.Log("Complete"),
            error => Debug.Log(error.GenerateErrorReport())
        );   
    }

    //sets the playername for the user so that it can be shown in the leaderboard, name must be between 3 -25 characters
    public void UpdateDisplayName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = userGivenName
            },
            result => Debug.Log("Displayname changed"),
            error => Debug.Log(error.GenerateErrorReport())
        );
    }
}
