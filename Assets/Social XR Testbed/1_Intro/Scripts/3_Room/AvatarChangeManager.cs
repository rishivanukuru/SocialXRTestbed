using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarChangeManager : MonoBehaviour
{

    [Header("Photon View")]
    PhotonView myPhotonView;
    

    [Header("Old Avatar UI")]
    public GameObject avatarButton;
    public GameObject avatarButtonAR;

    [Header("New Avatar UI")]
    public bool isNewUI;
    public GameObject hostPanel;
    public GameObject hostAvatarButton;
    public GameObject hostObjectButton;
    public int avatarNumber = 4;
    public int currentAvatar;
    public int objectNumber = 4;
    public int currentObject;


    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        currentAvatar = 0;
        currentObject = 0;

        //avatarButton.SetActive(false);
        //avatarButtonAR.SetActive(false);
        hostPanel.SetActive(false);


        if (!PhotonNetwork.IsMasterClient)
        {
    
           
            if (isNewUI)
            {
                hostPanel.SetActive(false);
            }
            else
            {
                avatarButton.SetActive(false);
                avatarButtonAR.SetActive(false);
            }

        }
        else
        {


            if (isNewUI)
            {
                hostPanel.SetActive(true);
                hostAvatarButton.GetComponent<Button>().onClick.AddListener(ChangeAllAvatars);
                hostObjectButton.GetComponent<Button>().onClick.AddListener(ChangeTableObject);
                
            }
            else
            {
                avatarButton.SetActive(true);
                avatarButtonAR.SetActive(true);
                avatarButton.GetComponent<Button>().onClick.AddListener(ChangeAllAvatars);
                avatarButtonAR.GetComponent<Button>().onClick.AddListener(ChangeAllAvatars);
            }
        }

        
    }

    public void ChangeTableObject()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentObject++;
            if (currentObject >= RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().tableObjects)
            {
                currentObject = 0;
            }
            myPhotonView.RPC("RPCChangeTableObject", RpcTarget.AllBuffered, currentObject);
        }
    }

    public void ChangeAllAvatars()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentAvatar++;
            if (currentAvatar >= avatarNumber)
            {
                currentAvatar = 0;
            }
            myPhotonView.RPC("RPCChangeAvatar", RpcTarget.All, currentAvatar);
        }

    }

    [PunRPC]
    void RPCChangeAvatar(int index)
    {
        foreach (PlayerHandler p in RoomManager.instance.OtherPlayerList)
        {
            p.myAvatars.ChangeAvatarDisplay(index);
        }

        //RoomManager.instance.PlayerRef.GetComponent<PlayerHandler>().myAvatars.ChangeAvatarDisplay(index);
    }

    [PunRPC]
    void RPCChangeTableObject(int index)
    {
        RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().NextObject(index);
    }

}
