using UnityEngine;

// Changes crystal rotation angle.
public class CrystalPuzzle1 : MonoBehaviour
{
    private Animator animator;

    private int rotationValueHash;

    [Range (0f, 1f)]
    public float rotationValue = 0.5f;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rotationValueHash = Animator.StringToHash("RotationValue");
    }

    public void Update()
    {
        animator.SetFloat(rotationValueHash, rotationValue);
    }
}
