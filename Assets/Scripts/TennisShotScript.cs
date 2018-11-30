using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TennisShotScript : MonoBehaviour {

    public float moveSpeed;
    public int pongedTimes;
    public int pongedRequirement;

    Animator tShotAnimator;
    Transform bossTransform;
    Transform wallTransform;
    AudioSource pongSource;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "InteractiveMainMenu")
        {
            bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
        }
        if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
        {
            wallTransform = GameObject.Find("PongWall").GetComponent<Transform>();
        }
        tShotAnimator = GetComponent<Animator>();
        pongSource = GetComponent<AudioSource>();
        tShotAnimator.Play("PH_TennisShotAnimation");
        pongedTimes = 0;
    }

    void Update()
    {
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
                Destroy(gameObject);
            }

        }
        else if (other.tag == "PullBlock" || other.tag == "StaticBlock" || other.tag == "DeflectBlock")
        {
            if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (other.tag == "Boss")
        {
            if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
            {
                wallTransform = other.gameObject.GetComponent<Transform>();
                WallPong();
            }

             if (pongedTimes > pongedRequirement)
            {
                if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    FindObjectOfType<StageManager>().ExposeArmorPiece();
                    FindObjectOfType<ProjectileSpawner>().TennisShotDestroyed();
                    Destroy(gameObject);
                }

            }
            else
            {
                BossPong();
            }
        }
    }

    public void HookShotPong(Vector3 _hookPos)
    {
        tShotAnimator.Play("PH_TennisShotPongAnimation");
        transform.eulerAngles = new Vector3(0, 0, -Mathf.Atan2((_hookPos.y - transform.position.y), -(_hookPos.x - transform.position.x)) * Mathf.Rad2Deg - 90);
        moveSpeed = moveSpeed * 1.1f;
        pongedTimes = pongedTimes + 1;
        pongSource.Play();
        pongSource.pitch = pongSource.pitch + 0.1f;
    }

    private void BossPong()
    {
        tShotAnimator.Play("PH_TennisShotPongAnimation");
        transform.eulerAngles = new Vector3(0, 0, -Mathf.Atan2((bossTransform.position.y - transform.position.y), -(bossTransform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90);
        moveSpeed = moveSpeed * 1.1f;
        pongedTimes = pongedTimes + 1;
        pongSource.Play();
        pongSource.pitch = pongSource.pitch + 0.1f;
    }

    private void WallPong()
    {

        tShotAnimator.Play("PH_TennisShotPongAnimation");
        transform.eulerAngles = new Vector3(0, 0, -Mathf.Atan2((wallTransform.position.y - transform.position.y), -(wallTransform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90);
        moveSpeed = moveSpeed * 1.1f;
        pongedTimes = pongedTimes + 1;
        pongSource.Play();
        pongSource.pitch = pongSource.pitch + 0.1f;
    }

    public void SetPongRequirement(int _requirement)
    {
        pongedRequirement = _requirement;
    }
}
