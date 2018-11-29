using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTentacle : MonoBehaviour
{
    public float lifeTime;
    public SpriteRenderer tentacleRenderer;
    public SpriteRenderer tentaIndicatorRenderer;
    public Animator tentacleAnimator;
    public Collider2D damageCollider;
    public Collider2D blockCollider;

    AudioSource despawnSource;
    bool despawning;

    void Start()
    {
        damageCollider.enabled = false;
        blockCollider.enabled = false;
        despawnSource = GetComponent<AudioSource>();
        StartCoroutine(TentacleSpawn());
    }

    public void hookHit()
    {
        if (!despawning)
        {
            Debug.Log("Hookhit");
            despawnSource.Play();
            StartCoroutine(TentacleDeSpawn());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
            //if (!despawning)
            //{
            //    StartCoroutine(TentacleDeSpawn());
            //}
        }
    }



    private IEnumerator TentacleSpawn()
    {
        tentacleAnimator.Play("TentacleSpawnAnimation");
        yield return new WaitForSeconds(1.5f);
        damageCollider.enabled = true;
        blockCollider.enabled = true;
        yield return new WaitForSeconds(lifeTime);
        StartCoroutine(TentacleDeSpawn());
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
