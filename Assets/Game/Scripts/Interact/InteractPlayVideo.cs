using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InteractPlayVideo : AbstractInteractable
{

    private MeshRenderer meshRenderer;
    private Material offMaterial;

    public Material onMaterial;
    public VideoPlayer videoPlayer;
    public bool stopVideo = false;

    void Awake()
    {
        meshRenderer = videoPlayer.GetComponent<MeshRenderer>();
        offMaterial = meshRenderer.material;
    }

    void Update()
    {

    }

    override public void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        if (!running)
        {
            if (onMaterial != null)
            {
                meshRenderer.material = onMaterial;
            }
            videoPlayer.Play();
            running = true;
        }
        else
        {
            if (stopVideo)
            {
                meshRenderer.material = offMaterial;
                videoPlayer.Stop();
                running = false;
            }
            else
            {
                videoPlayer.Pause();
            }
        }
    }

}
