using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaoverflowDMG : MonoBehaviour {
    PlayerMovement player;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag == "Player")
        {
            player.TakeDamage();
        }
    }
}
