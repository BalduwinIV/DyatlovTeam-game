using UnityEngine;

public class SnowEnableRtigger : MonoBehaviour
{
    public ParticleSystem snowParticleSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (snowParticleSystem != null)
            {
                snowParticleSystem.gameObject.SetActive(true);
            }
            PlayerManager.instance.setCharacterIsOnSnow(true);
        }
    }
}
