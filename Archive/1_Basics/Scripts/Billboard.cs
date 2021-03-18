using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public enum rotationType
    {
        YAxis,
        AllAxes
    }

    public rotationType RotationType;
    private Transform Cam;
    private Vector3 LookVector;

    void Start()
    {
        Cam = Camera.main.transform;
    }


    void Update()
    {
        LookVector = Cam.position - transform.position;
        if (RotationType == rotationType.YAxis)
            LookVector.y = 0;
        transform.rotation = Quaternion.LookRotation(LookVector);
    }
}
