using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetIKManager : MonoBehaviour
{
    private Animator animator;

    [Range (0, 1f)]
    public float distanceToGround;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimationIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distanceToGround;
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            }
        }
    }
}
