using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public bool notTeleported;

    public int currentState;                        // Current possible states of the player: 0 = Moving, 1 = Pulled, 2 = Pulling, 3 = Ripping, 4 = Death
    public int currentHealth;                       // Health variables, changed on Start based on difficulty
    public int maxHealth;
    public float moveSpeed;
    public float invulnerableCD;                    // How long the player is invulnerable after getting hit
    public Transform hookSpawn;                     // Where the hook shot spawns
    public Transform playerFacing;                  // Mainly a rotation for hook spawn
    public GameObject hookPrefab;
    public GameObject heartContainer;               // Visuals for displaying HP
    public bool hookIsOut;                          // Is there an active hook out
    public SpriteRenderer playerSpriteRenderer;
    public Color defaultColor, damageColor;
    public Animator healthAnimator;
    public Animator playerAnimator;
    public Text debugText;                          // Throw stuff here to show during testing
    public GameObject EIndicator;
    public GameObject deathButtonsContainer;
    public Vector3 animateUp, animateDown, animateRight, animateLeft, animateUpRight, animateUpLeft, animateDownRight, animateDownLeft, animateIdle;        // Vectors for animation matching, clunky and dumb
    public Transform bossPos;
    public AudioSource stepsSource, takeDamageSource, hookLaunchSource;

    bool invulnerable;
    float invulnerableTimer;
    float hookCdTimer;
    int phFlashCounter;

    bool animatingHookLaunch;
    int framesSinceLaunch, hookLaunchFrameLimit = 10;

    float horizontalM, verticalM;                   // Horizontal and vertical movement Input value

    Vector3 mousePosition;
    Vector3 faceDirection;
    Vector3 previousFacing;
    Vector3 moveDirection;
    Vector3 preferDirection;

    GameObject currentHook;

    public GameObject cols;

    void Start()
    {
        currentState = 0;
        maxHealth = InitPlayerHealth();
        currentHealth = maxHealth;
        phFlashCounter = 0;
        hookCdTimer = Time.time;
        EIndicator.SetActive(false);
        preferDirection = animateDown;
    }

    void Update()
    {
        if (currentState != 4 && currentState != 3)
        {
            FaceMouseCursor();
        }

        if (currentState == 0)                      // Moving
        {
            
                moveDirection = NewMoveDirection();     // Check inputs for movement
            
            if (!animatingHookLaunch)               // Unless a hook is being launched, set the animation based on moveDirection
            {
                SetAnimation();
            }
            LimitMoveDirection();                   // Limit the diagonal speed of the player

            transform.position += moveDirection * moveSpeed * Time.deltaTime;       // Move player

            if (!hookIsOut && Input.GetMouseButtonDown(0) && Time.time >= hookCdTimer)
            {
                FireHook();
                hookCdTimer = Time.time + 0.5f;
                hookLaunchSource.Play();
            }
        }
        else if (currentState == 1)                 // Pulled, dont allow movement inputs
        {
            if (currentHook != null)                // If the hook is still active, move player towards the hook
            {
                transform.position = Vector3.MoveTowards(transform.position, currentHook.transform.position, (moveSpeed * 5) * Time.deltaTime);
            }
            else                                    // If the hook has returned and is null, return to Moving state
            {
                currentState = 0;
            }
        }
        else if (currentState == 2)                 // Pulling, stop movement inputs
        {
            if (currentHook != null)                // Same check for current hook as above
            {

            }
            else
            {
                currentState = 0;
            }
        }
        else if (currentState == 3)                 // Ripping, dont allow movement
        {

        }
        else if (currentState == 4)                 // Death
        {
            
        }

        if (invulnerable)                           // Show visual for invulnerability and check timer
        {
            if (phFlashCounter % 8 == 0)
            {
                playerSpriteRenderer.color = damageColor;
            }
            else
            {
                playerSpriteRenderer.color = defaultColor;
            }
            phFlashCounter++;

            if (Time.time >= invulnerableTimer)
            {
                invulnerable = false;
                playerSpriteRenderer.color = defaultColor;
            }
        }

        if (animatingHookLaunch)
        {
            if (framesSinceLaunch < hookLaunchFrameLimit)
            {
                framesSinceLaunch++;
            }
            else
            {
                animatingHookLaunch = false;
            }
        }
    }

    // Checks inputs and sets the movement direction for this frame
    private Vector3 NewMoveDirection()
    {
        horizontalM = 0f;
        verticalM = 0f;

        // If you want to use easier / better / unity default inputs, read them at the top here and store as either 1, 0 or -1 in horizontaM, and verticalM.
        // For analog input use a deadzone value, and if the sticks input exceeds that deadzone, convert the input to either -1 or 1. 
        // For example, pushing stick halfway to the right gives an input of 0.5, which would be over our deadzone value of 0.25, so we say our horizontalM = 1, not 0.5

        // Horizontal inputs
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            horizontalM = 0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalM = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalM = 1f;
        }

        // Vertical inputs
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            verticalM = 0f;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            verticalM = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalM = -1f;
        }

        if (horizontalM != 0 && verticalM == 0)
        {
            if (horizontalM == 1)
            {
                preferDirection = animateRight;
            }
            else
            {
                preferDirection = animateLeft;
            }
        }
        else if (horizontalM == 0 && verticalM != 0)
        {
            if (verticalM == 1)
            {
                preferDirection = animateUp;
            }
            else
            {
                preferDirection = animateDown;
            }
        }

        return new Vector3(horizontalM, verticalM, 0f);
    }

    // Limiting diagonal speed
    private void LimitMoveDirection()
    {
        if (moveDirection.x > 0.8f && moveDirection.y > 0.8f) // UpRight limit
        {
            moveDirection.x = 0.8f;
            moveDirection.y = 0.8f;
        }
        else if (moveDirection.x < -0.8f && moveDirection.y < -0.8f) // DownLeft limit
        {
            moveDirection.x = -0.8f;
            moveDirection.y = -0.8f;
        }
        else if (moveDirection.x < -0.8f && moveDirection.y > 0.8f) // UpLeft limit
        {
            moveDirection.x = -0.8f;
            moveDirection.y = 0.8f;
        }
        else if (moveDirection.x > 0.8f && moveDirection.y < -0.8f) // DownRight limit
        {
            moveDirection.x = 0.8f;
            moveDirection.y = -0.8f;
        }
    }

    // Setting correct player animation sprite based on the movement direction
    private void SetAnimation()
    {
        if (moveDirection == animateIdle)
        {
            if (previousFacing == animateDown)
            {
                playerAnimator.Play("PlayerIdleDown");
            }
            else if (previousFacing == animateUp)
            {
                playerAnimator.Play("PlayerIdleUp");
            }
            else if (previousFacing == animateRight)
            {
                playerAnimator.Play("PlayerIdleRight");
            }
            else if (previousFacing == animateLeft)
            {
                playerAnimator.Play("PlayerIdleLeft");
            }
            return;
        }
        else if (moveDirection == animateUpLeft)
        {
            if (preferDirection == animateUp)
            {
                playerAnimator.Play("PlayerRunUp");
                previousFacing = animateUp;
            }
            else if (preferDirection == animateLeft)
            {
                playerAnimator.Play("PlayerRunLeft");
                previousFacing = animateLeft;
            }
            else
            {
                playerAnimator.Play("PlayerRunLeft");
                previousFacing = animateLeft;
            }
            return;
        }
        else if (moveDirection == animateDownLeft)
        {
            if (preferDirection == animateDown)
            {
                playerAnimator.Play("PlayerRunDown");
                previousFacing = animateDown;
            }
            else if (preferDirection == animateLeft)
            {
                playerAnimator.Play("PlayerRunLeft");
                previousFacing = animateLeft;
            }
            else
            {
                playerAnimator.Play("PlayerRunLeft");
                previousFacing = animateLeft;
            }
            return;
        }
        else if (moveDirection == animateUpRight)
        {
            if (preferDirection == animateUp)
            {
                playerAnimator.Play("PlayerRunUp");
                previousFacing = animateUp;
            }
            else if (preferDirection == animateRight)
            {
                playerAnimator.Play("PlayerRunRight");
                previousFacing = animateRight;
            }
            else
            {
                playerAnimator.Play("PlayerRunRight");
                previousFacing = animateRight;
            }
            return;
        }
        else if (moveDirection == animateDownRight)
        {
            if (preferDirection == animateDown)
            {
                playerAnimator.Play("PlayerRunDown");
                previousFacing = animateDown;
            }
            else if (preferDirection == animateRight)
            {
                playerAnimator.Play("PlayerRunRight");
                previousFacing = animateRight;
            }
            else
            {
                playerAnimator.Play("PlayerRunRight");
                previousFacing = animateRight;
            }
            return;
        }
        else if (moveDirection == animateLeft)
        {
            playerAnimator.Play("PlayerRunLeft");
            previousFacing = animateLeft;
        }
        else if (moveDirection == animateRight)
        {
            playerAnimator.Play("PlayerRunRight");
            previousFacing = animateRight;
        }
        else if (moveDirection == animateUp)
        {
            playerAnimator.Play("PlayerRunUp");
            previousFacing = animateUp;
        }
        else if (moveDirection == animateDown)
        {
            playerAnimator.Play("PlayerRunDown");
            previousFacing = animateDown;
        }
    }

    // Fire hookshot
    private void FireHook()
    {
        hookIsOut = true;   // a new hook is spawned, preventing more from spawning before this one is done, save spawned hook to currentHook
        currentHook = Instantiate(hookPrefab, hookSpawn.position, hookSpawn.rotation, null);

        // Check the position of the mouse, needs a different implementation for analog stick
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));

        // Check where the mouse is in relation to the player character and plays the correct launch animation based on direction
        if ((Mathf.Max(mousePosition.y, transform.position.y) == mousePosition.y) && (Mathf.Abs(mousePosition.y - transform.position.y) > Mathf.Abs(mousePosition.x - transform.position.x)))
        {
            playerAnimator.Play("PlayerHookLaunchUp");
        }
        else if (Mathf.Abs(mousePosition.y - transform.position.y) > Mathf.Abs(mousePosition.x - transform.position.x))
        {
            playerAnimator.Play("PlayerHookLaunchDown");
        }
        else if (Mathf.Max(mousePosition.x, transform.position.x) == mousePosition.x)
        {
            playerAnimator.Play("PlayerHookLaunchRight");
        }
        else
        {
            playerAnimator.Play("PlayerHookLaunchLeft");
        }

        framesSinceLaunch = 0;
        animatingHookLaunch = true;
    }

    // The currently active hook returns and collides with player
    public void HookReturned()
    {
        hookIsOut = false;
     
            //cols.SetActive(true);
        
    }

    // Hook collides with a StaticBlock and changes player State
    public void StaticHooked()
    {
        currentState = 1;

      
            //cols.SetActive(false);
        
        // Check which direction the hook is, play correct directional animation based on it
        if ((Mathf.Max(currentHook.transform.position.y, transform.position.y) == currentHook.transform.position.y) && (Mathf.Abs(currentHook.transform.position.y - transform.position.y) > Mathf.Abs(currentHook.transform.position.x - transform.position.x)))
        {
            playerAnimator.Play("PlayerHookFlyUp");
        }
        else if (Mathf.Abs(currentHook.transform.position.y - transform.position.y) > Mathf.Abs(currentHook.transform.position.x - transform.position.x))
        {
            playerAnimator.Play("PlayerHookFlyDown");
        }
        else if (Mathf.Max(currentHook.transform.position.x, transform.position.x) == currentHook.transform.position.x)
        {
            playerAnimator.Play("PlayerHookFlyRight");
        }
        else
        {
            playerAnimator.Play("PlayerHookFlyLeft");
        }
    }

    // Hook collides with a PullBlock and changes player State
    public void PullHooked()
    {
        currentState = 2;

        // Check hook pos and play correct directional animation based on that
        if ((Mathf.Max(currentHook.transform.position.y, transform.position.y) == currentHook.transform.position.y) && (Mathf.Abs(currentHook.transform.position.y - transform.position.y) > Mathf.Abs(currentHook.transform.position.x - transform.position.x)))
        {
            playerAnimator.Play("PlayerHookPullUp");
        }
        else if (Mathf.Abs(currentHook.transform.position.y - transform.position.y) > Mathf.Abs(currentHook.transform.position.x - transform.position.x))
        {
            playerAnimator.Play("PlayerHookPullDown");
        }
        else if (Mathf.Max(currentHook.transform.position.x, transform.position.x) == currentHook.transform.position.x)
        {
            playerAnimator.Play("PlayerHookPullRight");
        }
        else
        {
            playerAnimator.Play("PlayerHookPullLeft");
        }
    }

    // Hook collides with a ripping block and changes player State
    public void StartRipping()
    {
        // Force the player to look up and play animation
        playerFacing.eulerAngles = new Vector3(0, 0, Mathf.Atan2((currentHook.transform.position.y - transform.position.y), (currentHook.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90);
        playerAnimator.Play("PlayerHookPullUp");
        EIndicator.SetActive(true); // Show the indicator for mashing E, change if needed (different controls etc.)
        currentState = 3;
    }

    public void EndRipping()
    {
        currentState = 0;
        EIndicator.SetActive(false);
    }

    // Used by boss1 to change player sprite sorting order to make player appear "behind" it. NEEDS DEBUGGING.
    public void SwitchSpriteLayer(int _newLayer)
    {
        if (_newLayer == 5)
        {
            playerSpriteRenderer.sortingOrder = 5;
        }
        else
        {
            playerSpriteRenderer.sortingOrder = 9;
        }
    }

    // Changes the invisible player facing based on mouse, for hookshot spawn direction
    private void FaceMouseCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));
        playerFacing.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg - 90);
    }

    // Called by other objects from EnterTrigger2D functions
    public void TakeDamage()
    {
        // If the player has not been hit in a set amount of time
        if (!invulnerable)
        {
            invulnerableTimer = Time.time + invulnerableCD;
            invulnerable = true;
            phFlashCounter = 0;
            currentHealth = currentHealth - 1;
            if (currentState != 3)  // Knocks the player out of other states when taking damage
            {
                currentState = 0;
            }
            EIndicator.SetActive(false);
            takeDamageSource.Play();

            if (currentHook != null)
            {
                currentHook.GetComponent<HookScript>().PlayerDamaged();
            }

            // Show heart animations based on HP

            if (currentHealth == 4)
            {
                healthAnimator.Play("PH_HeartAnimation5");
            }
            else if (currentHealth == 3)
            {
                healthAnimator.Play("PH_HeartAnimation4");
            }
            else if (currentHealth == 2)
            {
                healthAnimator.Play("PH_HeartAnimation1");
            }
            else if (currentHealth == 1)
            {
                healthAnimator.Play("PH_HeartAnimation2");
            }
            else if (currentHealth == 0)
            {
                healthAnimator.Play("PH_HeartAnimation3");
            }

            if (currentHealth <= 0)
            {
                // Player death handling / animation / reset
                //debugText.text = "";
                PlayDeathAnimation();
                if (currentHook != null)
                {
                    Destroy(currentHook);
                }
                if (FindObjectOfType<BossScript>()!=null)
                {
                    FindObjectOfType<BossScript>().PlayerDead();
                }
                
                GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManagerScript>().SwitchToDeathTrack();
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(ShowDeathButtons());
                return;
            }
        }
    }

    private IEnumerator ShowDeathButtons()
    {
        currentState = 4;
        yield return new WaitForSeconds(2.8f);
        deathButtonsContainer.SetActive(true);
    }

    public void Victory()
    {
        playerAnimator.Play("PlayerIdleDown");
        currentState = 4;
    }

    // Check the selected difficulty (defaults to normal), change HP and visuals based on it
    public int InitPlayerHealth()
    {
        int difficulty = FindObjectOfType<GameSettingsScript>().GetDifficulty();

        if (difficulty == 1) // Easy
        {
            return 5;
        }
        else if (difficulty == 2) // Normal
        {
            heartContainer.transform.position = new Vector3(heartContainer.transform.position.x + 0.2f, heartContainer.transform.position.y, heartContainer.transform.position.z);
            return 3;
        }
        else if (difficulty == 3) // Hard
        {
            heartContainer.transform.position = new Vector3(heartContainer.transform.position.x + 0.4f, heartContainer.transform.position.y, heartContainer.transform.position.z);
            return 1;
        }

        return 3;
    }

    private void PlayDeathAnimation()
    {
        if (preferDirection == animateUp)
        {
            playerAnimator.Play("PlayerDeathUpAnimation");
        }
        else if (preferDirection == animateLeft)
        {
            playerAnimator.Play("PlayerDeathLeftAnimation");
        }
        else if (preferDirection == animateRight)
        {
            playerAnimator.Play("PlayerDeathRightAnimation");
        }
        else
        {
            playerAnimator.Play("PlayerDeathDownAnimation");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("other name: " + collision);
    }

    

}
