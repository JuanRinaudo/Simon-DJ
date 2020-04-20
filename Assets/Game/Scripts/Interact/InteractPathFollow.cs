using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class InteractPathFollow : AbstractInteractable
{

    public Vector3 lastPosition;
    public Transform target;

    private bool following;

    private float timerRecalculate;
    [Min(0.05f)] public float recalculateTime;

    private Animator animator;

    private Seeker seeker;
    private PathData pathData;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        seeker = GetComponent<Seeker>();

        pathData = new PathData(5);

        following = false;
    }

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        timerRecalculate = recalculateTime;
        following = !following;
    }

    void Update()
    {
        if(following)
        {
            timerRecalculate -= Time.deltaTime;
            if(timerRecalculate <= 0 && lastPosition != target.position)
            {
                PathingUtils.SeekPath(seeker, transform.position, target.position, ref pathData);
                lastPosition = target.position;
                timerRecalculate = recalculateTime;
            }

            bool seekingPath = pathData.path != null && !pathData.completed && pathData.path.IsDone();

            if (seekingPath)
            {
                PathingUtils.MoveTroughPathSmooth(transform, ref pathData);
            }
        }
    }

}
