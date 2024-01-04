using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowDisableTrigger : MonoBehaviour
{
    public ParticleSystem snowParticleSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (snowParticleSystem != null)
            {
                snowParticleSystem.gameObject.SetActive(false);
            }
        }
    }
}
