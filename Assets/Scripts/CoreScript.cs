using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreScript : MonoBehaviour
{
    public float moveSpeed;
    public bool beingRipped;
    public bool rippedOff;
    public Text debugText;
    public StageManager stageManager;
    public BoxCollider2D ripHitBox, pickUpHitBox;
    public Animator pieceAnimator;
    public AudioSource[] RipSources;

    int currentRipSource = 0;
    bool startedRipping;
    float ripTimer;
    Transform playerTransform;
    Vector3 originalPos;
    Vector3 playerPos;
    HookScript currentHook;
    LineRenderer lineRend;

    void Start()
    {
        ripHitBox.enabled = false;
        pickUpHitBox.enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        playerTransform = FindObjectOfType<PlayerMovement>().gameObject.transform;
        lineRend = GetComponent<LineRenderer>();
        lineRend.enabled = false;
    }
    
    void Update()
    {
        if (beingRipped)
        {
            if (Input.GetKeyDown(KeyCode.E) && Time.time >= ripTimer)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPos, moveSpeed * 1.7f * Time.deltaTime);
                RipSources[currentRipSource].Play();

                if (currentRipSource >= RipSources.Length - 1)
                {
                    currentRipSource = 0;
                }
                currentRipSource++;

                ripTimer = Time.time + 0.05f;
            }
            else if (Time.time >= ripTimer)
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPos, moveSpeed * 0.12f * Time.deltaTime);
            }

            if (!rippedOff && Vector3.Distance(transform.position, originalPos) * 0.8f > Vector3.Distance(transform.position, playerPos))
            {
                beingRipped = false;
                rippedOff = true;
                GetComponent<BoxCollider2D>().enabled = false;
                if (currentHook != null)
                {
                    currentHook.RippingEnd();
                }
                FindObjectOfType<PlayerMovement>().EndRipping();
            }
            lineRend.SetPosition(0, originalPos);
            lineRend.SetPosition(1, transform.position);
            lineRend.SetPosition(2, new Vector3(originalPos.x + 0.5f, originalPos.y - 0.3f));
        }
        else if (startedRipping)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, moveSpeed * 0.12f * Time.deltaTime);
            lineRend.SetPosition(0, originalPos);
            lineRend.SetPosition(1, transform.position);
            lineRend.SetPosition(2, new Vector3(originalPos.x + 0.5f, originalPos.y - 0.3f));
        }

        if (rippedOff)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * 0.8f * Time.deltaTime);
            pieceAnimator.enabled = false;
            lineRend.enabled = false;
        }
    }

    public void StartRipping(HookScript _currentHook)
    {
        currentHook = _currentHook;
        moveSpeed = currentHook.hookSpeed;
        originalPos = transform.position;
        lineRend.SetPosition(0, originalPos);
        lineRend.SetPosition(1, transform.position);
        lineRend.SetPosition(2, new Vector3(originalPos.x + 0.5f, originalPos.y - 0.3f));
        lineRend.enabled = true;
        beingRipped = true;
        playerPos = playerTransform.position;
        startedRipping = true;
        FindObjectOfType<BossScript>().StopVulnTimer();
        ripTimer = Time.time;
    }

    public void ExposeRipCollider()
    {
        ripHitBox.enabled = true;
        pickUpHitBox.enabled = true;
        RipPieceAnimPlay(1);
        FindObjectOfType<BossScript>().DisableCollider();
    }

    public void CoverRipColliders()
    {
        ripHitBox.enabled = false;
        pickUpHitBox.enabled = false;
        RipPieceAnimPlay(0);
    }

    public void ShowRipObject()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }

    public void ArmorPickUp()
    {
        FindObjectOfType<BossScript>().EnableCollider();
        FindObjectOfType<BossScript>().gameObject.GetComponentInChildren<ProjectileSpawner>().BossNotVulnerable();
        stageManager.ExposedArmorRipped();
        Destroy(gameObject);
    }

    public void CorePickUp()
    {
        //debugText.text = "Victory!";
        playerTransform.gameObject.GetComponent<PlayerMovement>().Victory();
        stageManager.StartVictoryState();
        FindObjectOfType<BossScript>().BossDeath();
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManagerScript>().SwitchToVictoryTrack();
        FindObjectOfType<ProjectileSpawner>().SetVictoryStage();
        Destroy(gameObject);
    }

    private void RipPieceAnimPlay(int _animIndex)
    {
        if (gameObject.name == "Stage3HeartArmorLeft")
        {
            if (_animIndex == 0)
            {
                pieceAnimator.Play("ArmorLeftAnimation");
            }
            else
            {
                pieceAnimator.Play("ArmorLeftVulnerableAnimation");
            }
        }
        else if (gameObject.name == "Stage3HeartArmorRight")
        {
            if (_animIndex == 0)
            {
                pieceAnimator.Play("ArmorRightAnimation");
            }
            else
            {
                pieceAnimator.Play("ArmorRightVulnerableAnimation");
            }
        }
        else if (gameObject.name == "Stage3Heart")
        {
            if (_animIndex == 0)
            {
                pieceAnimator.Play("ArmorCoreAnimation");
            }
            else
            {
                pieceAnimator.Play("ArmorCoreVulnerableAnimation");
            }
        }
    }
}
