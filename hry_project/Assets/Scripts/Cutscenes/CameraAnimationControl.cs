using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationControl : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void ActivatePlayerShootAnimation()
    {
        if(player != null){
            player.GetComponent<Animator>().SetTrigger("Shoot");
        }
    }
}
