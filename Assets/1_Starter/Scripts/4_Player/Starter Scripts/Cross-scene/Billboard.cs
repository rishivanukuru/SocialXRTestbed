using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    //Script to make the avatar canvas always display facing the player

    public enum rotationType
    {
        YAxis,
        AllAxes
    }

    [Header("Rotate Vertically/All Axes")]
    public rotationType RotationType;

    [Header("Orthographic Test for Lobby")]
    public bool isOrtho = false;

    [Header("Debug test for flipped text")]
    public bool isFlip = false;

    private Transform Cam; //Main Camera reference
    private Vector3 LookVector; //Vector from canvas to player

    void Update()
    {
        Cam = Camera.main.transform; //Assign Main Camera
        LookVector = Cam.position - transform.position; //Calculate look vector
        
        if (RotationType == rotationType.YAxis) //Constrain about Y Axis
            LookVector.y = 0;

        if(isFlip) //Flip direction for debug
        {
            LookVector = -LookVector;
        }
      
        if(isOrtho) //Debug, don't rotate at all
        {

            transform.rotation = Quaternion.identity;
            //LookVector.x = 0;
            //Quaternion tempRotation = Quaternion.LookRotation(LookVector);
            //transform.rotation = Quaternion.LookRotation(transform.position-LookVector);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(LookVector); //Calculate rotation that faces the look vector
        }

    }
}
