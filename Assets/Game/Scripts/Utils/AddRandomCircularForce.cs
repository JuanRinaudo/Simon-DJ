using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRandomCircularForce : MonoBehaviour
{

    public float forceMultiplier;
    private Vector3 forceAdded;

    private void Awake()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        forceAdded = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 0.2f), Random.Range(-1f, 1f));
        rigidbody.AddForce(forceAdded * forceMultiplier);
        rigidbody.angularVelocity = new Vector3(Random.Range(-Mathf.PI * 2f, Mathf.PI * 2f), Random.Range(-Mathf.PI * 2f, Mathf.PI * 2f), Random.Range(-Mathf.PI * 2f, Mathf.PI * 2f));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + forceAdded);
    }

}
