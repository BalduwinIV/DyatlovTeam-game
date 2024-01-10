using UnityEngine;

public class LightEnableTrigger : MonoBehaviour
{
    public Light targetLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetLight != null)
            {
                targetLight.enabled = true; 
            }
        }
    }
}
