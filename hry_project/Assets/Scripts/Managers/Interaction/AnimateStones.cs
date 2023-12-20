using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateStones : MonoBehaviour
{
    void activateStoneAnimation()
    {
        PuzzleManager.instance.startStoneAnimation();
    }
}
