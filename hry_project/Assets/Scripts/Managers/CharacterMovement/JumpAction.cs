using UnityEngine;

public class JumpAction : MonoBehaviour
{
    [SerializeField] private float jumpForce = 8f;
    private bool jumped;

    void FixedUpdate()
    {
        if (PlayerManager.instance.jumpButtonIsPressed() && !jumped) {
            jumped = true;
            PlayerManager.instance.setMovementVectorYComponent(jumpForce);
        } else if (!PlayerManager.instance.jumpButtonIsPressed() && jumped &&
                    PlayerManager.instance.getCharacterController().isGrounded) {
            jumped = false;
        }
    }
}
