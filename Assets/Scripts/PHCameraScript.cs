using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHCameraScript : MonoBehaviour
{
    public float camMoveSpeed;
    public float shakeDuration;
    public float shakeAmount;
    public float decreaseFactor;

    bool shakeCamera;
    bool stage3Transition;
    bool inStage3;
    float originalShakeDuration;
    Transform playerTransform;
    Vector3 targetTransform;
    Vector3 shakeOriginalPos;
    Vector3 stage3TransitionPos = new Vector3(0f, 0f, -5f);
    Animator cameraAnimator;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        originalShakeDuration = shakeDuration;
        cameraAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!shakeCamera && stage3Transition)
        {
            if (gameObject.transform.position != stage3TransitionPos)
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, stage3TransitionPos, camMoveSpeed * Time.deltaTime);
            }
        }
        else if (!shakeCamera)
        {
            targetTransform = playerTransform.position;
            targetTransform.z = -5f;
            if (inStage3)
            {
                targetTransform.y = targetTransform.y + 2f;
            }
            gameObject.transform.position = Vector3.MoveTowards(transform.position, targetTransform, camMoveSpeed * Time.deltaTime);
        }
        else
        {
            if (shakeDuration > 0)
            {
                transform.localPosition = shakeOriginalPos + Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                transform.localPosition = shakeOriginalPos;
                shakeCamera = false;
            }
        }
    }

    public void StartCameraShake()
    {
        shakeOriginalPos = transform.localPosition;
        shakeDuration = originalShakeDuration;
        shakeCamera = true;
    }

    public void Stage2StartCameraZoom()
    {
        cameraAnimator.Play("PH_CameraZoomAnimation");
    }

    public void Stage3CameraZoom()
    {
        cameraAnimator.Play("PH_CameraZoomAnimation2");
    }

    public void Stage3StartCameraMove()
    {
        stage3Transition = true;
        inStage3 = true;
    }

    public void Stage2StartCameraMove()
    {
        stage3Transition = true;
        cameraAnimator.Play("CameraStage2BossHurtZoomAnimation");
    }

    public void Stage3EndCameraMove()
    {
        stage3Transition = false;
    }

}
