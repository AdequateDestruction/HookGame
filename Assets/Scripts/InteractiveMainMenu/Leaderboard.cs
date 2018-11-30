using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour {

    public PlayFabSM playfab;
    public string leaderboardToShow;
    public GameObject boss1down;
    public GameObject boss2down;
    public GameObject boss3down;
    public GameObject bosstotaldown;

    // Use this for initialization
    void Start () {
        playfab = playfab.GetComponent<PlayFabSM>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && leaderboardToShow == "TotalScore")
        {
            playfab.ShowTotalScores();
            playfab.ShowSpecificPlayerScore(leaderboardToShow);
            bosstotaldown.SetActive(true);
            boss1down.SetActive(false);
            boss2down.SetActive(false);
            boss3down.SetActive(false);
        }

        if (collision.tag == "Player" && leaderboardToShow == "Boss1")
        {
            playfab.ShowScoresBoss1();
            playfab.ShowSpecificPlayerScore(leaderboardToShow);
            bosstotaldown.SetActive(false);
            boss1down.SetActive(true);
            boss2down.SetActive(false);
            boss3down.SetActive(false);
        }

        if (collision.tag == "Player" && leaderboardToShow == "Boss2")
        {
            playfab.ShowScoresBoss2();
            playfab.ShowSpecificPlayerScore(leaderboardToShow);
            bosstotaldown.SetActive(false);
            boss1down.SetActive(false);
            boss2down.SetActive(true);
            boss3down.SetActive(false);
        }

        if (collision.tag == "Player" && leaderboardToShow == "Boss3")
        {
            playfab.ShowScoresBoss3();
            playfab.ShowSpecificPlayerScore(leaderboardToShow);
            bosstotaldown.SetActive(false);
            boss1down.SetActive(false);
            boss2down.SetActive(false);
            boss3down.SetActive(true);
        }
    }
}
