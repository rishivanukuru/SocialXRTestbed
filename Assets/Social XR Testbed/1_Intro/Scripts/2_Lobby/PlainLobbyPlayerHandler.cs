using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Realtime;

public class PlainLobbyPlayerHandler : MonoBehaviour
{
    public PhotonView playerPhotonView;
    public TMP_Text playerName;
    public Button roleButton;
    public TMP_Text roleText;

    public string currentRoleName;
    public int currentRoleIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerName.text = " " + playerPhotonView.Owner.NickName;
        this.gameObject.transform.parent = PlainLobbyManager.instance.lobbyPlayerHolder.transform;
        this.gameObject.transform.localScale = Vector3.one;

        PlainLobbyManager.instance.OtherPlayerList.Add(this);

        if(PhotonNetwork.IsMasterClient)
        {
            roleButton.gameObject.SetActive(true);
        }
        else
        {
            roleButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
