using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButton : MonoBehaviour {

    public GameObject playfab;
    float timer = 12;
    bool timerOn = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(timerOn && timer < 11)
        {
            timer = timer + Time.deltaTime;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && timer > 10)
        {
            print("debug");
            playfab.GetComponent<PlayFabSM>().GetLeaderboardScores();
            timerOn = true;
            timer = 0;
        }
    }
}
