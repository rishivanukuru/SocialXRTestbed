using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Realtime;

public class PlainLobbyPlayerHandler : MonoBehaviourPun
{
    //Skeleton script for a UI that allows the host to assign roles to people in the room

    [Header("Player Photon View")]
    public PhotonView playerPhotonView;

    [HideInInspector]
    public TMP_Text playerName;
    [Header("Player UI details")]
    public Button roleButton;
    public TMP_Text roleText;

    //Variables that haven't really been used yet
    [HideInInspector]
    public string currentRoleName;
    [HideInInspector]
    public int currentRoleIndex;

    void Start()
    {
        if (LobbyManager.instance == null)
        {
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
        else
        {
            if (!photonView.IsMine)
                LobbyManager.instance.OtherPlainPlayerList.Add(this);
        }

        playerName.text = " " + playerPhotonView.Owner.NickName;
        this.gameObject.transform.parent = LobbyManager.instance.lobbyPlayerHolder.transform; //Makes the gameobject a child of the Grid UI
        this.gameObject.transform.localScale = Vector3.one; //Adjusts scale (was throwing an alignment issue otherwise)

        if(PhotonNetwork.IsMasterClient) //Only allow the host to potentially change roles
        {
            roleButton.gameObject.SetActive(true);
        }
        else
        {
            roleButton.gameObject.SetActive(false);
        }
    }

    
}
