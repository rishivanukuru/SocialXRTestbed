using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine.UI;

public class BasicARSessionManager : MonoBehaviour
{
    //For the singleton
    public static BasicARSessionManager instance;

    [HideInInspector]
    public GameObject PlayerRef;

    [HideInInspector]
    public Vector3 RefPosition;

    [HideInInspector]
    public bool isPlaced = false;

    [HideInInspector]
    public List<PlayerHandler> OtherPlayerList;

    public GameObject referenceObject;
    public GameObject spawnPrefab;
    public GameObject spawnedObject;

    private PlayerHandler playerHandler;

    [Header("AR Player Start Transform")]
    public GameObject camera_AR;

    [Header("PC Player Start Transform")]
    public GameObject playerTransformPC;



    public bool isAR = true;

    private void Awake()
    {

        //Setting singleton
        if (instance is null)
            instance = this;
        else
            Destroy(instance);
    }

    private void Start()
    {
        OtherPlayerList = new List<PlayerHandler>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }
    }


    public void SpawnPlayer()
    {

        Transform cam;

        if (isAR)
        {
            cam = camera_AR.transform;
        }
        else
        {
            cam = playerTransformPC.transform;
        }

        PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), cam.position, cam.rotation);
        PlayerRef.transform.parent = cam;

        if (PhotonVoiceNetwork.Instance.ClientState == Photon.Realtime.ClientState.Joined)
        {
            //muteButton.SetActive(true);
            Debug.Log("Joined Voice, Button Active.");
            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false;
            //muteButtonText.text = "speak";
        }

       

        foreach (PlayerHandler p in OtherPlayerList)
            p.MakeVisible();

        playerHandler = PlayerRef.GetComponent<PlayerHandler>();
       

        isPlaced = true;
    }

    public void ToggleMute()
    {
        if(PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled)
        {
            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false;
            //muteButtonText.text = "speak";
        }
        else
        {
            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = true;
            //muteButtonText.text = "mute";
        }
    }


}
