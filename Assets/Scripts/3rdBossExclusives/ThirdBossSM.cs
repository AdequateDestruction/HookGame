using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossSM : MonoBehaviour {

    string currentState= "1stState";
    MiniVolcanoes thisVolcano;
    bool coroutineInProgress = false;


    public GameObject MiniVolcParent;
    public GameObject secondPhaseAmmunition;

	// Use this for initialization
	void Start () {
        thisVolcano = gameObject.GetComponent<MiniVolcanoes>();
	}
	
	// Update is called once per frame
	void Update () {

        switch (currentState)
        {
            case "1stState":
                CheckVolcanoes();
                break;
            case "BustinOut":
                if(coroutineInProgress == false)
                {
                    print("Bustin out coroutine started");
                    StartCoroutine("BustinOut");
                }
                break;
            case "2ndState":
                SecondState();
                break;
            default:
                break;
        }
    }

    void CheckVolcanoes()
    {
        if(MiniVolcParent.GetComponentInChildren<MiniVolcanoes>() == null)//Kaikki mini volcanot on tuhottu
        {
            print("all mini volcanoes dead");
            //voi tuhota sen isomman volcanon
            thisVolcano.stopShooting = true;
            currentState = "BustinOut";
        }
    }

    IEnumerator BustinOut() //Boss busts out of the volcano
    {
        coroutineInProgress = true;
        print("BustinOut");

        new WaitForSeconds(4f);
        currentState = "2ndState";
        StopCoroutine("BustinOut");
        yield return null;
    }

    void SecondState()
    {
        if(thisVolcano.projectileShadow != secondPhaseAmmunition)
        {
            thisVolcano.projectileShadow = secondPhaseAmmunition;
        }

        thisVolcano.stopShooting = false;
    }
}
