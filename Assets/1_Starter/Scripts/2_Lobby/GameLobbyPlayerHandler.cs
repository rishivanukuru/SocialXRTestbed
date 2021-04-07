using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLobbyPlayerHandler : MonoBehaviourPun
{
    void Start()
    {
        if(LobbyManager.instance==null)
        {
            this.gameObject.SetActive(false); //To avoid the ghost avatar shells from appearing
            //Destroy(this.gameObject);
        }
        else
        if(!photonView.IsMine)
        {
            LobbyManager.instance.OtherGamePlayerList.Add(this);
        }
    }
}
