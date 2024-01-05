using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheStartTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerManager.instance.lockMovement();
        }
    }
}
