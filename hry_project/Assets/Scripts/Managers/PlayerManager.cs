using UnityEngine;
using UnityEngine.InputSystem;

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
    // Components
    private CharacterController characterController;
    private Animator animator;

    // Animator hashes
    private int normalizedMovementVectorXHash;
    private int turnHash;

    // Variables
    private Vector3 movementVector;
    private bool lockTransform;

    // Input variables
    private Vector2 rawMovementInput;
    private bool jumpButtonState;

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

        InputManager.instance.controls.PlayerControls.Jump.started += JumpButtonCallbackFunction;
        InputManager.instance.controls.PlayerControls.Jump.canceled += JumpButtonCallbackFunction;
    }

    void FixedUpdate() {
        if (!lockTransform)
        {
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }
    }

// ---------
// Setters
// ---------

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

// --------------------
// Callback functions
// --------------------

    private void MovementCallbackFunction(InputAction.CallbackContext context) {
        rawMovementInput = context.ReadValue<Vector2>();
    }

    private void JumpButtonCallbackFunction(InputAction.CallbackContext context) {
        jumpButtonState = context.ReadValueAsButton();
    }
}
