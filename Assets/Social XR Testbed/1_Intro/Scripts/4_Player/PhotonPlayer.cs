using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using Photon.Pun;
using Photon.Realtime;


public class PhotonPlayer : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView = null;

    public GameObject myAvatar = null;



    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();

        if(myPhotonView.IsMine)
        {
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), ARBasicGameSetup.GS.spawnPoint.position, ARBasicGameSetup.GS.spawnPoint.rotation);
            //myAvatar.transform.parent = ARBasicGameSetup.GS.arCamera.transform;

        }
        else
        {

        }

    }

    private void Update()
    {
        if (myPhotonView.IsMine)
        {
            //myAvatar.transform.position = ARBasicGameSetup.GS.arCamera.transform.position;
            //myAvatar.transform.rotation = ARBasicGameSetup.GS.arCamera.transform.rotation;
        }
        
    }




}
