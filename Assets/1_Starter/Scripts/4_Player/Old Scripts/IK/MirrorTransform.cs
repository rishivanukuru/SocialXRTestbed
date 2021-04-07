
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MirrorTransform : MonoBehaviour
{
    public bool isEnabledInEditor = false;
    private Transform src;
    private Vector3 InitOffset,Center, newCenter, groundPos, angles;
    private Quaternion prevRot,rotDelta;
    private void OnEnable()
    {
        if (Application.isEditor) this.enabled = isEnabledInEditor;
        src = Camera.main.transform;
        InitOffset = transform.position - src.position;
        Center = GetGroundCenter(transform.position,src.position);
        prevRot = src.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = src.position + InitOffset;        
        newCenter = GetGroundCenter(transform.position, src.position);
        groundPos = transform.position;
        groundPos.y = 0;
        var newCenterDir = (newCenter - groundPos);
        float dist = Vector3.Dot(newCenterDir, Center - groundPos)/newCenterDir.magnitude;
        dist -= newCenterDir.magnitude;
        transform.position +=2* dist * newCenterDir.normalized;

        rotDelta = Quaternion.Inverse(src.rotation) * prevRot;
        angles = rotDelta.eulerAngles;
        angles.x *= -1;
        rotDelta.eulerAngles = angles;
        transform.rotation *= rotDelta;
        prevRot = src.rotation;
    }

    private Vector3 GetGroundCenter(Vector3 a, Vector3 b)
    {
        Vector3 c = a + (b - a) / 2;
        c.y = 0;
        return c;
    }
}
