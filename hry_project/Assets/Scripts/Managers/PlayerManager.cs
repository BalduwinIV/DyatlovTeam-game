using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    // Components
    private CharacterController characterController;
    private Animator animator;

    // Animator hashes
    private int normalizedMovementVectorXHash;
    private int turnHash;

    // Variables
    private Vector3 movementVector;
    private Vector2 rawMovementInput;
    private bool lockTransform;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        normalizedMovementVectorXHash = Animator.StringToHash("normalizedMovementVectorX");
        turnHash = Animator.StringToHash("turn");
        lockTransform = false;
    }

    void Start()
    {
        InputManager.instance.controls.PlayerControls.Movement.started += MovementCallbackFunction;
        InputManager.instance.controls.PlayerControls.Movement.performed += MovementCallbackFunction;
        InputManager.instance.controls.PlayerControls.Movement.canceled += MovementCallbackFunction;
    }

    void FixedUpdate() {
        if (!lockTransform)
        {
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }
    }

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

    public void lockMovement()
    {
        lockTransform = true;
    }

    public void unlockMovement()
    {
        lockTransform = false;
    }

    public Vector3 getMovementVector() {
        return movementVector;
    }

    public void setNormalizedMovementVectorX(float value)
    {
        animator.SetFloat(normalizedMovementVectorXHash, value);
    }

    public void setTurnTringger()
    {
        animator.SetTrigger(turnHash);
    }

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

    private void MovementCallbackFunction(InputAction.CallbackContext context) {
        rawMovementInput = context.ReadValue<Vector2>();
    }
}
