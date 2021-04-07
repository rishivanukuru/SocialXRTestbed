using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainRelativePosition : MonoBehaviour
{
    public Transform targetTransform;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - targetTransform.position;
    }

    void Update()
    {
        transform.position = targetTransform.position + offset;
    }
}
