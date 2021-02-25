using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BasicGridPoint : MonoBehaviour
{
    MeshRenderer myRenderer;
    SphereCollider myCollider;
    bool hasRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<MeshRenderer>()!=null)
        {
            myRenderer = GetComponent<MeshRenderer>();
            myRenderer.material.color = Color.black;
            myRenderer.enabled = false;
            hasRenderer = true;
        }
        if (GetComponent<SphereCollider>() != null)
        {
            myCollider = GetComponent<SphereCollider>();
            myCollider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("guidesphere") && hasRenderer)
        {
            myRenderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("guidesphere") && hasRenderer)
        {
            myRenderer.enabled = false;
        }
    }
}
