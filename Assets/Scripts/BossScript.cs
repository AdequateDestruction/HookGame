using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public int currentStage;            // Tracking stage of the fight
    public int currentState;            // Tracking state machine stage
    public int currentStage1HP;
    public int currentStage2HP;
    public float jumpCooldown;
    public float invulnerableCD;
    public StageManager stageManager;
    public Text debugText;
    public Vector3 stage2StartPos;      // Where the boss should jump to during stage transitions
    public Vector3 stage3StartPos;
    public float jumpJourneyTime;       // How long the boss should stay in the air, if you change this, you should also fiddle with the bosses jump animation as it is timed to be the same lenght
    public Color defaultColor, damageColor;
    public CircleCollider2D bossCollider, damageCollider;       // Boss collider (solid) blocks the hookshot and player movement through the boss, damage collider (trigger) deals damage to player on contact
    public BoxCollider2D stage3BossCollider, stage3damageCollider;  // Same as above but for stage3, to account for the different shape of the boss in this stage
    public PHCameraScript cameraScript;
    public SpriteRenderer bossVisual;
    public Transform leftTrapTrans, rightTrapTrans;
    public ProjectileSpawner projSpawner;
    public AudioSource landingSource, jumpSource, damageSource, tentacleSpawnSource1, tentacleSpawnSource2;
    public AudioSource[] deathSounds;
    public BoxCollider2D[] jumpPushColliders;
    public BoxCollider2D[] jumpBlockColliders;

    bool readyForStage2;    // booleans for checking if the boss has taken enough damage per stage to transition
    bool movingToStage3;
    bool landed;            // Flips when the boss jumps and when it reaches its destination
    bool invulnerable;
    float jumpTimer;
    float jumpStartTime;
    int invulFlashCounter;
    float invulnerableTimer;
    float stage3vulnTimer;
    bool stage3vulnerable;
    Animator bossAnimator;
    Transform playerTransform;
    public PlayerMovement pMoveScript;
    Vector3 jumpStartPos;
    Vector3 jumpTarget;
    bool playerAbove;


    void Start()
    {
        currentStage = 1;
        currentState = 0;
        bossAnimator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        pMoveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        jumpTimer = Time.time + jumpCooldown;
        stage3BossCollider.enabled = false;
        stage3damageCollider.enabled = false;
        playerAbove = false;
    }

    void Update()
    {
        // Cheat for skipping stage1
        if (Input.GetKeyDown(KeyCode.B) && currentStage == 1)
        {
            Stage1TakeDamage();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BossDeath();
        }

        // Basic state machine for boss behaviour, checks the current stage of the fight, then the state the boss is in, acts accordingly
        if (currentStage == 1)
        {
            if (currentState == 0) // Stage1 waiting for jump to cooldown, then preparing to jump
            {
                if (Time.time >= jumpTimer)
                {
                    PrepareToJump();
                }
            }
            else if (currentState == 1) // Stage1 jumping to player pos
            {
                if (!landed)
                {
                    JumpToTarget();
                }
                else        // Landing the jump, resetting timer for next jump
                {
                    cameraScript.StartCameraShake();
                    jumpTimer = Time.time + jumpCooldown;
                    currentState = 0;
                }
            }
            else if (currentState == 2) // Stage1 transitioning to Stage2
            {
                JumpToTarget();

                if (readyForStage2 && transform.position == jumpTarget)     // Boss has taken enough damage to flip the readyForStage2 bool, and has completed the jump to the middle of the stage
                {
                    cameraScript.StartCameraShake();
                    //debugText.text = "Starting Stage2";
                    stageManager.TransitionToStage2();
                    currentState = 0;
                    currentStage = 2;
                    bossAnimator.Play("BossStage2TransitionAnimation");
                }
            }

            // Checking player pos compared to boss pos, if player goes "above" the boss in the stage, switch players order in sprite layers to show it "behind" the boss
            if (transform.position.y + 0.2f < playerTransform.position.y && !playerAbove)
            {
                pMoveScript.SwitchSpriteLayer(5);
                playerAbove = true;
            }
            else if (transform.position.y + 0.2f > playerTransform.position.y && playerAbove)
            {
                pMoveScript.SwitchSpriteLayer(9);
                playerAbove = false;
            }
        }
        else if (currentStage == 2)
        {
            // Not doing much in stage2 as the attacking is mostly handled by Projectile spawner. Possible to add something for a hard mode or something similar
            if (currentState == 0)
            {

            }
            else if (currentState == 1)
            {
                if (!landed && movingToStage3)
                {
                    JumpToTarget();
                }
                else
                {
                    currentStage = 3;
                    bossCollider.enabled = false;
                    damageCollider.enabled = false;
                    stage3BossCollider.enabled = true;
                    stage3damageCollider.enabled = true;
                    bossAnimator.Play("BossStage3TransitionAnimation");
                }
            }

            // Checking player pos compared to boss pos, if player goes "above" the boss in the stage, switch players order in sprite layers to show it "behind" the boss
            if (transform.position.y + 0.2f < playerTransform.position.y && !playerAbove)
            {
                pMoveScript.SwitchSpriteLayer(5);
                playerAbove = true;
            }
            else if (transform.position.y + 0.2f > playerTransform.position.y && playerAbove)
            {
                pMoveScript.SwitchSpriteLayer(9);
                playerAbove = false;
            }
        }
        else if (currentStage == 3)
        {
            if (stage3vulnerable)
            {
                if (Time.time >= stage3vulnTimer)
                {
                    stage3vulnerable = false;
                    TimeOutEnableCollider();
                }
            }
        }

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
        }
    }

    // Checking where the boss should jump to, if the landing position is "safe" and setting variables for the actual jump
    private void PrepareToJump()
    {
        jumpTarget = playerTransform.position;

        if (CheckLandingPos())
        {
            bossCollider.enabled = false;
            damageCollider.enabled = false;
            jumpStartPos = transform.position;
            jumpStartTime = Time.time;
            landed = false;
            currentState = 1;
            bossVisual.sortingOrder = 14;
            bossAnimator.Play("PH_BossJumpShadowAnimation");
            jumpSource.Play();
        }
        else    // If the jump position is blocked (around the bottom traps or top right of the stage, spawn a tentacle instead
        {
            jumpTimer = Time.time + jumpCooldown;
            currentState = 0;
            stageManager.SpawnTentacle();
            if (Random.Range(0, 2) == 0)
            {
                tentacleSpawnSource1.Play();
            }
            else
            {
                tentacleSpawnSource2.Play();
            }
        }
    }

    // Checks if the suggested landing position is valid based on lists of colliders and set distances from the bottom traps (these values might need to be adjusted if the boss jumps to weird spots when testing
    private bool CheckLandingPos()
    {
        foreach (BoxCollider2D blockCollider in jumpBlockColliders)
        {
            if (blockCollider.bounds.Contains(jumpTarget))
            {
                return false;
            }
        }

        if ((jumpTarget.y > leftTrapTrans.position.y - 2.3f && jumpTarget.y < leftTrapTrans.position.y + 2.3f) && (jumpTarget.x > leftTrapTrans.position.x - 2.3f && jumpTarget.x < leftTrapTrans.position.x + 2.3f))
        {
            jumpTarget = leftTrapTrans.position;
            jumpTarget.y = jumpTarget.y + 0.41f;
            return true;
        }
        else if ((jumpTarget.y > rightTrapTrans.position.y - 2.3f && jumpTarget.y < rightTrapTrans.position.y + 2.3f) && (jumpTarget.x > rightTrapTrans.position.x - 2.3f && jumpTarget.x < rightTrapTrans.position.x + 2.3f))
        {
            jumpTarget = rightTrapTrans.position;
            jumpTarget.y = jumpTarget.y + 0.41f;
            return true;
        }

        foreach (BoxCollider2D pushCollider in jumpPushColliders)
        {
            if (pushCollider.bounds.Contains(jumpTarget))
            {
                if (pushCollider.gameObject.name == "Right")
                {
                    jumpTarget.x = jumpTarget.x + 1.5f;
                }
                else if (pushCollider.gameObject.name == "Left")
                {
                    jumpTarget.x = jumpTarget.x - 1.5f;
                }
                else if (pushCollider.gameObject.name == "Up")
                {
                    jumpTarget.y = jumpTarget.y + 1.5f;
                }
                else if (pushCollider.gameObject.name == "Down")
                {
                    jumpTarget.y = jumpTarget.y - 1.5f;
                }
            }
        }

        return true;
    }

    void JumpToTarget()
    {
        float fracComplete = (Time.time - jumpStartTime) / jumpJourneyTime;
        transform.position = Vector3.Lerp(jumpStartPos, jumpTarget, fracComplete);

        if (transform.position == jumpTarget)
        {
            bossCollider.enabled = true;
            damageCollider.enabled = true;
            landed = true;
            bossVisual.sortingOrder = 8;
            landingSource.Play();
        }
    }

    public void DisableCollider()
    {
        stage3BossCollider.enabled = false;
        stage3vulnerable = true;
        stage3vulnTimer = Time.time + 8f;
        invulnerableTimer = Time.time + invulnerableCD / 4;
        invulnerable = true;
        invulFlashCounter = 0;
        damageSource.Play();
    }

    public void StopVulnTimer()
    {
        stage3vulnerable = false;
    }

    public void TimeOutEnableCollider()
    {
        stage3BossCollider.enabled = true;
        projSpawner.BossNotVulnerable();
        stageManager.RipTimeOut();
    }

    public void EnableCollider()
    {
        stage3BossCollider.enabled = true;
        invulnerableTimer = Time.time + invulnerableCD;
        invulnerable = true;
        invulFlashCounter = 0;
        damageSource.Play();
    }

    public void Stage1TakeDamage()
    {
        if (!invulnerable)
        {
            currentStage1HP = currentStage1HP - 1;
            invulnerableTimer = Time.time + invulnerableCD;
            invulnerable = true;
            invulFlashCounter = 0;
            projSpawner.BossDamaged();
            damageSource.Play();

            if (currentStage1HP <= 0)
            {
                currentState = 2;
                //debugText.text = "Transitioning to Stage2";
                jumpTarget = stage2StartPos;
                bossCollider.enabled = false;
                damageCollider.enabled = false;
                jumpStartPos = transform.position;
                jumpStartTime = Time.time;
                readyForStage2 = true;
                bossVisual.sortingOrder = 14;
                bossAnimator.Play("PH_BossJumpShadowAnimation");
                jumpSource.Play();
            }
        }
    }

    public void Stage2TakeDamage()
    {
        if (!invulnerable)
        {
            currentStage2HP = currentStage2HP - 1;
            invulnerableTimer = Time.time + invulnerableCD;
            invulnerable = true;
            invulFlashCounter = 0;
            projSpawner.BossDamaged();
            damageSource.Play();

            if (currentStage2HP <= 0)
            {
                jumpTarget = stage3StartPos;
                bossCollider.enabled = false;
                damageCollider.enabled = false;
                jumpStartPos = transform.position;
                jumpStartTime = Time.time;
                landed = false;
                movingToStage3 = true;
                currentState = 1;
                bossVisual.sortingOrder = 14;
                bossAnimator.Play("PH_BossJumpShadowAnimation");
                jumpSource.Play();
            }
        }
    }

    public void BossDeath()
    {
        currentStage = 4;
        FindObjectOfType<PlayerMovement>().currentState = 4;
        bossAnimator.Play("BossDeathAnimation");
        deathSounds[2].Play();
        deathSounds[0].Play();
        WorldSceneManager.LoadBreakRoom();
    }

    public void PlayerDead()
    {
        currentStage = 4;
    }
}
