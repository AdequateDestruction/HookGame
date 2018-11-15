using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int currentStage;
    public int stage3RippedPieces;
    public float trapCooldown;
    public float tentacleCooldown;
    public ElecFloorScript[] allTraps;
    public PHCameraScript cameraScript;
    public GameObject stage2PullBlockPrefab;
    public GameObject stage2TentaclePrefab;
    public GameObject bigElecEffectPrefab;
    public GameObject topDestroyMushroom;
    public GameObject deathButtonsContainer;
    public Text debugText;
    public Vector3 pullBlockSpawn1, pullBlockSpawn2, pullBlockSpawn3, pullBlockSpawn4;
    public Animator botRightButtonAnimator, leftButtonAnimator;
    public ButtonScript botRightButton, leftButton;
    public ProjectileSpawner pSpawner;
    public CoreScript[] ripPieces;
    public FloorDisplayScript floorDisplayScript;
    public Transform leftTrapTrans, rightTrapTrans;
    public BoxCollider2D[] tentacleBlockColliders;
    public BoxCollider2D stage3SafetyBlock;
    public AudioSource trapWarningSource, pullBlockSpawnSource;

    int stg2ButtonActivations;
    float nextTrapSetTimer;
    float nextTentacleTimer;
    bool completedStage2;
    Transform playerTransform;
    List<GameObject> tentacleList = new List<GameObject>();
    MusicManagerScript musicManager;

    void Start()
    {
        currentStage = 1;
        stg2ButtonActivations = 0;
        nextTrapSetTimer = Time.time + trapCooldown - 1f;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        musicManager = FindObjectOfType<MusicManagerScript>();
        musicManager.StartStage1Music();
    }

    void Update()
    {
        if (currentStage == 1)
        {
            if (Time.time >= nextTrapSetTimer)
            {
                ActivateTraps();
                nextTrapSetTimer = Time.time + trapCooldown;
            }
        }
        else if (currentStage == 2)
        {
            if (Time.time >= nextTentacleTimer)
            {
                SpawnTentacle();
                nextTentacleTimer = Time.time + tentacleCooldown;
            }

            if (!completedStage2 && stg2ButtonActivations == 2 && botRightButton.buttonActive && leftButton.buttonActive)
            {
                completedStage2 = true;
                StartCoroutine(Stage3TransitionRoutine());
                currentStage = 3;
            }
            else if (stg2ButtonActivations == 1 && leftButton.buttonActive)
            {
                StartCoroutine(Stage2SecondButtonsComplete());
                stg2ButtonActivations++;
            }
            else if (stg2ButtonActivations == 0 && botRightButton.buttonActive)
            {
                StartCoroutine(Stage2FirstButtonsComplete());
                stg2ButtonActivations++;
            }
        }
        else if (currentStage == 3)
        {

        }
    }

    private void ActivateTraps()
    {
        foreach (ElecFloorScript trap in allTraps)
        {
            trap.ActivateTrap();
        }

        StartCoroutine(TrapWarningSFX());
    }

    public void SpawnTentacle()
    {
        int maxValidTries = 20;
        int currentTries = 0;
        bool validSpawnPos = false;
        Vector3 playerPos = playerTransform.position;
        Vector2 tentacleSpawnPos = playerPos;
        
        while (!validSpawnPos)
        {
            currentTries++;
            validSpawnPos = true;
            playerPos = playerTransform.position;
            tentacleSpawnPos = playerPos;
            tentacleSpawnPos = tentacleSpawnPos + Random.insideUnitCircle * 2f;
            
            foreach (BoxCollider2D collider in tentacleBlockColliders)
            {
                if (collider.bounds.Contains(tentacleSpawnPos))
                {
                    validSpawnPos = false;
                }
            }
            
            if (currentTries > maxValidTries)
            {
                break;
            }
        }

        GameObject go = Instantiate(stage2TentaclePrefab, tentacleSpawnPos, new Quaternion(0, 0, 0, 0));
        tentacleList.Add(go);
    }

    private IEnumerator TrapWarningSFX()
    {
        trapWarningSource.loop = true;
        trapWarningSource.Play();
        yield return new WaitForSeconds(2.7f);
        trapWarningSource.loop = false;
    }

    private IEnumerator Stage2TransitionRoutine()
    {
        pSpawner.TransitionToStage2();
        cameraScript.Stage2StartCameraZoom();
        //debugText.text = "Starting Stage 2";
        yield return new WaitForSeconds(2);
        //debugText.text = "Spawning Pull Blocks";
        cameraScript.StartCameraShake();
        SpawnFirstPullBlocks();
        pullBlockSpawnSource.Play();
        yield return new WaitForSeconds(2);
        //debugText.text = "";
    }

    private IEnumerator Stage3TransitionRoutine()
    {
        pSpawner.TransitionToStage3();
        //debugText.text = "Ending Stage2";
        yield return new WaitForSeconds(2f);
        botRightButton.DeactivateButton();
        leftButton.DeactivateButton();
        floorDisplayScript.DeActivateLight("botRightButton");
        floorDisplayScript.DeActivateLight("leftButton");
        cameraScript.Stage3CameraZoom();
        cameraScript.Stage3StartCameraMove();
        yield return new WaitForSeconds(2f);
        Instantiate(bigElecEffectPrefab, new Vector3(0, 0.9f, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(0.8f);
        DestroyTentacles();
        yield return new WaitForSeconds(0.8f);
        topDestroyMushroom.SetActive(false);
        yield return new WaitForSeconds(1.2f);
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManagerScript>().SwitchToStage3Track();
        cameraScript.Stage3EndCameraMove();
        //debugText.text = "";
        yield return new WaitForSeconds(0.2f);
        pSpawner.BossNotVulnerable();
        foreach (CoreScript ripPiece in ripPieces)
        {
            ripPiece.ShowRipObject();
        }
        stage3SafetyBlock.enabled = true;
    }

    private IEnumerator Stage2FirstButtonsComplete()
    {
        yield return new WaitForSeconds(1f);
        floorDisplayScript.DeActivateLight("botRightButton");
        yield return new WaitForSeconds(1f);
        cameraScript.Stage2StartCameraMove();
        yield return new WaitForSeconds(1f);
        botRightButton.DeactivateButton();
        leftButton.DeactivateButton();
        Instantiate(bigElecEffectPrefab, new Vector3(0, 0.9f, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(2f);
        cameraScript.Stage3EndCameraMove();
        SpawnSecondPullBlocks();
        pullBlockSpawnSource.Play();
    }

    private IEnumerator Stage2SecondButtonsComplete()
    {
        yield return new WaitForSeconds(1f);
        floorDisplayScript.DeActivateLight("leftButton");
        yield return new WaitForSeconds(1f);
        cameraScript.Stage2StartCameraMove();
        yield return new WaitForSeconds(1f);
        botRightButton.DeactivateButton();
        leftButton.DeactivateButton();
        Instantiate(bigElecEffectPrefab, new Vector3(0, 0.9f, 0), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(2f);
        cameraScript.Stage3EndCameraMove();
        SpawnThirdPullBlocks();
        pullBlockSpawnSource.Play();
    }

    private void DestroyTentacles()
    {
        if (tentacleList.Count > 0)
        {
            foreach (GameObject tentacle in tentacleList)
            {
                if (tentacle != null)
                {
                    tentacle.GetComponent<TentacleScript>().DestroyTentacle();
                }
            }
        }
        tentacleList.Clear();
    }

    private void SpawnFirstPullBlocks()
    {
        floorDisplayScript.ActivateLight("botRightButton");

        GameObject go = Instantiate(stage2PullBlockPrefab, pullBlockSpawn1, new Quaternion(0, 0, 0, 0));
        Vector3 falltoLocation = new Vector3(pullBlockSpawn1.x, pullBlockSpawn1.y - 6, pullBlockSpawn1.z);
        go.GetComponent<PullBlockScript>().FallFromCeiling(falltoLocation);

        botRightButton.ActivateButton();
    }

    private void SpawnSecondPullBlocks()
    {
        cameraScript.StartCameraShake();
        floorDisplayScript.ActivateLight("leftButton");

        GameObject go = Instantiate(stage2PullBlockPrefab, pullBlockSpawn2, new Quaternion(0, 0, 0, 0));
        Vector3 falltoLocation = new Vector3(pullBlockSpawn2.x, pullBlockSpawn2.y - 6, pullBlockSpawn2.z);
        go.GetComponent<PullBlockScript>().FallFromCeiling(falltoLocation);
        
        leftButton.ActivateButton();
    }

    private void SpawnThirdPullBlocks()
    {
        cameraScript.StartCameraShake();
        floorDisplayScript.ActivateLight("botRightButton");
        floorDisplayScript.ActivateLight("leftButton");

        GameObject go = Instantiate(stage2PullBlockPrefab, pullBlockSpawn3, new Quaternion(0, 0, 0, 0));
        Vector3 falltoLocation = new Vector3(pullBlockSpawn3.x, pullBlockSpawn3.y - 6, pullBlockSpawn3.z);
        go.GetComponent<PullBlockScript>().FallFromCeiling(falltoLocation);

        go = Instantiate(stage2PullBlockPrefab, pullBlockSpawn4, new Quaternion(0, 0, 0, 0));
        falltoLocation = new Vector3(pullBlockSpawn4.x, pullBlockSpawn4.y - 6, pullBlockSpawn4.z);
        go.GetComponent<PullBlockScript>().FallFromCeiling(falltoLocation);

        botRightButton.ActivateButton();
        leftButton.ActivateButton();
    }

    public void ExposeArmorPiece()
    {
        pSpawner.BossVulnerable();

        if (stage3RippedPieces < ripPieces.Length)
        {
            ripPieces[stage3RippedPieces].ExposeRipCollider();
        }
    }

    public void ExposedArmorRipped()
    {
        stage3RippedPieces++;
        pSpawner.BossArmorPieceRipped();
    }

    public void RipTimeOut()
    {
        ripPieces[stage3RippedPieces].CoverRipColliders();
    }

    public void TransitionToStage2()
    {
        foreach (ElecFloorScript trap in allTraps)
        {
            trap.DeactivateTrap();
        }

        StartCoroutine(Stage2TransitionRoutine());
        nextTentacleTimer = Time.time + tentacleCooldown + 4f;
        currentStage = 2;
    }

    public void DeathButtons(int _index)
    {
        if (_index == 0) // Restart level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (_index == 1) // Return to Main Menu
        {
            SceneManager.LoadScene(0);
            //WorldSceneManager.NextScene();
        }
    }

    public void StartVictoryState()
    {
        currentStage = 4;
        StartCoroutine(VictoryRoutine());
    }

    private IEnumerator VictoryRoutine()
    {
        yield return new WaitForSeconds(4f);
        deathButtonsContainer.SetActive(true);
    }
}
