using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class XRRoomManager : MonoBehaviourPunCallbacks
{
    //Initializing variables for the Singleton
    public static XRRoomManager instance;
    PhotonView myPhotonView;

    //Local Player
    [HideInInspector]
    public GameObject PlayerRef; //Local Player GameObject Reference

    //Reference Object
    [HideInInspector]
    public GameObject referenceObject; //Space Reference Object (Table)
    [HideInInspector]
    public Vector3 RefPosition; //Initial position of Reference Object
    [HideInInspector]
    public bool isPlaced = false; //Flag to check if reference object has been placed

    //Other Players
    [HideInInspector]
    public List<XRPlayerHandler> OtherPlayerList; //List of other players in the room


    [Header("Room Info")]
    public TMP_Text roomInfo; //UI Text to display room info and number of participants

    [Header("Networked Player Prefab")]
    public GameObject playerPrefab;

    [Header("Main XR Object")]
    public GameObject spawnPrefab;
    public ARPlacementInteractableSingle arPlacementInteractable;
    [HideInInspector]
    public GameObject spawnedObject; 


    [Header("AR Player Start Transform")]
    public GameObject camera_AR;

    [Header("Screen Player Start Transform")]
    public GameObject playerTransformScreen;
    [HideInInspector]
    public GyroCamera playerGyro;

    [Header("Screen mode reference origin")]
    public GameObject screenOrigin;

    [Header("Player Rotation Offset")]
    public float offsetAngle = 20f;

    private bool isLocked;
    private bool displayOtherAvatars;

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
        

        //Initializing flags !CHECK IF NEEDED
        isLocked = false;
        displayOtherAvatars = true;

        myPhotonView = GetComponent<PhotonView>();

        //Extracting script reference for Gyroscope
        playerGyro = playerTransformScreen.GetComponent<GyroCamera>();

        if (roomInfo != null)
            roomInfo.text = PhotonNetwork.CurrentRoom.Name.Split('_')[1] + '\n'; //Add Room name to Info Text
        
        OtherPlayerList = new List<XRPlayerHandler>();
        UpdatePlayerCount();
    }

    private void Update()
    {
        /*
         * START OF AR MODE CODE
         */
        if(!isPlaced)
        {
            if(arPlacementInteractable.isObjectPlaced) //If AR object has been placed
            {
                isPlaced = true;
                arPlacementInteractable.placementObject.transform.Rotate(Vector3.up, -offsetAngle * XRRoomManager.instance.OtherPlayerList.Count, Space.Self); //Rotate to space players out
                referenceObject = arPlacementInteractable.placementObject; //Assign reference object
                isLocked = true; //Set object lock flag
                SpawnPlayer(); //Spawn local player
            }
        }


        //Handling Reference Object Lock
        if(isLocked && !arPlacementInteractable.isLocked)
        {
            arPlacementInteractable.isLocked = true;
        }
        if (!isLocked && arPlacementInteractable.isLocked)
        {
            arPlacementInteractable.isLocked = false;
        }
        /*
         * END OF AR MODE CODE
         */
    }

    public void SpawnPlayer()
    {

        Transform cam;
        cam = camera_AR.transform;

        PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerPrefab.name), cam.position, cam.rotation);
        PlayerRef.transform.parent = cam; //Instantiate Avatar and attach to phone's position (AR Camera)

        if (PhotonVoiceNetwork.Instance.ClientState == Photon.Realtime.ClientState.Joined) //Setup voice comms
        {
            Debug.Log("Joined Voice");
            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false; //Start off as muted
        }

        foreach (XRPlayerHandler p in OtherPlayerList)
        {
            p.MakeVisible(); //Make other joined players visible
        }

    }

    public void SpawnPlayerScreen()
    {
        playerGyro.CalibrateFunction(); // Calibrate forward direction
        StartCoroutine(WaitForSpawnUpdate());
    }

    public void SpawnPlayerObjectScreen()
    {
        Transform cam;
        cam = playerTransformScreen.transform;
        PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerPrefab.name), cam.position, cam.rotation);
        PlayerRef.transform.parent = playerTransformScreen.transform; //Instantiate Avatar and attach to screen mode movement gameobject

        if (PhotonVoiceNetwork.Instance)
        {
            if (PhotonVoiceNetwork.Instance.ClientState == Photon.Realtime.ClientState.Joined)
            {
                Debug.Log("Joined Voice");
                PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false;
            }
        }

        foreach (XRPlayerHandler p in OtherPlayerList)
        {
            p.MakeVisible();
        }

        isPlaced = true;
    }

    public void SpawnPlayerRefScreen()
    {
        spawnedObject = Instantiate(spawnPrefab, screenOrigin.transform.position, screenOrigin.transform.rotation);
        spawnedObject.transform.Rotate(Vector3.up, -offsetAngle * XRRoomManager.instance.OtherPlayerList.Count, Space.Self); //Rotate reference object to space players out
        referenceObject = spawnedObject;
    }

    IEnumerator WaitForSpawnUpdate()
    {
        //Pause for 0.5 seconds
        yield return new WaitForSeconds(0.1f);
        SpawnPlayerRefScreen();
        SpawnPlayerObjectScreen();
    }

    public void ObjectLockToggle()
    {
        isLocked = isLocked ? false : true;
    }

    public bool isObjectLocked()
    {
        return isLocked;
    }
    
    private void UpdatePlayerCount()
    {
        if (roomInfo != null)
            roomInfo.text = PhotonNetwork.CurrentRoom.Name.Split('_')[1] + ": " + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString() + " people";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player Entered Room - " + newPlayer.NickName);
        base.OnPlayerEnteredRoom(newPlayer);
        StartCoroutine(WaitForPlayerUpdate());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Other Player Left Room - " + otherPlayer.NickName);
        base.OnPlayerLeftRoom(otherPlayer);
        StartCoroutine(WaitForPlayerUpdate());
    }

    IEnumerator WaitForPlayerUpdate()
    {
        //Pause for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        UpdatePlayerCount();
    }

}
