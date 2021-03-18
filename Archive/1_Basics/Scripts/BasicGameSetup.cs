using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

public class BasicGameSetup : MonoBehaviourPunCallbacks
{

    public static BasicGameSetup GS;

    public Transform spawnPoint;


    private void OnEnable()
    {
        if(GS == null)
        {
            GS = this;
        }
    }

}
