using UnityEngine;
using Pathfinding;

public struct PathData
{
    public Path path;
    public float speed;
    public int waypointIndex;
    public float waypointThreshold;
    public Vector3 direction;
    public bool completed;

    public PathData(float movementSpeed = 1, float distanceThreshold = 0.1f)
    {
        path = null;
        speed = movementSpeed;
        waypointIndex = -1;
        waypointThreshold = distanceThreshold;
        direction = Vector3.zero;
        completed = true;
    }
}

public class PathingUtils
{

    public static void SeekPath(Seeker seeker, Vector3 from, Vector3 to, ref PathData data)
    {
        data.path = null;
        data.waypointIndex = 0;
        data.waypointThreshold = 0.1f;
        data.completed = false;
        data.path = seeker.StartPath(from, to);
    }
    
    public static void MoveTroughPath(Transform transform, ref PathData data)
    {
        while (data.waypointIndex < data.path.vectorPath.Count && (data.path.vectorPath[data.waypointIndex] - transform.position).magnitude < data.waypointThreshold)
        {
            data.waypointIndex++;
        }

        if (data.waypointIndex < data.path.vectorPath.Count)
        {
            data.direction = (data.path.vectorPath[data.waypointIndex] - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(data.direction);
            transform.position += data.direction * data.speed * Time.deltaTime;
        }
        else
        {
            data.completed = true;
        }
    }

    public static void MoveTroughPathSmooth(Transform transform, ref PathData data)
    {
        while (data.waypointIndex < data.path.vectorPath.Count && (data.path.vectorPath[data.waypointIndex] - transform.position).magnitude < data.waypointThreshold)
        {
            data.waypointIndex++;
        }

        if (data.waypointIndex < data.path.vectorPath.Count)
        {
            Vector3 targetDirection = (data.path.vectorPath[data.waypointIndex] - transform.position).normalized;
            data.direction = Vector3.Lerp(data.direction, targetDirection, 0.1f);
            transform.rotation = Quaternion.LookRotation(data.direction);
            transform.position += data.direction * data.speed * Time.deltaTime;
        }
        else
        {
            data.completed = true;
        }
    }

}
