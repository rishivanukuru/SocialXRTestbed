using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class RoomSetupController : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        Debug.Log("Created Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }

}
