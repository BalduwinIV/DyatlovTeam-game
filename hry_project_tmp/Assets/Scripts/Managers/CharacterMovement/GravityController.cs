using System.Security;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    // --- Parameters ---
    [SerializeField] private float gravityValue = 15f;
    [SerializeField] private float maxDownSpeed = 19.6f;
    [SerializeField] private float groundGravityValue = 0.69f;
    [SerializeField] private float fallingTriggerDistance;
    // --- Local varibles ---
    private Vector3 rayStart;

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
        else if (PlayerManager.instance.getMovementVector().y < -maxDownSpeed)
        {
            PlayerManager.instance.setMovementVectorYComponent(-maxDownSpeed);
        }

        rayStart = transform.position;
        rayStart.y += 0.5f;
        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hitInfo, 50f)) {
            if (hitInfo.distance > fallingTriggerDistance + 0.5f) {
                PlayerManager.instance.setIsFalling(true);
            } else {
                PlayerManager.instance.setIsFalling(false);
            }
        }
    }
}
