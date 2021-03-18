using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
    
public class NetworkControllerBasic : MonoBehaviourPunCallbacks
{
    static bool isConnectedToServer = false;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We're now connected to a Server on - " + PhotonNetwork.CloudRegion.ToString());
        isConnectedToServer = true;
    }

    public void ConnectToPhotonServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

}

