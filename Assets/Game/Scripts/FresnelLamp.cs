using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FresnelLamp : MonoBehaviour
{

    private float initialYRotation;
    public float rotationOffset = 0;
    public float rotationSpeed = 1;
    public float rotationDelta = 10;

    private void Awake()
    {
        initialYRotation = transform.localRotation.eulerAngles.y;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y = initialYRotation + Mathf.Sin(Time.time * rotationSpeed + rotationOffset) * rotationDelta;
        transform.localEulerAngles = rotation;
    }
    
}
