using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniVolcanoes : MonoBehaviour {

    //should probably make this use object pooling, do this once we know what fire rate we will use

    public GameObject projectileShadow;

    //public variables for balancing



    public float maxRange = 3, minRange = -3, randomInvokeMin = 0.05f, randomInvokeMax = 0.9f, invokeRepeat = 1;
    public bool stopShooting, boss = false;



    AudioSource despawnSource;
    bool despawning;
    


    PlayerMovement playerMovement;

    
    GameObject newProjectile;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        float randomStart = Random.Range(randomInvokeMin, randomInvokeMax);
        InvokeRepeating("TacticalArtillery", randomStart, invokeRepeat);
    }

    public void hookHit()
    {
        if (!despawning && !boss)
        {
            StartCoroutine(TentacleDeSpawn());
        }
    }


    private IEnumerator TentacleDeSpawn()
    {
        despawning = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    
    private void TacticalArtillery()
    {
        print("TacticalArtillery");

        if(stopShooting == false)
        {
            float xRandom = Random.Range(minRange, maxRange);
            float yRandom = Random.Range(minRange, maxRange);
            Vector2 spawnPos = new Vector2(playerMovement.transform.position.x + xRandom, playerMovement.transform.position.y + yRandom);
            newProjectile = Instantiate(projectileShadow);
            newProjectile.GetComponent<SpriteRenderer>().color = new Color(newProjectile.GetComponent<SpriteRenderer>().color.r, newProjectile.GetComponent<SpriteRenderer>().color.g, newProjectile.GetComponent<SpriteRenderer>().color.b, 0); //this line is horrible
            newProjectile.transform.position = spawnPos;

            if(boss)
            {
                ThirdBossSM.animController.SetBool("PlayRockShootAnim", true);
                StartCoroutine(ShootAnimation());
            }
        }
    }

    IEnumerator ShootAnimation()
    {
        yield return new WaitForSeconds(0.45f);
        ThirdBossSM.animController.SetBool("PlayRockShootAnim", false);
    }
    
        
    

    public void DestroyTentacle()
    {
        if (!despawning)
        {
            StartCoroutine(TentacleDeSpawn());
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
