using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Parameters
    [SerializeField] private float movementSpeedMultiplier = 1.0f;
    [SerializeField] private float movementSpeedChangeMultiplier = 1.0f;
    [SerializeField] private float movementStopSpeedChangeMultiplier = 1.0f;
    [SerializeField] private AnimationCurve movementCurve;

    // Local variables
    private float smoothT;
    private bool previousMovementIsPositive;
    private bool currentMovementIsPositive;
    private bool movementHasBeenLocked;

    void Start()
    {
        smoothT = 0.0f;
        previousMovementIsPositive = true;
        currentMovementIsPositive = true;
        movementHasBeenLocked = false;
    }

    void FixedUpdate() {
        UpdateMovementVector();
        CheckTurn();
    }

    void CheckTurn() {
        if (PlayerManager.instance.getRawMovementInputX() > 0)
        {
            currentMovementIsPositive = true;
        } else if (PlayerManager.instance.getRawMovementInputX() < 0) {
            currentMovementIsPositive = false;
        }
        if (currentMovementIsPositive != previousMovementIsPositive)
        {
            PlayerManager.instance.setTurnTringger();
        }
        previousMovementIsPositive = currentMovementIsPositive;

        if (!PlayerManager.instance.movementIsLocked() && movementHasBeenLocked) {
            if (transform.forward.x > 0) {
                transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
            } else {
                transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
            }
        }
        movementHasBeenLocked = PlayerManager.instance.movementIsLocked();
    }

    void UpdateMovementVector() {
        float rawX = PlayerManager.instance.getRawMovementInputX();
        if (smoothT < rawX) {
            smoothT += rawX == 0.0f ? movementSpeedChangeMultiplier * Time.fixedDeltaTime * movementStopSpeedChangeMultiplier : movementSpeedChangeMultiplier * Time.fixedDeltaTime;
            if (smoothT > rawX) {
                smoothT = rawX;
            }
        } else if (smoothT > rawX) {
            smoothT -= rawX == 0.0f ? movementSpeedChangeMultiplier * Time.fixedDeltaTime * movementStopSpeedChangeMultiplier : movementSpeedChangeMultiplier * Time.fixedDeltaTime;
            if (smoothT < rawX) {
                smoothT = rawX;
            }
        }
        if (smoothT > 0) {
            PlayerManager.instance.setMovementVectorXComponent(movementCurve.Evaluate(smoothT) * movementSpeedMultiplier);
        } else {
            PlayerManager.instance.setMovementVectorXComponent(-1 * movementCurve.Evaluate(-smoothT) * movementSpeedMultiplier);
        }
        PlayerManager.instance.setNormalizedMovementVectorX(Math.Abs(smoothT));
    }
}
