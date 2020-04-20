using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class InteractPathMoveTo : AbstractInteractable
{

    public Transform target;

    private Animator animator;

    private Seeker seeker;
    private PathData pathData;

    void Start()
    {
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();

        pathData = new PathData(5);
    }

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        PathingUtils.SeekPath(seeker, transform.position, target.position, ref pathData);
    }
    
    void Update()
    {
        bool seekingPath = pathData.path != null && !pathData.completed && pathData.path.IsDone();

        if (seekingPath)
        {
            PathingUtils.MoveTroughPathSmooth(transform, ref pathData);
        }
    }
    
}
