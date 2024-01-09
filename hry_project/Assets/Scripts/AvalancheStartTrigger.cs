using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheStartTrigger : MonoBehaviour
{
    public GameObject avalanche;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerManager.instance.lockMovement();
            PlayerManager.instance.setFinalDeathTrigger();
            if(avalanche != null){
                avalanche.GetComponent<Animator>().SetTrigger("Start");
            }
        }
    }
}
