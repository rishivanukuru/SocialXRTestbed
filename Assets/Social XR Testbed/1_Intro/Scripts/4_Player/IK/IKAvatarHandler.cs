using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;
using TMPro;

public class IKAvatarHandler : MonoBehaviourPun
{
    [Header("Hands or Head")]
    public GameObject referencePoint;
    [Header("Prefab")]
    public GameObject prefabIK;


    [HideInInspector]
    public GameObject mainPlacedObject;
    [HideInInspector]
    public RigBuilder mainRig;
    [HideInInspector]
    public GameObject mainIKTarget;
    [HideInInspector]
    public HumanoidController humanoidControllerRef;
    [HideInInspector]
    public BoundsManager boundsManager;
    [HideInInspector]
    public Transform handTarget;
    [HideInInspector]
    public Transform legTarget;

    

    void Awake()
    {
    }

    void Update()
    {
        
    }


    private void OnEnable()
    {
        StartIK();
    }


    private void OnDisable()
    {
        StopIK();
    }


    void StartIK()
    {

        Quaternion startRotation = Quaternion.Euler(0, referencePoint.transform.rotation.eulerAngles.y, 0);

        mainPlacedObject = Instantiate(prefabIK, referencePoint.transform.position, startRotation);
        mainRig = mainPlacedObject.GetComponentInChildren<RigBuilder>();

        var humanoidControllerRef = mainRig.GetComponent<HumanoidController>();
        humanoidControllerRef.SetDependeciesBasic(mainPlacedObject.transform.GetChild(0), mainPlacedObject.transform.GetChild(1));


        mainIKTarget = mainPlacedObject.transform.GetChild(0).gameObject;

        mainIKTarget.transform.parent = referencePoint.transform;
        mainIKTarget.transform.position = referencePoint.transform.position;
        mainIKTarget.transform.rotation = referencePoint.transform.rotation;


        mainRig.gameObject.transform.parent = null;
        mainRig.enabled = true;

        /*
        mainRig.gameObject.SetActive(true);
        mainIKTarget.gameObject.SetActive(true);

        Quaternion startRotation = Quaternion.Euler(0, referencePoint.transform.rotation.eulerAngles.y, 0);

        mainPlacedObject.transform.position = referencePoint.transform.position;
        mainPlacedObject.transform.rotation = startRotation;


        humanoidControllerRef.SetDependecies(boundsManager, handTarget, legTarget);

        mainIKTarget.transform.parent = referencePoint.transform;
        mainIKTarget.transform.position = referencePoint.transform.position;
        mainIKTarget.transform.rotation = referencePoint.transform.rotation;

        mainRig.gameObject.transform.parent = null;

        mainRig.enabled = true;
        */
    }

    void StopIK()
    {
        /*
        mainPlacedObject.transform.position = referencePoint.transform.position;
        mainPlacedObject.transform.rotation = referencePoint.transform.rotation;
        //mainRig.gameObject.transform.parent = this.gameObject.transform;
        mainRig.gameObject.SetActive(false);
        mainIKTarget.gameObject.SetActive(false);
        mainRig.enabled = false;
        */

        if (mainPlacedObject != null)
        {
            GameObject temp1 = mainPlacedObject;
            mainPlacedObject = null;
            GameObject.Destroy(temp1);
        }

        if (mainRig != null)
        {
            GameObject temp2 = mainRig.gameObject;
            mainRig = null;
            GameObject.Destroy(temp2);

        }

        if (mainIKTarget != null)
        {
            GameObject temp3 = mainIKTarget;
            mainIKTarget = null;
            GameObject.Destroy(temp3);
        }
    }

}
