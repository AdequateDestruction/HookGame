using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    // Used to spawn prefab shot types based on arrays of transforms. The arrays are customized in the editor for position / rotation.

    public int currentStage;
    public float shotCooldown;
    public GameObject basicShotPrefab, slowShotPrefab, tennisShotPrefab, splitterShotPrefab, laserShotPrefab, shortRangeShotPrefab;
    public Transform[] pattern1Transforms, pattern2Transforms, pattern3Transforms, pattern4Transforms, pattern5Transforms, pattern6Transforms, pattern7Transforms, pattern8Transforms;
    public Transform[] stage3Pattern1, stage3Pattern2, stage3Pattern3, stage3Pattern4, stage3Pattern5, stage3Pattern6, stage3Pattern7;
    public Transform[] tennisShotPattern;
    public Transform[] laserShotPattern, finalLaserShotPattern;
    public Transform[] stage2SplitterPatternTop, stage2SplitterPatternBot;

    float shotTimer;
    int currentPattern;
    int pongRequirement;
    public int currentBossHP;
    bool bossVulnerable;
    GameObject currentTennisShot = null;

    void Start()
    {
        currentStage = 1;
        currentPattern = 0;
        pongRequirement = 0;
        shotTimer = Time.time + 5f;
        currentBossHP = 3;
    }

    void Update()
    {
        if (currentStage == 1)
        {
            if (Time.time >= shotTimer)
            {
                shotTimer = Time.time + shotCooldown;
                FireStage1Shot(currentPattern);
            }
        }
        else if (currentStage == 2)
        {
            if (Time.time >= shotTimer)
            {
                shotTimer = Time.time + shotCooldown;
                FireStage2Shot(currentPattern);
            }
        }
        else if (currentStage == 3)
        {
            if (!bossVulnerable)
            {
                if (Time.time >= shotTimer)
                {
                    shotTimer = Time.time + shotCooldown;
                    FireStage3Shot(currentPattern);
                }
            }
            else
            {
                if (Time.time >= shotTimer)
                {
                    shotTimer = Time.time + shotCooldown / 3f;
                    FireStage3VulnerableShot();
                }
            }
        }
        else if (currentStage == 4)
        {

        }
    }

    private void FireStage1Shot(int _pattern)
    {
        if (currentBossHP == 3)
        {
            if (_pattern == 0)
            {
                foreach (Transform spawnTransform in pattern1Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 1)
            {
                foreach (Transform spawnTransform in pattern2Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 2)
            {
                foreach (Transform spawnTransform in pattern1Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 3)
            {
                foreach (Transform spawnTransform in pattern2Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
        }
        else if (currentBossHP == 2)
        {
            if (_pattern == 0)
            {
                foreach (Transform spawnTransform in pattern1Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 1)
            {
                foreach (Transform spawnTransform in pattern2Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 2)
            {
                foreach (Transform spawnTransform in pattern3Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 3)
            {
                foreach (Transform spawnTransform in pattern4Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
        }
        else
        {
            if (_pattern == 0)
            {
                foreach (Transform spawnTransform in pattern3Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 1)
            {
                foreach (Transform spawnTransform in pattern4Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 2)
            {
                foreach (Transform spawnTransform in pattern3Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
                foreach (Transform spawnTransform in pattern4Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 3)
            {
                foreach (Transform spawnTransform in pattern3Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
                foreach (Transform spawnTransform in pattern4Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
        }

        currentPattern++;
        if (currentPattern > 3)
        {
            currentPattern = 0;
        }
    }

    private void FireStage2Shot(int _pattern)
    {
        if (currentBossHP == 3)
        {
            if (_pattern == 0)
            {
                foreach (Transform spawnTransform in pattern5Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 1)
            {
                foreach (Transform spawnTransform in pattern6Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 2)
            {
                foreach (Transform spawnTransform in pattern8Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 3)
            {
                foreach (Transform spawnTransform in pattern7Transforms)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
        }
        else if (currentBossHP == 2)
        {
            if (_pattern == 0)
            {
                foreach (Transform spawnTransform in stage2SplitterPatternBot)
                {
                    Instantiate(splitterShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 1)
            {
                foreach (Transform spawnTransform in pattern2Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 2)
            {
                
            }
            else if (_pattern == 3)
            {
                foreach (Transform spawnTransform in pattern6Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
        }
        else
        {
            if (_pattern == 0)
            {
                foreach (Transform spawnTransform in stage2SplitterPatternTop)
                {
                    Instantiate(splitterShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
                shotTimer = shotTimer + 2.5f;
            }
            else if (_pattern == 1)
            {
                foreach (Transform spawnTransform in pattern7Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 2)
            {
                foreach (Transform spawnTransform in stage2SplitterPatternBot)
                {
                    Instantiate(splitterShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
                shotTimer = shotTimer + 2.5f;
            }
            else if (_pattern == 3)
            {
                foreach (Transform spawnTransform in pattern8Transforms)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
        }

        currentPattern++;
        if (currentPattern > 3)
        {
            currentPattern = 0;
        }
    }

    private void FireStage3Shot(int _pattern)
    {
        if (currentBossHP == 3)
        {
            if (_pattern == 0)
            {
                foreach (Transform spawnTransform in stage3Pattern1)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 1)
            {
                foreach (Transform spawnTransform in stage3Pattern2)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 2)
            {
                foreach (Transform spawnTransform in stage3Pattern1)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 3)
            {
                foreach (Transform spawnTransform in stage3Pattern5)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }

                foreach (Transform spawnTransform in stage3Pattern4)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }

                if (currentTennisShot == null)
                {
                    int tennisShotIndex = Random.Range(0, tennisShotPattern.Length - 1);
                    currentTennisShot = Instantiate(tennisShotPrefab, tennisShotPattern[tennisShotIndex].position, tennisShotPattern[tennisShotIndex].rotation);
                    currentTennisShot.GetComponent<TennisShotScript>().SetPongRequirement(pongRequirement);
                    shotTimer = Time.time + shotCooldown + 2f;
                }
            }
            else
            {
                foreach (Transform spawnTransform in stage3Pattern1)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
        }
        else if (currentBossHP == 2)
        {
            if (_pattern == 0)
            {
                foreach (Transform spawnTransform in stage3Pattern7)
                {
                    Instantiate(splitterShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 1)
            {
                foreach (Transform spawnTransform in stage3Pattern2)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 2)
            {
                Instantiate(laserShotPrefab, laserShotPattern[0].position, laserShotPattern[0].rotation);
                foreach (Transform spawnTransform in stage3Pattern1)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 3)
            {
                Instantiate(laserShotPrefab, laserShotPattern[1].position, laserShotPattern[1].rotation);
                foreach (Transform spawnTransform in stage3Pattern6)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else
            {
                foreach (Transform spawnTransform in stage3Pattern6)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }

                foreach (Transform spawnTransform in stage3Pattern1)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }

                if (currentTennisShot == null)
                {
                    int tennisShotIndex = Random.Range(0, tennisShotPattern.Length - 1);
                    currentTennisShot = Instantiate(tennisShotPrefab, tennisShotPattern[tennisShotIndex].position, tennisShotPattern[tennisShotIndex].rotation);
                    currentTennisShot.GetComponent<TennisShotScript>().SetPongRequirement(pongRequirement);
                    shotTimer = Time.time + shotCooldown + 2f;
                }
            }
        }
        else
        {
            if (_pattern == 0)
            {
                StartCoroutine(FinalLaserRoutine());

                foreach (Transform spawnTransform in stage3Pattern6)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }
            }
            else if (_pattern == 1)
            {

            }
            else if (_pattern == 2)
            {
                
            }
            else if (_pattern == 3)
            {
                
            }
            else if (_pattern == 4)
            {
                foreach (Transform spawnTransform in stage3Pattern1)
                {
                    Instantiate(basicShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }

                foreach (Transform spawnTransform in stage3Pattern4)
                {
                    Instantiate(slowShotPrefab, spawnTransform.position, spawnTransform.rotation);
                }

                if (currentTennisShot == null)
                {
                    int tennisShotIndex = Random.Range(0, tennisShotPattern.Length - 1);
                    currentTennisShot = Instantiate(tennisShotPrefab, tennisShotPattern[tennisShotIndex].position, tennisShotPattern[tennisShotIndex].rotation);
                    currentTennisShot.GetComponent<TennisShotScript>().SetPongRequirement(pongRequirement);
                    shotTimer = Time.time + shotCooldown + 2f;
                }
            }
        }

        currentPattern++;
        if (currentPattern > 4)
        {
            currentPattern = 0;
        }
    }

    private IEnumerator FinalLaserRoutine()
    {
        Instantiate(laserShotPrefab, finalLaserShotPattern[1].position, finalLaserShotPattern[1].rotation);
        yield return new WaitForSeconds(1.3f);
        Instantiate(laserShotPrefab, finalLaserShotPattern[3].position, finalLaserShotPattern[3].rotation);
        yield return new WaitForSeconds(1.3f);
        Instantiate(laserShotPrefab, finalLaserShotPattern[4].position, finalLaserShotPattern[4].rotation);
        Instantiate(laserShotPrefab, finalLaserShotPattern[2].position, finalLaserShotPattern[2].rotation);
        yield return new WaitForSeconds(1.3f);
        Instantiate(laserShotPrefab, finalLaserShotPattern[0].position, finalLaserShotPattern[0].rotation);
        yield return new WaitForSeconds(1.3f);
        Instantiate(laserShotPrefab, finalLaserShotPattern[6].position, finalLaserShotPattern[6].rotation);
    }

    private void FireStage3VulnerableShot()
    {
        foreach (Transform spawnTransform in stage3Pattern4)
        {
            Instantiate(shortRangeShotPrefab, spawnTransform.position, spawnTransform.rotation);
        }
    }

    public void TransitionToStage2()
    {
        shotCooldown = shotCooldown * 0.5f;
        shotTimer = Time.time + shotCooldown + 3f;
        currentStage = 2;
        currentPattern = 0;
        currentBossHP = 3;
    }

    public void TransitionToStage3()
    {
        shotCooldown = shotCooldown * 2f;
        shotTimer = Time.time + shotCooldown + 8f;
        currentStage = 3;
        currentPattern = 0;
        currentBossHP = 4;
    }

    public void SetVictoryStage()
    {
        currentStage = 4;
    }

    public void BossVulnerable()
    {
        bossVulnerable = true;
        currentPattern = 0;
        StopAllCoroutines();
    }

    public void BossNotVulnerable()
    {
        bossVulnerable = false;
        currentPattern = 0;
        shotTimer = Time.time + shotCooldown;
    }

    public void TennisShotDestroyed()
    {
        currentTennisShot = null;
    }

    public void BossArmorPieceRipped()
    {
        pongRequirement = pongRequirement + 2;
        BossDamaged();
    }

    public void BossDamaged()
    {
        currentBossHP = currentBossHP - 1;
    }
}
