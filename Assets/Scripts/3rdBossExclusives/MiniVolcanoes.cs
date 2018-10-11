using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniVolcanoes : MonoBehaviour {

    //should probably make this use object pooling, do this once we know what fire rate we will use

    public SpriteRenderer tentacleRenderer;
    public SpriteRenderer tentaIndicatorRenderer;
    public Animator tentacleAnimator;
    public Collider2D damageCollider;
    public Collider2D blockCollider;
    public GameObject volcanoProjectile;

    //public variables for balancing



    public float maxRange = 3, minRange = -3, randomInvokeMin = 0.05f, randomInvokeMax = 0.9f, invokeRepeat = 1;




    AudioSource despawnSource;
    bool despawning;

    PlayerMovement playerMovement;

    
    GameObject newProjectile;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        damageCollider.enabled = false;
        blockCollider.enabled = false;
        despawnSource = GetComponent<AudioSource>();
        StartCoroutine(TentacleSpawn());


        float randomStart = Random.Range(randomInvokeMin, randomInvokeMax);
        InvokeRepeating("TacticalArtillery", randomStart, invokeRepeat);
    }

    public void hookHit()
    {
        if (!despawning)
        {
            despawnSource.Play();
            StartCoroutine(TentacleDeSpawn());
        }
    }

    private IEnumerator TentacleSpawn()
    {
        tentacleAnimator.Play("TentacleSpawnAnimation");
        yield return new WaitForSeconds(1.5f);
        damageCollider.enabled = true;
        blockCollider.enabled = true;
    }

    private IEnumerator TentacleDeSpawn()
    {
        despawning = true;
        StopCoroutine(TentacleSpawn());
        damageCollider.enabled = false;
        blockCollider.enabled = false;
        tentacleAnimator.Play("TentacleDeSpawnAnimation");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    
    private void TacticalArtillery()
    {
        print("TacticalArtillery");

        float xRandom = Random.Range(minRange, maxRange);
        float yRandom = Random.Range(minRange, maxRange);
        Vector2 spawnPos = new Vector2(playerMovement.transform.position.x + xRandom, playerMovement.transform.position.y + yRandom);
        newProjectile = Instantiate(volcanoProjectile);
        newProjectile.GetComponent<SpriteRenderer>().color = new Color(newProjectile.GetComponent<SpriteRenderer>().color.r, newProjectile.GetComponent<SpriteRenderer>().color.g, newProjectile.GetComponent<SpriteRenderer>().color.b, 0); //this line is horrible
        newProjectile.transform.position = spawnPos;

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
