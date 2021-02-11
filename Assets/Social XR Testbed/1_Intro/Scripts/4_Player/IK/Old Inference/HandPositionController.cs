using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPositionController : MonoBehaviour
{

    public Vector3 upVector;
    public Vector3 downVector;
    public GameObject face;
    public PlayerHandler myPlayerHandler;

    Vector3 lookVector;

    bool isPhoneUp;

    // Start is called before the first frame update
    void Start()
    {
        isPhoneUp = true;
    }

    // Update is called once per frame
    void Update()
    {

        /*
         * 
        if(myPlayerHandler.HandUp == true)
        {
            if(isPhoneUp == false)
            {
                HandsUp();
                isPhoneUp = true;
            }
        }
        else
        {
            if (isPhoneUp == true)
            {
                HandsDown();
                isPhoneUp = false;
            }
        }
        */


        /*
        lookVector = face.transform.position - RoomManager.instance.PlayerRef.transform.position;
    
        if(Vector3.Dot(face.transform.forward.normalized,lookVector)> -0.5 && Vector3.Dot(RoomManager.instance.PlayerRef.transform.forward.normalized, lookVector) > -0.5)
        {
            if(isPhoneUp == true)
            {
                HandsDown();
                isPhoneUp = false;
            }
        }
        else
        {
            if(isPhoneUp == false)
            {
                HandsUp();
                isPhoneUp = true;
            }
        }
        */
    }


    public void HandsDown()
    {
        StartCoroutine(RotateToDirection(this.transform, downVector, 1));
        //this.gameObject.transform.localRotation = Quaternion.Euler(downVector);
    }

    public void HandsUp()
    {
        StartCoroutine(RotateToDirection(this.transform, upVector, 1));
        //this.gameObject.transform.localRotation = Quaternion.Euler(upVector);
    }

    public IEnumerator RotateToDirection(Transform transform, Vector3 positionToLook, float timeToRotate)
    {
        var startRotation = transform.localRotation;
        //var direction = positionToLook - transform.position;
        var finalRotation = Quaternion.Euler(positionToLook);
        var t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / timeToRotate;
            transform.localRotation = Quaternion.Lerp(startRotation, finalRotation, t);
            yield return null;
        }
        transform.localRotation = finalRotation;
    }
}
