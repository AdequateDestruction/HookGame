using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButton : MonoBehaviour {

    public PlayFabSM playfab;
    float timer = 12;
    bool timerOn = true;
    public GameObject buttondown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (playfab == null)
        {
            playfab = GameObject.FindGameObjectWithTag("Playfab").GetComponent<PlayFabSM>();
        }

        if (timerOn && timer < 11)
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
            playfab.GetLeaderboardBoss1();
            playfab.GetLeaderboardBoss2();
            playfab.GetLeaderboardBoss3();
            playfab.GetLeaderboardTotalScores();
            playfab.ShowTotalScores();
            playfab.ShowSpecificPlayerScore("TotalScore");
            timerOn = true;
            timer = 0;
            buttondown.SetActive(true);
        }
    }
}
