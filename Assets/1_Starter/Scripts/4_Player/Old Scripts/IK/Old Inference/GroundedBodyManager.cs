using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedBodyManager : MonoBehaviour
{

    float xAngle;
    float yAngle;
    float zAngle;
    Vector3 clampedAngle;

    public GameObject playerHead;
    public GameObject groundedBody;
    public float bodyOffset;
    public float height;
    public Vector3 scaleVector;

    public GameObject groundReference;

    bool isGrounded;

    void Start()
    {
        isGrounded = false;       
    }

    void Update()
    {
        if(!isGrounded)
        {
            FindGroundReference();
        }
        else
        {
            ClampBodyAngle();
            GroundBody();
        }
    }

    void FindGroundReference()
    {
        if (RoomManager.instance.referenceObject != null)
        {
            groundReference = RoomManager.instance.referenceObject;
            isGrounded = true;
        }
    }

    void ClampBodyAngle()
    {
        xAngle = 0;
        yAngle = this.gameObject.transform.rotation.eulerAngles.y;
        zAngle = 0;
        clampedAngle.Set(xAngle, yAngle, zAngle);

        this.transform.rotation = Quaternion.Euler(clampedAngle);
    }

    void GroundBody()
    {
        height = playerHead.transform.position.y - groundReference.transform.position.y - bodyOffset;
        scaleVector.Set(1, height, 1);
        
        groundedBody.transform.localScale = scaleVector;
    }
}
