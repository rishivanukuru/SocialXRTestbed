using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using TMPro;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

public class BasicStartController : MonoBehaviourPunCallbacks
{

    [Header("Start Screen")]
    [SerializeField] private GameObject hostButton = null;
    [SerializeField] private GameObject guestButton = null;
    [SerializeField] private TMP_Text startText = null; // Main instruction
    public string startMessage; //Message while connecting to Photon servers
    public string connectedMessage; //Message after a connection is established

    [Header("Host")]
    [SerializeField] private TMP_InputField hostNameInput;
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_InputField roomSizeInput;

    [Header("Guest")]
    [SerializeField] private TMP_InputField guestNameInput;
    [SerializeField] private TMP_InputField guestRoomNameInput;

    [Header("Lobby Scene")]
    [SerializeField] private int lobbySceneIndex = 1;

    private void Awake()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif

    }

    private void Start()
    {
        //Initialising Static Variables
        PlayerDataHolder.isHost = false;
        PlayerDataHolder.playerName = "Player";

        //Setting up Start Screen
        startText.text = startMessage;
        hostButton.SetActive(false);
        guestButton.SetActive(false);

        //Beginning connection to Photon Servers
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We're now connected to a Server on - " + PhotonNetwork.CloudRegion.ToString());

        //Photon Connection established, user can now choose to be a host or a guest

        startText.text = connectedMessage;
        hostButton.SetActive(true);
        guestButton.SetActive(true);
    }

    public void HostStartAndJoinRoom()
    {
        string roomName = "Room_" + roomNameInput.text; //To ensure the room name isn't empty
        int roomSize = int.Parse(roomSizeInput.text)>0? int.Parse(roomSizeInput.text) : 2; //Default size is 2, also can't be less than 1
        CreateRoom(roomName, roomSize);
        SetPlayerAsHost();
    }

    public void GuestJoinRoom()
    {
        string roomName = "Room_" + guestRoomNameInput.text; //To ensure the room name isn't empty
        PhotonNetwork.JoinRoom(roomName);
        SetPlayerAsGuest();
    }

    public void CreateRoom(string roomName, int roomSize)
    {
        Debug.Log("Creating Room Now");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
     
        //Set player nickname
        if(PlayerDataHolder.isHost)
        {
            PhotonNetwork.LocalPlayer.NickName = hostNameInput.text;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = guestNameInput.text;
        }


        //move to next scene
        PhotonNetwork.LoadLevel(lobbySceneIndex);
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Create Room.");
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room does not exist.");
        base.OnJoinRoomFailed(returnCode, message);
    }

    //Setting up static variables for use across scenes

    public void SetPlayerName(string playerName)
    {
        PlayerDataHolder.playerName = playerName;
    }

    public void SetPlayerAsHost()
    {
        PlayerDataHolder.isHost = true;
    }

    public void SetPlayerAsGuest()
    {
        PlayerDataHolder.isHost = false;
    }

    


}
