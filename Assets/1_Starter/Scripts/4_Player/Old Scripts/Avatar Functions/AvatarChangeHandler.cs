using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;
using TMPro;

public class AvatarChangeHandler : MonoBehaviourPun
{


    public GameObject avatarHolder;
    public GameObject[] playerAvatars;
    public int currentAvatar;
    private GameObject body;
    [HideInInspector]
    public bool displayAvatar;

    public GameObject headHandsPhoneIK;
    public GameObject upperBodyPhoneIK;
    public GameObject fullBodyPhoneIK;


    [HideInInspector]
    public GameObject mainPlacedObject;
    [HideInInspector]
    public RigBuilder mainRig;
    [HideInInspector]
    public GameObject mainIKTarget;

    private PhotonView myPhotonView;

    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();


        //avatarHolder.SetActive(false);

        foreach (GameObject avatar in playerAvatars)
        {
            avatar.SetActive(false);
        }

        playerAvatars[0].SetActive(true);

        //mainPlacedObject = null;
        //mainRig = null;
        //mainIKTarget = null;

        currentAvatar = 0;

        if(photonView.IsMine)
        {
            displayAvatar = false;
        }
        else
        {
            displayAvatar = true;
        }
    }

    public void HideAvatar()
    {
        avatarHolder.SetActive(false);
    }

    public void ShowAvatar()
    {
        avatarHolder.SetActive(true);
    }

    public void ChangeAvatarDisplay(int index)
    {
        playerAvatars[currentAvatar].SetActive(false);
        currentAvatar = index;
        if (currentAvatar >= playerAvatars.Length)
        {
            currentAvatar = 0;
        }
        playerAvatars[currentAvatar].SetActive(true);

        /*
        if (displayAvatar)
        {
            if (currentAvatar == 2)
            {
                InstantiateIK(headHandsPhoneIK);
            }
            else
            if (currentAvatar == 3)
            {
                DestroyCurrentIK();
                InstantiateIK(upperBodyPhoneIK);
            }
            else
            if (currentAvatar == 4)
            {
                DestroyCurrentIK();
                InstantiateIK(fullBodyPhoneIK);
            }
            else
            {
                DestroyCurrentIK();
            }
        }
        */
    }


    public void DisplayNextAvatar()
    {
        playerAvatars[currentAvatar].SetActive(false);
        currentAvatar++;
        if (currentAvatar >= playerAvatars.Length)
        {
            currentAvatar = 0;
        }
        playerAvatars[currentAvatar].SetActive(true);
    }


    void InstantiateIK(GameObject prefabIK)
    {
        if (displayAvatar)
        {

            Quaternion startRotation = Quaternion.Euler(0, avatarHolder.transform.rotation.eulerAngles.y, 0);

            mainPlacedObject = Instantiate(prefabIK, avatarHolder.transform.position, startRotation);
            mainRig = mainPlacedObject.GetComponentInChildren<RigBuilder>();

            var humanoidControllerRef = mainRig.GetComponent<HumanoidController>();
            humanoidControllerRef.SetDependeciesBasic(mainPlacedObject.transform.GetChild(0), mainPlacedObject.transform.GetChild(1));

            //humanoidControllerRef.SetDependecies(mainPlacedObject.GetComponentInChildren<BoundsManager>(), mainPlacedObject.transform.GetChild(0), mainPlacedObject.transform.GetChild(1));


            mainIKTarget = mainPlacedObject.transform.GetChild(0).gameObject;

            mainIKTarget.transform.parent = avatarHolder.transform;
            mainIKTarget.transform.position = avatarHolder.transform.position;
            mainIKTarget.transform.rotation = avatarHolder.transform.rotation;


            mainRig.gameObject.transform.parent = null;
            mainRig.enabled = true;


        }
    }

    void DestroyCurrentIK()
    {
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
