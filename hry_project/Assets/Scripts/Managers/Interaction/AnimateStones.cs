using UnityEngine;

/* Used in animation. */
public class AnimateStones : MonoBehaviour
{
    void activateStoneAnimation()
    {
        PuzzleManager.instance.startStoneAnimation();
    }
}
