using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;





public class LeaderboardSet : MonoBehaviour {


    // Use this for initialization
    void Start () {
        UpdatePlayerStatistics();
        GetPlayerStatistics();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void GetPlayerStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest()
            {
                StatisticNames = new List<string>() { "Headshots" }
            },
            result => Debug.Log("Complete"),
            error => Debug.Log(error.GenerateErrorReport())
        );
    }

    public void UpdatePlayerStatistics()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate() {
                    StatisticName = "Headshots",
                    Version = 2,
                    Value = 10
                }
                }
            },
            result => Debug.Log("Complete"),
            error => Debug.Log(error.GenerateErrorReport())
        );
    }   
}
