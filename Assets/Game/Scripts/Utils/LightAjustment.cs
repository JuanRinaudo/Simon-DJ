using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAjustment : MonoBehaviour
{

    private float initialSpotAngle;
    private Vector3 initialPosition;
    private Vector3 raycastPostion;
    public Light spotlight;
    public LayerMask layerMask;
    
    void Start()
    {
        initialSpotAngle = spotlight.spotAngle;
        initialPosition = spotlight.transform.localPosition;
    }

    void Update()
    {
        RaycastHit hit;
        raycastPostion = Vector3.Lerp(transform.position, raycastPostion, 0.98f);
        if(Physics.Raycast(new Ray(raycastPostion, transform.forward), out hit, 1, layerMask))
        {
            float hitDistanceLeft = (1f - (transform.position - hit.point).magnitude);
            spotlight.spotAngle = Mathf.Lerp(spotlight.spotAngle, initialSpotAngle + hitDistanceLeft * 10f, 0.98f);
            Debug.Log(hitDistanceLeft);
            Debug.Log(hit.collider.name);
            spotlight.transform.localPosition = Vector3.Lerp(spotlight.transform.localPosition, new Vector3(0, 0, initialPosition.z - hitDistanceLeft), 0.98f);
        }
        else
        {
            spotlight.spotAngle = initialSpotAngle;
            spotlight.transform.localPosition = initialPosition;
        }
    }
}
