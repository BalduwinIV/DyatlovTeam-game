using UnityEngine;

public class JumpAction : MonoBehaviour
{
    // --- Parameters ---
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float climbingSpeedMultiplier;
    [SerializeField] AnimationCurve climbOffsetCurve;
    [SerializeField] private bool showRays;
    [Range (0f, 3f)] public float rayGroundPadding = 0f;
    [Range (0.05f, 3f)] public float rayStep = 0.1f;
    [Range (0.1f, 1f)] public float rayDistance = 0.5f;
    [Range (0f, 5f)] public float obstacleRayGroundPadding = 3f;
    [Range (0f, 2f)] public float obstacleRayDistance = 1f;

    // --- Local variables ---
    private bool jumped;
    private float rayRange = 2f;

    void Update()
    {
        PlayerManager.instance.setClimbingSpeedMultiplier(climbingSpeedMultiplier);
    }

    void FixedUpdate()
    {
        bool climable = false;
        float climbOffset = 0f;
        Vector3 climbingPosition = transform.position;

        // Check if there is an obstacle in front of the character.
        Vector3 obstacleRayStart = transform.position;
        obstacleRayStart.y += obstacleRayGroundPadding;
        Ray obstacleRay = new Ray(obstacleRayStart, transform.forward);
        if (Physics.Raycast(obstacleRay, out RaycastHit obstacleRaycastHit, obstacleRayDistance))
        {
            climable = false;
            if (showRays)
            {
                Debug.DrawRay(obstacleRayStart, transform.forward * obstacleRayDistance, Color.magenta);
            }
        }
        else
        {
            if (showRays)
            {
                Debug.DrawRay(obstacleRayStart, transform.forward * obstacleRayDistance, Color.blue);
            }

            // Get precise info about the collision.
            for (float groundPaddingI = rayGroundPadding + rayRange; groundPaddingI >= rayGroundPadding; groundPaddingI -= rayStep)
            {
                Vector3 rayStart = transform.position;
                rayStart.y += groundPaddingI;
                Ray ray = new Ray(rayStart, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, rayDistance))
                {
                    if (!climable && raycastHit.transform.tag == "Wall")
                    {
                        climable = true;
                        climbingPosition = raycastHit.point;
                        climbingPosition.z -= transform.forward.z * 0.1f;
                    }
                    if (showRays)
                    {
                        // Debug.Log("Collision!: " + raycastHit.collider.name);
                        Debug.DrawRay(rayStart, transform.forward * rayDistance, Color.green);
                    }
                }
                else
                {
                    if (!climable)
                    {
                        climbOffset += (rayStep / rayRange);
                    }
                    if (showRays)
                    {
                        Debug.DrawRay(rayStart, transform.forward * rayDistance, Color.red);
                    }
                }
            }
            climbOffset = climbOffsetCurve.Evaluate(climbOffset);
            PlayerManager.instance.setClimbOffset(climbOffset);
        }

        if (PlayerManager.instance.jumpButtonIsPressed() && !jumped) {
            jumped = true;
            if (climable)
            {
                PlayerManager.instance.setClimbPosition(climbingPosition);
                PlayerManager.instance.setClimbTrigger();
            }
            else
            {
                PlayerManager.instance.setMovementVectorYComponent(jumpForce);
            }
        } else if (!PlayerManager.instance.jumpButtonIsPressed() && jumped && 
            PlayerManager.instance.getCharacterController().isGrounded) {
            jumped = false;
        } else if (jumped && climable)
        {
            PlayerManager.instance.setClimbPosition(climbingPosition);
            PlayerManager.instance.setClimbOffset(climbOffset);
            PlayerManager.instance.setClimbTrigger();
        }

    }
}
