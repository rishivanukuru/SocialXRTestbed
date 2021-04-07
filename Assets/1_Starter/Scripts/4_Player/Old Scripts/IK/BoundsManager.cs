using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{    
    public delegate void PhoneEvent();
    public event PhoneEvent OnPhoneExit,OnPhoneEnter;
    private BoxCollider collider;
    private Vector3 Pos, Size;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        Pos = transform.TransformPoint(collider.center);
        Size = collider.size;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "phoneModel")
            OnPhoneExit?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "phoneModel")
            OnPhoneEnter?.Invoke();
    }

    private void OnDrawGizmos()
    {
        collider = GetComponent<BoxCollider>();
        Gizmos.DrawWireCube(transform.TransformPoint(collider.center), collider.size);
        //Gizmos.DrawWireCube(Pos, Size);
    }
}
