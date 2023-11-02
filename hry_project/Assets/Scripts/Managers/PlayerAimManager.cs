using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerAimManager : MonoBehaviour
{
    // Objects
    [SerializeField] private GameObject lookAtObject;
    private CharacterController lookAtObjectController;
    private Animator animator;

    private bool showTarget;

    private Vector2 currentAimInput;
    private Vector3 currentAimMovement;
    private float viewAngle;
    [SerializeField] private bool lockOnTarget;

    private Vector3 aimVector;
    private Vector3 rotationVector;
    private Vector3 defaultRotationVector;
    [SerializeField] private float minDegreeY;
    [SerializeField] private float maxDegreeY;
    [SerializeField] private float minDegreeZ;
    [SerializeField] private float maxDegreeZ;
    [Range (1.0f, 20.0f)]
    public float aimMovementSpeed;

    void Start()
    {
        lookAtObjectController = lookAtObject.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        showTarget = false;
        lockOnTarget = false;

        defaultRotationVector.x = animator.GetBoneTransform(HumanBodyBones.Head).localRotation.x;
        defaultRotationVector.y = animator.GetBoneTransform(HumanBodyBones.Head).localRotation.y;
        defaultRotationVector.z = animator.GetBoneTransform(HumanBodyBones.Head).localRotation.z;

        InputManager.instance.controls.PlayerControls.HeadAim.started += onHeadAim;
        InputManager.instance.controls.PlayerControls.HeadAim.performed += onHeadAim;
        InputManager.instance.controls.PlayerControls.HeadAim.canceled += onHeadAim;

        InputManager.instance.controls.PlayerControls.HeadAimDepth.started += onHeadDepthAim;
        InputManager.instance.controls.PlayerControls.HeadAimDepth.performed += onHeadDepthAim;
        InputManager.instance.controls.PlayerControls.HeadAimDepth.canceled += onHeadDepthAim;

        InputManager.instance.controls.PlayerControls.HideTarget.performed += onAimHide;
    }

    void FixedUpdate()
    {
        if (showTarget)
        {
            lookAtObjectController.Move(currentAimMovement * Time.deltaTime * aimMovementSpeed);
            HandleHeadAngle();
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (lockOnTarget)
        {
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(lookAtObject.transform.position);
        }
    }

    private void HandleHeadAngle() {
        viewAngle = Vector3.Angle(transform.forward, lookAtObject.transform.position - animator.GetBoneTransform(HumanBodyBones.Head).position);
        if (viewAngle >= minDegreeY && viewAngle <= maxDegreeY)
        {
            lockOnTarget = true;
        }
        else
        {
            lockOnTarget = false;
        }
    }

    private void onHeadAim(InputAction.CallbackContext context)
    {
        currentAimInput = context.ReadValue<Vector2>();
        currentAimMovement.x = currentAimInput.x;
        currentAimMovement.z = currentAimInput.y;
    }

    private void onHeadDepthAim(InputAction.CallbackContext context)
    {
        currentAimMovement.y = context.ReadValue<float>();
    }

    private void onAimHide(InputAction.CallbackContext context)
    {
        showTarget = !showTarget;
        if (lockOnTarget && !showTarget)
        {
            lockOnTarget = false;
        }
        lookAtObject.SetActive(showTarget);
        RumbleManager.instance.RumblePulse(0.25f, 0.75f, 0.5f);
    }
}