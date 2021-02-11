using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLobbyPlayerHandler : MonoBehaviourPun
{
    private bool isActive;

    void Start()
    {
        if (!photonView.IsMine)
        {

            GameLobbyManager.instance.OtherPlayerList.Add(this);
        }

        isActive = true;
    }

    void Update()
    {
        if(isActive && SceneManager.GetActiveScene().buildIndex!=1)
        {
            this.gameObject.SetActive(false);
            isActive = false;
        }
    }


}
