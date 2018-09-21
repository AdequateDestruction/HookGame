using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisShotScript : MonoBehaviour {

    public float moveSpeed;
    public int pongedTimes;
    public int pongedRequirement;

    Animator tShotAnimator;
    Transform bossTransform;
    AudioSource pongSource;

    void Start()
    {
        bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
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
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
            Destroy(gameObject);
        }
        else if (other.tag == "PullBlock" || other.tag == "StaticBlock" || other.tag == "DeflectBlock")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Boss")
        {
            if (pongedTimes > pongedRequirement)
            {
                FindObjectOfType<StageManager>().ExposeArmorPiece();
                FindObjectOfType<ProjectileSpawner>().TennisShotDestroyed();
                Destroy(gameObject);
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

    public void SetPongRequirement(int _requirement)
    {
        pongedRequirement = _requirement;
    }
}
