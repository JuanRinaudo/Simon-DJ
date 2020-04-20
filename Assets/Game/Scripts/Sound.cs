using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{

    private static AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.minDistance = 1;
        audioSource.maxDistance = 100;
    }

    public static void PlayOneShot(Transform transform, AudioClip clip, float volume = 1)
    {
        audioSource.transform.position = transform.position;
        audioSource.PlayOneShot(clip, volume);
    }

}
