using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovementManager : MonoBehaviour
{    
    // Objects
    private CharacterController characterController;
    private Animator animator;

    // Controls
    private String currentControlScheme;
    [SerializeField] private bool lockControls;

    // Animator connected stuff
    private int isWalkingHash;
    private int isRunningHash;
    private int walkingAnimationMultiplierHash;
    private float walkingAnimationMultiplier;
    private int hasJumpedHash;
    private int isFallingHash;
    private int isPushingHash;
    private int pickUpHash;

    // Movement connected stuff
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private float errorRate;
    [SerializeField] private float velocityValue;
    [SerializeField] private float runSpeedMultiplier;
    private bool isMovementPressed;
    private bool isRunPressed;
    private Vector3 positionToLookAt;
    private Quaternion targetRotation;
    [SerializeField] private float rotationFactorPerFrame = 7.0f;

    // Jump
    private bool isJumpPressed;
    private bool hasJumped;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float jumpDuration = 1.0f;
    [SerializeField] private float gravity = 9.8f;
    private float distanceToGround;
    [SerializeField] private float groundedDistanceTrigger = .1f;

    private bool isClimbing;
    private GameObject objectToClimbAt;
    [SerializeField] private float distanceToWall;

    [SerializeField] private String[] animationsLockingControls;
    [SerializeField] private String[] transitionsLockingControls;
    private int[] transitionsLockingControlsHashes;

    // Falling
    private bool isFalling;

    private bool isPushingPressed;
    private bool isPushing;
    private GameObject objectToPush;
    [SerializeField] private float pushDistance;
    [SerializeField] private float pushForceMagnitude;

    private bool pickUpPressed;
    private bool isPickingUp;
    private int startPickingUp;
    private bool holdObject;
    [SerializeField] private GameObject objectToPickUp;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        lockControls = false;
        holdObject = false;

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        walkingAnimationMultiplierHash = Animator.StringToHash("walkingAnimationMultiplier");
        hasJumpedHash = Animator.StringToHash("hasJumped");
        isFallingHash = Animator.StringToHash("isFalling");
        isPushingHash = Animator.StringToHash("isPushing");
        pickUpHash = Animator.StringToHash("pickUp");

        transitionsLockingControlsHashes = new int[transitionsLockingControls.Length];
        for (int transitionIndex = 0; transitionIndex < transitionsLockingControls.Length; transitionIndex++)
        {
            transitionsLockingControlsHashes[transitionIndex] = Animator.StringToHash(transitionsLockingControls[transitionIndex]);
        }

        errorRate = .9f;

        currentControlScheme = InputManager.instance.playerInput.currentControlScheme;
        InputManager.instance.playerInput.onControlsChanged += SwitchControlsCallbackFunction;

        InputManager.instance.controls.PlayerControls.Movement.started += MovementCallbackFunction;
        InputManager.instance.controls.PlayerControls.Movement.canceled += MovementCallbackFunction;
        InputManager.instance.controls.PlayerControls.Movement.performed += MovementCallbackFunction;

        InputManager.instance.controls.PlayerControls.Running.started += RunningCallbackFunction;
        InputManager.instance.controls.PlayerControls.Running.canceled += RunningCallbackFunction;

        InputManager.instance.controls.PlayerControls.Jump.started += JumpCallbackFunction;
        InputManager.instance.controls.PlayerControls.Jump.canceled += JumpCallbackFunction;

        InputManager.instance.controls.PlayerControls.Push.started += PushCallbackFunction;
        InputManager.instance.controls.PlayerControls.Push.canceled += PushCallbackFunction;

        InputManager.instance.controls.PlayerControls.PickUp.started += PickUpCallbackFunction;
        InputManager.instance.controls.PlayerControls.PickUp.canceled += PickUpCallbackFunction;
    }

    private void FixedUpdate()
    {
        HandleGravity();
        RaycastForward();
        HandleJump();
        HandleControlsLocking();
        HandleObjectsPushMovement();
        characterController.Move(currentMovement * Time.deltaTime);
        HandlePosition();
    }

    private void Update()
    {
        HandleAnimation();
    }

    private void LateUpdate()
    {
        HandleRotation();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (isPushing)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            Ray leftHandRay = new Ray(animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).position, transform.forward);
            RaycastHit leftHandRayHit;
            Ray rightHandRay = new Ray(animator.GetBoneTransform(HumanBodyBones.RightUpperArm).position, transform.forward);
            RaycastHit rightHandRayHit;
            Physics.Raycast(leftHandRay, out leftHandRayHit);
            Physics.Raycast(rightHandRay, out rightHandRayHit);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandRayHit.point);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandRayHit.point);
        }
        else if (holdObject)
        {
            if (startPickingUp > 1) {
                startPickingUp--;
                animator.SetBool(pickUpHash, true);
            }
            else if (startPickingUp == 1) 
            {
                startPickingUp--;
                objectToPickUp.transform.SetPositionAndRotation(animator.GetBoneTransform(HumanBodyBones.RightLittleProximal).position, objectToPickUp.transform.GetChild(0).rotation);
                objectToPickUp.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.RightLittleProximal));
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, objectToPickUp.transform.GetChild(0).transform.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, objectToPickUp.transform.GetChild(0).transform.rotation);
            } else {
                animator.SetBool(pickUpHash, false);
            }
        }
        else if (!isPushing && !holdObject)
        {
            startPickingUp = 60;
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        }
    }

    private void RaycastForward()
    {
        Ray ray = new Ray(animator.GetBoneTransform(HumanBodyBones.Chest).position, transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 2f);
        if (hit.collider)
        {
            if (hit.collider.gameObject.tag == "ObjectToPush" && hit.distance <= pushDistance)
            {
                if (isPushingPressed)
                {
                    isPushing = true;
                    objectToPush = hit.collider.gameObject;
                    animator.SetBool(isPushingHash, isPushing);
                }
            }
            else if (hit.collider.gameObject.tag == "Wall" && hit.distance <= distanceToWall)
            {
                if (isFalling)
                {
                    // isClimbing = true;
                    objectToClimbAt = hit.collider.gameObject;
                    animator.Play("Climbing");
                }
            }
        }   
    }

    private void HandleObjectsPushMovement()
    {
        if (isPushing & objectToPush)
        {
            Vector3 forceVector = currentMovement;
            forceVector.y = 0.0f;
            forceVector.z = 0.0f;
            objectToPush.GetComponent<Rigidbody>().AddForce(forceVector * pushForceMagnitude, ForceMode.Impulse);
        }
        if (isPushing && !isPushingPressed)
        {
            isPushing = false;
            animator.SetBool(isPushingHash, isPushing);
        }
    }

    private void HandleControlsLocking()
    {
        lockControls = false;
        HandleMovementSpeed();
        for (int animationIndex = 0; animationIndex < animationsLockingControls.Length; animationIndex++)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationsLockingControls[animationIndex]))
            {
                lockControls = true;
                currentMovement.x = 0.0f;
                return;
            }
        }
        for (int transitionIndex = 0; transitionIndex < transitionsLockingControls.Length; transitionIndex++)
        {
            if (animator.GetAnimatorTransitionInfo(0).userNameHash == transitionsLockingControlsHashes[transitionIndex])
            {
                lockControls = true;
                currentMovement.x = 0.0f;
                return;
            }
        }
    }

    private void HandleAnimation() 
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking) {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking) {
            animator.SetBool(isWalkingHash, false);
        }

        if (currentControlScheme == "Gamepad")
        {
            float characterCurrentVelocity = (float)Math.Sqrt(Math.Pow(currentMovement.x, 2) + Math.Pow(currentMovement.z, 2));
            walkingAnimationMultiplier = Math.Min(1, characterCurrentVelocity / velocityValue);
            if (!isRunning && characterCurrentVelocity >= errorRate * (velocityValue * runSpeedMultiplier)) 
            {
                animator.SetBool(isRunningHash, true);
            }
            else if (isRunning && characterCurrentVelocity < errorRate * (velocityValue * runSpeedMultiplier))
            {
                animator.SetBool(isRunningHash, false);
            }
        }
        else
        {
            walkingAnimationMultiplier = 1;
            if (!isRunning && isWalking && isRunPressed)
            {
                animator.SetBool(isRunningHash, true);
            }
            else if (isRunning && (!isRunPressed || !isWalking))
            {
                animator.SetBool(isRunningHash, false);
            }
        }
        animator.SetFloat(walkingAnimationMultiplierHash, walkingAnimationMultiplier);
    }

    private void HandleRotation()
    {
        positionToLookAt.x = currentMovement.x > 0 ? 1 : -1;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = 0.0f;
        Quaternion currentRotation = transform.rotation;
        targetRotation = Quaternion.LookRotation(positionToLookAt);

        if (isMovementPressed && !hasJumped && !lockControls)
        {
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleGravity()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, Vector3.down);
        distanceToGround = 0.0f;
        if (Physics.Raycast(downRay, out hit)) {
            distanceToGround = hit.distance;
        }
        if (characterController.isGrounded || distanceToGround < groundedDistanceTrigger)
        {
            if (isFalling)
            {
                isFalling = false;
                animator.SetBool(isFallingHash, isFalling);
            }
            currentMovement.y = Math.Max(-gravity * Time.fixedDeltaTime, currentMovement.y - gravity * Time.fixedDeltaTime);
        }
        else
        {
            if (!isFalling)
            {
                isFalling = true;
                animator.SetBool(isFallingHash, isFalling);
            }
            currentMovement.y += -gravity * Time.fixedDeltaTime;
        }
    }

    private void HandlePosition()
    {
        transform.position.Set(transform.position.x, transform.position.y, 0.0f);
    }

    private void HandleMovementSpeed()
    {
        if (!lockControls)
        {
            if (isRunPressed)
            {
                currentMovement.x = currentMovementInput.x * velocityValue * runSpeedMultiplier;
                // currentMovement.z = currentMovementInput.y * velocityValue * runSpeedMultiplier;
            }
            else
            {
                currentMovement.x = currentMovementInput.x * velocityValue;
                // currentMovement.z = currentMovementInput.y * velocityValue;
            }
        }
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void HandleJump()
    {
        if (isJumpPressed && !hasJumped && characterController.isGrounded && !lockControls)
        {
            currentMovement.y = (float)((jumpHeight + (gravity * Math.Pow(jumpDuration / 2, 2)) / 2) / (jumpDuration / 2));
            hasJumped = true;
            animator.SetBool(hasJumpedHash, hasJumped);
        }
        else if (!isJumpPressed && hasJumped && characterController.isGrounded && !lockControls)
        {
            hasJumped = false;
            animator.SetBool(hasJumpedHash, hasJumped);
        }
    }

    private void MovementCallbackFunction(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        HandleMovementSpeed();
    }

    private void RunningCallbackFunction(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
        HandleMovementSpeed();
    }

    private void JumpCallbackFunction(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();    
    }

    private void PushCallbackFunction(InputAction.CallbackContext context)
    {
        isPushingPressed = context.ReadValueAsButton();
    }

    private void PickUpCallbackFunction(InputAction.CallbackContext context)
    {
        pickUpPressed = context.ReadValueAsButton();
        if (pickUpPressed) {
            holdObject = !holdObject;
        }
    }

    private void SwitchControlsCallbackFunction(PlayerInput input) 
    {
        Debug.Log("device is now: " + input.currentControlScheme);
        currentControlScheme = input.currentControlScheme;
    }

    private void OnDisable()
    {
        InputManager.instance.playerInput.onControlsChanged -= SwitchControlsCallbackFunction;
    }
}
