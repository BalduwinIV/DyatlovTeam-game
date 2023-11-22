using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] private float gravityValue = 9.8f;
    [SerializeField] private float maxDownSpeed = 9.8f;
    [SerializeField] private float groundGravityValue = 2f;

    void FixedUpdate()
    {
        if (PlayerManager.instance.getCharacterController().isGrounded &&
            PlayerManager.instance.getMovementVector().y <= 0)
        {
            PlayerManager.instance.setMovementVectorYComponent(-groundGravityValue);
        }
        else if (PlayerManager.instance.getMovementVector().y > -maxDownSpeed)
        {
            PlayerManager.instance.changeMovementVectorYComponent(-gravityValue * Time.fixedDeltaTime);
        }
        else if (PlayerManager.instance.getCharacterController().isGrounded ||
                PlayerManager.instance.getMovementVector().y < -maxDownSpeed)
        {
            PlayerManager.instance.setMovementVectorYComponent(-maxDownSpeed);
        }
    }
}
