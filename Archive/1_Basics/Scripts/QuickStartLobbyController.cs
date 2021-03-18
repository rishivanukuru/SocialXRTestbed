using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject hostButton = null;
    [SerializeField] private GameObject guestButton = null;

    private PhotonView myPhotonView;

    [Header("Server")]
    static bool isConnectedToServer = false;

    [Header("Host")]
    [SerializeField] private TMP_InputField hostNameInput;
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_InputField roomSizeInput;
    public GameObject hostSetupScreen;
    public GameObject hostLobbyScreen;

    [Header("Guest")]
    [SerializeField] private TMP_InputField guestNameInput;
    [SerializeField] private TMP_InputField guestRoomNameInput;
    public GameObject guestLoginScreen;
    public GameObject guestLobbyScreen;


    [Header("Lobby")]
    public TMP_Text hostPlayerCount;
    public TMP_Text hostPlayerNames;
    public TMP_Text guestPlayerCount;
    public TMP_Text guestPlayerNames;
    public string lobbyNameList;

    [Header("Main Scene")]
    [SerializeField] private int LobbySceneIndex = 1;

    bool isHost = false;

    private void Start()
    {
        //myPhotonView = GetComponent<PhotonView>();

        hostButton.SetActive(false);
        guestButton.SetActive(false);
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectUsingSettings();
        isHost = false;
    }

    public void ConnectToPhotonServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We're now connected to a Server on - " + PhotonNetwork.CloudRegion.ToString());
        isConnectedToServer = true;
        PhotonNetwork.AutomaticallySyncScene = true;

        hostButton.SetActive(true);
        guestButton.SetActive(true);

    }

    

    public void HostStartAndJoinRoom()
    {
        string roomName = "Room_" + roomNameInput.text;
        int roomSize = 2 + int.Parse(roomSizeInput.text);       
        CreateRoom(roomName, roomSize);
        //Next Screen for Host
        isHost = true;
    }

    public void GuestJoinRoom()
    {
        string roomName = "Room_" + guestRoomNameInput.text;
        PhotonNetwork.JoinRoom(roomName);
        //Next Screen for Guests
        isHost = false;
    }

    public void CreateRoom(string roomName, int roomSize)
    {
        Debug.Log("Creating Room Now");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Created Room: " + roomName);
        
    }

    public override void OnCreatedRoom()
    {
        //string roomName = "Room_" + guestRoomNameInput.text;
        //PhotonNetwork.JoinRoom(roomName);
        //Debug.Log("Joined Room: " + roomName);
        //base.OnCreatedRoom();
        Debug.Log("Created Room");
    }
    

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        if(isHost)
        {
            GoToHostLobby();
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
            PhotonNetwork.LocalPlayer.NickName = hostNameInput.text;
            lobbyNameList = "Host: " + hostNameInput.text + "\n Guests: ";
            UpdateLobbyDisplay(lobbyNameList);
        }
        else
        {
            GoToGuestLobby();
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
            PhotonNetwork.LocalPlayer.NickName = guestNameInput.text;
            UpdateLobbyDisplay();
        }
        UpdateLobbyDisplay();
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

    public void GoToHostLobby()
    {
        hostSetupScreen.SetActive(false);
        hostLobbyScreen.SetActive(true);
    }

    public void GoToGuestLobby()
    {
        guestLoginScreen.SetActive(false);
        guestLobbyScreen.SetActive(true);
    }

    public void UpdateLobbyDisplay()
    {

        if(isHost)
        {
            hostPlayerCount.text = "Players: " + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        }
        else
        {
            guestPlayerCount.text = "Players: " + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        }
    }

    public void UpdateLobbyDisplay(string lastPlayerName)
    {
        if(isHost)
        {
            hostPlayerCount.text = "Players: " + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            //hostPlayerNames.text = lastPlayerName;
        }
        else
        {
            guestPlayerCount.text = "Players: " + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            //guestPlayerNames.text = lastPlayerName;
        }
        
    }

    [PunRPC]
    private void UpdateLobbyDisplayRPC(string lastPlayerName)
    {
        if (isHost)
        {
            hostPlayerCount.text = "Players: " + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            //hostPlayerNames.text = lastPlayerName;
        }
        else
        {
            guestPlayerCount.text = "Players: " + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            //guestPlayerNames.text = lastPlayerName;
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if (PhotonNetwork.PlayerList.Length > 1)
        {
            lobbyNameList += newPlayer.NickName + " ";
        }
        base.OnPlayerEnteredRoom(newPlayer);
        if(PhotonNetwork.IsMasterClient)
        {
            //myPhotonView.RPC("UpdateLobbyDisplayRPC", RpcTarget.All, lobbyNameList);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyDisplay();
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void StartMainScene()
    {
        if (!isHost || !PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(LobbySceneIndex);
    }


}
