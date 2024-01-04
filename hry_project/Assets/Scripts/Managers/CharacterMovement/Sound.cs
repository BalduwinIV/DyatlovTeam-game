using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioClip snowStep1;
    [SerializeField] private AudioClip snowStep2;
    [Range (0f, 1f)]
    public float stepVolume;

    public void PlayStep1() {
        AudioManager.instance.PlayAudioClip(snowStep1, transform, stepVolume);
    }

    public void PlayStep2() {
        AudioManager.instance.PlayAudioClip(snowStep2, transform, stepVolume);
    }
}
