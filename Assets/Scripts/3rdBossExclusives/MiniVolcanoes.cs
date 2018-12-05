using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniVolcanoes : MonoBehaviour {

    //should probably make this use object pooling, do this once we know what fire rate we will use

    public GameObject projectileShadow;

    
    //invul flash
    SpriteRenderer bossVisual;
    public Color defaultColor, damageColor;
    public float invulnerableCD;
    bool inPool, invulnerable;
    float invulnerableTimer;
    public int invulFlashCounter;

    float flashTimer;

//public variables for balancing
    public float maxRange = 3, minRange = -3, randomInvokeMin = 0.05f, randomInvokeMax = 0.9f, invokeRepeat = 1;
    public bool stopShooting, boss = false;
    public int volcanoHP = 3;


    AudioSource despawnSource;
    bool despawning;
    


    PlayerMovement playerMovement;

    
    GameObject newProjectile;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        bossVisual = gameObject.GetComponent<SpriteRenderer>();

        float randomStart = Random.Range(randomInvokeMin, randomInvokeMax);
        InvokeRepeating("TacticalArtillery", randomStart, invokeRepeat);
    }

    public void hookHit()
    {
        if (volcanoHP > 0)
        {
            volcanoHP = volcanoHP - 1;
            invulnerable = true;
        }


        if (!despawning && !boss && volcanoHP <= 0)
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

    private void Update()
    {
        /*
        if (invulnerable)
        {
            if (invulFlashCounter % 10 == 0 || invulFlashCounter % 10 == 1)
            {
                bossVisual.color = damageColor;
            }
            else
            {
                bossVisual.color = defaultColor;
            }
            invulFlashCounter++;

            if (Time.time >= invulnerableTimer)
            {
                invulnerable = false;
                bossVisual.color = defaultColor;
            }
        }*/

        if(flashTimer < 1)
        {
            invulnerableTimer = Time.time + invulnerableCD;
        }
        else if(flashTimer >= 1)
        {
            flashTimer = 0;
        }

        if (invulnerable)
        {
            flashTimer += Time.deltaTime;
            if (invulFlashCounter % 10 == 0 || invulFlashCounter % 10 == 1)
            {
                bossVisual.color = damageColor;
            }
            else
            {
                bossVisual.color = defaultColor;
            }
            invulFlashCounter++;

            if (Time.time >= invulnerableTimer)
            {
                invulnerable = false;
                bossVisual.color = defaultColor;
            }
        }


    }
}
