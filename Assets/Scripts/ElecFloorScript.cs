using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecFloorScript : MonoBehaviour
{
    public int currentState; // 0 = inactive, 1 = warning, 2 = active
    public float activeTime, warningTime;
    public Color defaultTrapColor, defaultWarningColor, brokenColor;
    public GameObject trapEffectPrefab;
    public GameObject smokeEffectPrefab1, smokeEffectPrefab2;
    public SpriteRenderer warningRenderer, trapRenderer;
    public FloorDisplayScript floorDisplayScript;

    Animator trapAnimator;
    BoxCollider2D trapCollider;
    float activeTimer;
    float warningTimer;
    bool broken;
    GameObject smokeEffect1, smokeEffect2;

    void Start()
    {
        currentState = 0;
        trapCollider = GetComponent<BoxCollider2D>();
        trapCollider.enabled = false;
        trapAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentState == 0) // Inactive
        {
            
        }
        else if (currentState == 1) // Warning
        {
            if (Time.time >= warningTimer)
            {
                trapAnimator.enabled = false;
                trapCollider.enabled = true;
                warningRenderer.color = defaultWarningColor;
                activeTimer = Time.time + activeTime;
                currentState = 2;
                Instantiate(trapEffectPrefab, transform.position, new Quaternion(0, 0, 0, 0));
            }
        }
        else if (currentState == 2) // Active
        {
            if (Time.time >= activeTimer)
            {
                floorDisplayScript.DeActivateLight(gameObject.name);
                trapCollider.enabled = false;
                trapAnimator.enabled = false;
                currentState = 0;
            }
        }
    }

    public void ActivateTrap()
    {
        if (!broken)
        {
            floorDisplayScript.ActivateLight(gameObject.name);
            warningTimer = Time.time + warningTime;
            trapAnimator.enabled = true;
            trapAnimator.Play("PH_TrapWarningAnimation");
            currentState = 1;
        }
    }

    public void DeactivateTrap()
    {
        floorDisplayScript.DeActivateLight(gameObject.name);
        trapAnimator.Play(0);
        trapAnimator.enabled = false;
        trapCollider.enabled = false;
        trapRenderer.color = defaultTrapColor;
        warningRenderer.color = defaultWarningColor;
        currentState = 0;

        StartCoroutine(DeSpawnSmokeEffect());
    }

    public void BreakTrap()
    {
        floorDisplayScript.DeActivateLight(gameObject.name);
        trapAnimator.Play(0);
        trapAnimator.enabled = false;
        trapCollider.enabled = false;
        trapRenderer.color = brokenColor;
        warningRenderer.color = defaultWarningColor;
        currentState = 0;
        broken = true;

        StartCoroutine(SpawnSmokeEffect());
    }

    private IEnumerator SpawnSmokeEffect()
    {
        smokeEffect1 = Instantiate(smokeEffectPrefab1, transform.position, new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(0.5f);
        smokeEffect2 = Instantiate(smokeEffectPrefab2, transform.position, new Quaternion(0, 0, 0, 0));
    }

    private IEnumerator DeSpawnSmokeEffect()
    {
        Destroy(smokeEffect1);
        yield return new WaitForSeconds(0.5f);
        Destroy(smokeEffect2);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
        }
        else if (other.tag == "Boss")
        {
            other.gameObject.GetComponent<BossScript>().Stage1TakeDamage();
            BreakTrap();
        }
    }
}
