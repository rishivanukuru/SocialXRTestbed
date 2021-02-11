using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCameraCalibration : MonoBehaviour
{
    GameObject follower;
    public bool isGlobal = false;
    public bool isDebug = false;

    void Start()
    {
        follower = null;
        if(!isGlobal)
        {
            follower = this.gameObject.transform.parent.gameObject;
        }
        else
        {
            follower = Camera.main.gameObject;
        }

        this.gameObject.transform.parent = null;

    }

    void Update()
    {
        this.gameObject.transform.position = follower.transform.position + follower.transform.forward * 0.76f;
        if(isDebug)
        {
            Debug.Log(Vector3.Distance(this.gameObject.transform.position, follower.transform.position).ToString());
        }
    }
}
