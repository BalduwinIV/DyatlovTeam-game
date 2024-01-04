using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioClip snowStep1;
    [SerializeField] private AudioClip snowStep2;
    [SerializeField] private AudioClip[] stoneSteps;
    [Range (0f, 1f)]
    public float stepVolume;

    public void PlayStep1() {
        if (PlayerManager.instance.getCharacterIsOnSnow())
        {
            AudioManager.instance.PlayAudioClip(snowStep1, transform, stepVolume);
        }
        else
        {
            AudioManager.instance.PlayAudioClip(stoneSteps[(int)UnityEngine.Random.Range(0f, stoneSteps.Length)], transform, stepVolume);
        }
    }

    public void PlayStep2() {
        if (PlayerManager.instance.getCharacterIsOnSnow())
        {
            AudioManager.instance.PlayAudioClip(snowStep2, transform, stepVolume);
        }
        else
        {
            AudioManager.instance.PlayAudioClip(stoneSteps[(int)UnityEngine.Random.Range(0f, stoneSteps.Length)], transform, stepVolume);
        }
    }
}
