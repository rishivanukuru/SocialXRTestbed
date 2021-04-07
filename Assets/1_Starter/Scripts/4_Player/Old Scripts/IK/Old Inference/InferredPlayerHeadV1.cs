using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InferredPlayerHeadV1 : MonoBehaviour
{

    public GameObject phone;
    public GameObject head;

    public Vector3 innerEllipsoid;
    public Vector3 outerEllipsoid;

    public float minRange = 0.3f;
    public float idealRange = 0.6f;
    public float maxRange = 0.8f;
    public float time = 0.01f;
    public float smoothSpeed = 0.125f;
    public float rotSmoothSpeed = 0.125f;

    GameObject targetObject;
    Transform target;

    public bool isAdjustingPosition = false;
    public bool isAdjustingRotation = false;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
    }

    void Start()
    {
        targetObject = new GameObject();

        target = targetObject.transform;

        isAdjustingPosition = false;
        isAdjustingRotation = false;
    }

    void Update()
    {
        Debug.DrawLine(head.transform.position, head.transform.position + head.transform.forward * outerEllipsoid.z);
        Debug.DrawLine(head.transform.position, head.transform.position - head.transform.forward * outerEllipsoid.z);
        Debug.DrawLine(head.transform.position, head.transform.position + head.transform.up * outerEllipsoid.y);
        Debug.DrawLine(head.transform.position, head.transform.position - head.transform.up * outerEllipsoid.y);
        Debug.DrawLine(head.transform.position, head.transform.position + head.transform.right * outerEllipsoid.x);
        Debug.DrawLine(head.transform.position, head.transform.position - head.transform.right * outerEllipsoid.x);

        Debug.DrawLine(head.transform.position, head.transform.position + head.transform.forward * idealRange,Color.red);

        Debug.DrawLine(head.transform.position, head.transform.position + head.transform.forward * innerEllipsoid.z,Color.blue);
        Debug.DrawLine(head.transform.position, head.transform.position - head.transform.forward * innerEllipsoid.z, Color.blue);
        Debug.DrawLine(head.transform.position, head.transform.position + head.transform.up * innerEllipsoid.y, Color.blue);
        Debug.DrawLine(head.transform.position, head.transform.position - head.transform.up * innerEllipsoid.y, Color.blue);
        Debug.DrawLine(head.transform.position, head.transform.position + head.transform.right * innerEllipsoid.x, Color.blue);
        Debug.DrawLine(head.transform.position, head.transform.position - head.transform.right * innerEllipsoid.x, Color.blue);


        SetTargetPosition();
        SetTargetRotation();
        
    }

    private void FixedUpdate()
    {
        CheckPosition();
        AdjustPosition();

        CheckRotation();
        AdjustRotation();

    }

    void SetTargetPosition()
    {
        target.transform.position = phone.transform.position - phone.transform.forward*idealRange;
    }

    void CheckPosition()
    {


        Vector3 d = phone.transform.position - head.transform.position;

        float dx = phone.transform.position.x - head.transform.position.x;
        float dy = phone.transform.position.y - head.transform.position.y;
        float dz = phone.transform.position.z - head.transform.position.z;

        float x = Vector3.Dot(d, head.transform.right);
        float y = Vector3.Dot(d, head.transform.up);
        float z = Vector3.Dot(d, head.transform.forward);

        Debug.Log("> X: " + x.ToString() + "> Y: " + y.ToString() + "> Z: " + z.ToString());

        float innerEllipsoidDistance = (x * x) / (innerEllipsoid.x * innerEllipsoid.x) + (y * y) / (innerEllipsoid.y * innerEllipsoid.y) + (z * z) / (innerEllipsoid.z * innerEllipsoid.z);
        float outerEllipsoidDistance = (x * x) / (outerEllipsoid.x * outerEllipsoid.x) + (y * y) / (outerEllipsoid.y * outerEllipsoid.y) + (z * z) / (outerEllipsoid.z * outerEllipsoid.z);

        bool inDistRange = (innerEllipsoidDistance > 1 && outerEllipsoidDistance < 1) ? true : false;

        Debug.Log("Inner Ellipsoid: " + innerEllipsoidDistance.ToString() + ", Outer Ellipsoid: " + outerEllipsoidDistance.ToString() + ", inDistRange: " + inDistRange.ToString());

        //bool inDistRange = (Mathf.Abs(Vector3.Distance(phone.transform.position, head.transform.position)) < minRange || Mathf.Abs(Vector3.Distance(phone.transform.position, head.transform.position)) > maxRange) ? false : true;
        //Debug.Log("DistRange: " + Mathf.Abs(Vector3.Distance(phone.transform.position, head.transform.position)).ToString());


        bool inRotRange = (Mathf.Abs(Vector3.Dot(head.transform.forward.normalized, phone.transform.forward.normalized)) > Mathf.Cos(Mathf.Deg2Rad*20)) ? true : false;

        // Debug.Log("Rotation Range: " + Mathf.Abs(Vector3.Dot(head.transform.forward, phone.transform.forward)).ToString());

        if(!inDistRange || !inRotRange)
        {
            isAdjustingPosition = true;
        }
    }

    void AdjustPosition()
    {

        if (Vector3.Distance(head.transform.position, target.transform.position) < 0.001f)
        {
            isAdjustingPosition = false;
        }

        if (isAdjustingPosition)
        {
            Vector3 smoothedPosition = Vector3.Lerp(head.transform.position, target.transform.position, smoothSpeed * Time.fixedDeltaTime);
            head.transform.position = smoothedPosition;
            //Vector3 lookVector = (phone.transform.position - head.transform.position).normalized;
            //head.transform.Translate(lookVector * Time.deltaTime * smoothSpeed);


        }
       
    }

    void SetTargetRotation()
    {
        target.LookAt(phone.transform);
    }

    void CheckRotation()
    {
        bool inRotRange = (Mathf.Abs(Vector3.Dot(head.transform.forward.normalized, phone.transform.forward.normalized)) > Mathf.Cos(Mathf.Deg2Rad * 20)) ? true : false;

        //Debug.Log("Rotation Value: " + Mathf.Abs(Vector3.Dot(head.transform.forward.normalized, phone.transform.forward.normalized)).ToString());

        if (!inRotRange)
        {
            isAdjustingRotation = true;
        }

    }

    void AdjustRotation()
    {
        //head.transform.rotation = target.rotation;

        //Debug.Log("Rotation Factor: " + Mathf.Abs(Vector3.Dot(head.transform.forward.normalized, phone.transform.forward.normalized)).ToString());

        if (Mathf.Abs(Vector3.Dot(head.transform.forward.normalized, phone.transform.forward.normalized)) > Mathf.Cos(Mathf.Deg2Rad * 2))
        {
            isAdjustingRotation = false;
        }

        if (isAdjustingRotation)
        {
            Quaternion smoothedRotation = Quaternion.Lerp(head.transform.rotation, target.transform.rotation, rotSmoothSpeed * Time.fixedDeltaTime);
            head.transform.rotation = smoothedRotation;
        }

    }



}
