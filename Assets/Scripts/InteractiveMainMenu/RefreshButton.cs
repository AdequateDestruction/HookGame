using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButton : MonoBehaviour {

    public GameObject playfab;
    float timer = 12;
    bool timerOn = true;
    public GameObject buttondown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(timerOn && timer < 11)
        {
            timer = timer + Time.deltaTime;
        }

        if (timer >= 10)
        {
            buttondown.SetActive(false);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && timer > 10)
        {
            print("debug");
            playfab.GetComponent<PlayFabSM>().GetLeaderboardBoss1();
            playfab.GetComponent<PlayFabSM>().GetLeaderboardBoss2();
            playfab.GetComponent<PlayFabSM>().GetLeaderboardBoss3();
            playfab.GetComponent<PlayFabSM>().GetLeaderboardTotalScores();
            playfab.GetComponent<PlayFabSM>().ShowTotalScores();
            playfab.GetComponent<PlayFabSM>().ShowSpecificPlayerScore("TotalScore");
            timerOn = true;
            timer = 0;
            buttondown.SetActive(true);
        }
    }
}
