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

    public bool isOrtho = false;

    public rotationType RotationType;
    private Transform Cam;
    private Vector3 LookVector;

    void Start()
    {
        
    }


    void Update()
    {
        Cam = Camera.main.transform;
        LookVector = Cam.position - transform.position;
        
        if (RotationType == rotationType.YAxis)
            LookVector.y = 0;
      
        if(isOrtho)
        {

            transform.rotation = Quaternion.identity;
            //LookVector.x = 0;
            //Quaternion tempRotation = Quaternion.LookRotation(LookVector);
            //transform.rotation = Quaternion.LookRotation(transform.position-LookVector);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(LookVector);
        }

    }
}
