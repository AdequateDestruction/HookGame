using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TentacleScript : MonoBehaviour
{
    public float lifeTime;
    public SpriteRenderer tentacleRenderer;
    public SpriteRenderer tentaIndicatorRenderer;
    public Animator tentacleAnimator;
    public Collider2D damageCollider;
    public Collider2D blockCollider;

    AudioSource despawnSource;
    bool despawning;

    [SerializeField]
    EnemySpawner enemySpawner;

    void Start()
    {
        damageCollider.enabled = false;
        blockCollider.enabled = false;
        despawnSource = GetComponent<AudioSource>();
        StartCoroutine(TentacleSpawn());
        if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
        {
            enemySpawner= GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        }
    }

    public void hookHit()
    {
        if (!despawning)
        {
            despawnSource.Play();
            StartCoroutine(TentacleDeSpawn());
        }
        if (SceneManager.GetActiveScene().name=="InteractiveMainMenu")
        {
            enemySpawner.invoketentacle(3f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
            if (!despawning)
            {
                StartCoroutine(TentacleDeSpawn());
            }
        }
    }

    private IEnumerator TentacleSpawn()
    {

        if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
        {
            tentacleAnimator.Play("TentacleSpawnAnimation");
            yield return new WaitForSeconds(1.5f);
            damageCollider.enabled = true;
            blockCollider.enabled = true;

        }
        else
        {
            tentacleAnimator.Play("TentacleSpawnAnimation");
            yield return new WaitForSeconds(1.5f);
            damageCollider.enabled = true;
            blockCollider.enabled = true;
            yield return new WaitForSeconds(lifeTime);
            StartCoroutine(TentacleDeSpawn());
        }

    }

    private IEnumerator TentacleDeSpawn()
    {

        if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
        {
            StopCoroutine(TentacleSpawn());
            tentacleAnimator.Play("TentacleDeSpawnAnimation");
            yield return new WaitForSeconds(1f);
            this.gameObject.SetActive(false);
        }
        else
        {
            despawning = true;
            StopCoroutine(TentacleSpawn());
            damageCollider.enabled = false;
            blockCollider.enabled = false;
            tentacleAnimator.Play("TentacleDeSpawnAnimation");
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);

        }
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
