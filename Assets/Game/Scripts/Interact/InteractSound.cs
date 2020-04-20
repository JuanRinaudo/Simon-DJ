using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSound : AbstractInteractable
{

    public AudioSource audioSource;
    public AudioClip audioClip;

    [Space]
    public float volume = 1;
    public float pitchShift = 0;

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if(audioClip)
        {
            if(audioSource)
            {
                audioSource.pitch = 1 + pitchShift;
                audioSource.PlayOneShot(audioClip, volume);
            }
            else
            {
                Sound.PlayOneShot(transform, audioClip, volume);
            }
        }
        else if(audioSource)
        {
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

}
