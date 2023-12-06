using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

/*
 * Players manager singleton.
 * 
 * Get access: PlayerManager.instance.<method>
 * 
 * Get variables values by calling getter functions.
 * Implement setter functions only if needed.
 * Use hashes for animator parameters.
 * Get access to components through this manager.
 * For physics related updates use FixedUpdate.
 * For animations related updates use LateUpdate.
 */

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    // --- Components ---
    private CharacterController characterController;
    private Animator animator;

    // --- Animator hashes ---
    // Defines x value of current movement input vector (floata <0,1>)
    private int normalizedMovementVectorXHash;
    // Defines from what time (normalized) should climbing animation start. (float <0,1>)
    private int climbOffsetHash; 
    // Defines characters falling state. (bool <True;False>)
    private int isFallingHash;
    // Defines climbing speed increese while holding movement input. (float, defined by JumpAction.cs parameter)
    private int climbingSpeedMultiplierHash;

    // Variables
    private Vector3 movementVector;
    private bool lockTransform;
    private Vector3 climbPosition;
    private float normalizedMovementVectorX;
    private float climbOffset;
    private bool isFalling;
    private float climbingSpeedMultiplier;

    // Input variables
    private Vector2 rawMovementInput;
    private bool jumpButtonState;
    private bool actionButtonState;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        normalizedMovementVectorXHash = Animator.StringToHash("normalizedMovementVectorX");
        climbOffsetHash = Animator.StringToHash("climbOffset");
        isFallingHash = Animator.StringToHash("isFalling");
        climbingSpeedMultiplierHash = Animator.StringToHash("climbingSpeedMultiplier");
        lockTransform = false;
    }

    void Start()
    {
        InputManager.instance.controls.PlayerControls.Movement.started += MovementCallbackFunction;
        InputManager.instance.controls.PlayerControls.Movement.performed += MovementCallbackFunction;
        InputManager.instance.controls.PlayerControls.Movement.canceled += MovementCallbackFunction;

        InputManager.instance.controls.PlayerControls.Jump.started += JumpButtonCallbackFunction;
        InputManager.instance.controls.PlayerControls.Jump.canceled += JumpButtonCallbackFunction;

        InputManager.instance.controls.PlayerControls.ActionButton.started += ActionButtonCallbackFunction;
        InputManager.instance.controls.PlayerControls.ActionButton.canceled += ActionButtonCallbackFunction;
    }

    void FixedUpdate() {
        if (!lockTransform)
        {
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }
    }

    public void changeMovementVectorYComponent(float value)
    {
        movementVector.y += value;
    }

// ---------
// Setters
// ---------

// Animator
    public void lockMovement()
    {
        lockTransform = true;
    }

    public void unlockMovement()
    {
        lockTransform = false;
    }

    public void setNormalizedMovementVectorX(float value)
    {
        normalizedMovementVectorX = value;
        animator.SetFloat(normalizedMovementVectorXHash, value);
        animator.SetFloat(climbingSpeedMultiplierHash, Math.Max(climbingSpeedMultiplier + value, 1f));
    }

    public void setTurnTrigger()
    {
        if (normalizedMovementVectorX > 0.3f)
        {
            animator.Play("RunningTurn");
        }
        else
        {
            animator.Play("WalkingTurn");
        }
    }

    public void setClimbTrigger()
    {
        lockMovement();
        applyClimbPosition();
        if (climbOffset < 0.6f)
        {
            animator.Play("Climbing", 0, climbOffset);
        }
        else
        {
            animator.Play("Climbing", 0, 0.6f);
        }
        Debug.Log("Playing climb animation with climbOffset=" + climbOffset);
    }

    public void setClimbOffset(float offset)
    {
        climbOffset = offset;
        animator.SetFloat(climbOffsetHash, offset);
    }

    public void setClimbPosition(Vector3 position)
    {
        climbPosition = position;
    }

    public void applyClimbPosition()
    {
        transform.position = climbPosition;
    }

    public void setIsFalling(bool state)
    {
        isFalling = state;
        animator.SetBool(isFallingHash, state);
    }

    public void setClimbingSpeedMultiplier(float value)
    {
        climbingSpeedMultiplier = value;
    }

// Player movement
    public void setMovementVector(float x, float y, float z) 
    {
        movementVector.Set(x, y, z);
    }

    public void setMovementVectorXComponent(float value) 
    {
        movementVector.x = value;
    }

    public void setMovementVectorYComponent(float value) 
    {
        movementVector.y = value;
    }

    public void setMovementVectorZComponent(float value) 
    {
        movementVector.z = value;
    }

// ---------
// Getters
// ---------

    public CharacterController getCharacterController()
    {
        return characterController;
    }

    public Animator getAnimator()
    {
        return animator;
    }

    public bool movementIsLocked()
    {
        return lockTransform;
    }

    public Vector3 getMovementVector() {
        return movementVector;
    }

    public Vector2 getRawMovementInput() 
    {
        return rawMovementInput;
    }

    public float getRawMovementInputX() 
    {
        return rawMovementInput.x;
    }

    public float getRawMovementInputY()
    {
        return rawMovementInput.y;
    }

    public bool jumpButtonIsPressed()
    {
        return jumpButtonState;
    }

    public bool actionButtonIsPressed()
    {
        return actionButtonState;
    }

    public bool getIsFalling()
    {
        return isFalling;
    }

// --------------------
// Callback functions
// --------------------

    private void MovementCallbackFunction(InputAction.CallbackContext context) {
        rawMovementInput = context.ReadValue<Vector2>();
    }

    private void JumpButtonCallbackFunction(InputAction.CallbackContext context) {
        jumpButtonState = context.ReadValueAsButton();
    }

    private void ActionButtonCallbackFunction(InputAction.CallbackContext context)
    {
        actionButtonState = context.ReadValueAsButton();
    }
}
