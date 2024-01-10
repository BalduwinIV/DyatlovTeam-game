using UnityEngine;

// Audio manager singleton.
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSourceObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Plays <audioClip> at <spawnTransform> position with <volume> volume.
    public void PlayAudioClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(audioSourceObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}