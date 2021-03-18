using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

public class ARBasicRoomSetup : MonoBehaviourPunCallbacks
{

    public static ARBasicRoomSetup GS;

    public GameObject arCamera;
    public Transform spawnPoint;


    private void OnEnable()
    {
        if (GS == null)
        {
            GS = this;
        }
    }

}
