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

public class IntroManager : MonoBehaviourPunCallbacks
{

    //Script that controls the creation of rooms and allows users to join open ones

    [Header("Start Screen")]
    [SerializeField] private GameObject hostButton = null;      
    [SerializeField] private GameObject guestButton = null;
    [SerializeField] private GameObject introText = null;

    [Header("Host")]
    [SerializeField] private TMP_InputField hostNameInput;
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_InputField roomSizeInput;
    [SerializeField] private TMP_Text createButtonText;

    [Header("Guest")]
    [SerializeField] private TMP_InputField guestNameInput;
    [SerializeField] private TMP_Text guestInstructions;
    [SerializeField] private TMP_InputField guestRoomNameInput;

    [Header("Password")]
    [SerializeField] private string password = "123";
    public TMP_InputField passwordInput = null;

    [Header("Lobby Scene")]
    [SerializeField] private int lobbySceneIndex = 1;

    private void Awake()
    {
        //Checking for permissions, as this is the first custom script called in the app
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

        //Setting up Start Screen
        //Turning off the join/host buttons until a connection to Photon is established
        introText.SetActive(true);
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
        introText.SetActive(false);
        hostButton.SetActive(true);
        guestButton.SetActive(true);
    }

    public void HostStartAndJoinRoom()
    {
        if(passwordInput.text==password) //Restricting Room Creation Access
        {
            string roomName = "Room_" + roomNameInput.text; //To ensure the room name isn't empty
            int roomSize = int.Parse(roomSizeInput.text) > 0 ? int.Parse(roomSizeInput.text) : 2; //Default size is 2, also can't be less than 1
            CreateRoom(roomName, roomSize);
        }
       
    }

    public void GuestJoinRoom()
    {
        string roomName = "Room_" + guestRoomNameInput.text; //To ensure the room name isn't empty
        PhotonNetwork.JoinRoom(roomName);
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

        PhotonNetwork.AutomaticallySyncScene = true;                //CHECK!!


        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LocalPlayer.NickName = hostNameInput.text;    //Sets the Host Photon Nickname
            PhotonNetwork.LoadLevel(lobbySceneIndex);                   //Loads the lobby, as per the index in the inspector
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = guestNameInput.text;   //Sets the Guest Photon Nickname
        }        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        createButtonText.text = "Error, try again...";
        Debug.Log("Failed to Create Room.");
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        guestInstructions.text = "Room not open yet, please check the room name.";      //Error message in UI
        Debug.Log("Room does not exist.");
        base.OnJoinRoomFailed(returnCode, message);
    }




}
