using UnityEngine;
using UnityEngine.SceneManagement;

// Changes first level to the second.
public class ChangeSceneOnTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip suspenseClip;
    [Range (0f, 1f)]
    public float suspenseVolume;
    [SerializeField] private AudioClip hitSoundClip;
    [Range (0f, 1f)]
    public float hitSoundVolume;
    public string sceneName = "SecondLvl";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            LoadScene();
        }
    }
    void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlaySuspenseSound()
    {
        AudioManager.instance.PlayAudioClip(suspenseClip, transform, suspenseVolume);
    }

    public void PlayHitSound()
    {
        AudioManager.instance.PlayAudioClip(hitSoundClip, transform, hitSoundVolume);
    }
}
