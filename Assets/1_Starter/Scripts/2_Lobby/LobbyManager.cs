using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum LobbyModes{Game, Plain};
    [Header("Lobby Mode")]
    public LobbyModes lobbyMode;

    //Methods to keep track of networked players
    [HideInInspector]
    public static LobbyManager instance;
    [HideInInspector]
    public List<GameLobbyPlayerHandler> OtherGamePlayerList;
    [HideInInspector]
    public List<PlainLobbyPlayerHandler> OtherPlainPlayerList;

    [Header("Room Text")]
    [SerializeField] private TMP_Text lobbyHeader; //Room Header Text

    [Header("Buttons")]
    [SerializeField] private GameObject startButton = null;     //Start button that is only available to the host
    [SerializeField] private GameObject muteButton = null;      //Mute/Speak toggle for all users

    [Header("Instruction Text")]
    [SerializeField] private TMP_Text instructionText = null;     //General Instruction Text
    public string instructionStringGuest = "\nPress 'Speak' to speak \n\nMove using the joystick on the right \n\nThe Host will start the room soon";
    public string instructionStringHost = "\nPress 'Speak' to speak \n\nMove using the joystick on the right \n\nPress 'Start' once all people have joined";

    [Header("Build index for Main Scene")]
    public int mainSceneIndex = 2;

    private GameObject lobbyPlayerRef; // The local player

    [Header("Local Player Data")]
    public GameObject lobbyPlayerHolder; // Gameobject controller by the Joystick
    bool isPlayerPlaced; 
    public int maxSpawnAttempts = 10;
    public float spawnCheckRadius = 0.8f;


    private void Awake()
    {
        if (instance is null)
            instance = this;
        else
            Destroy(instance);
    }

    private void Start()
    {

        //Initialise values
        isPlayerPlaced = false;
        OtherGamePlayerList = new List<GameLobbyPlayerHandler>();
        OtherPlainPlayerList = new List<PlainLobbyPlayerHandler>();
        lobbyHeader.text = PhotonNetwork.CurrentRoom.Name.Split('_')[1] + '\n'; //Add Room name to Lobby Header       
        muteButton.GetComponentInChildren<TMP_Text>().text = "Speak";


        if (PhotonNetwork.IsMasterClient) //If Host, allow them to start, display host instructions
        {
            SetStartButton(true);
            instructionText.SetText(instructionStringHost);
        }
        else                              //If Guest, don't allow them to start, display guest instructions
        {
            SetStartButton(false);
            instructionText.SetText(instructionStringGuest);
        }

        if(lobbyMode == LobbyModes.Game)
        {
            //Instantiate local player
            lobbyPlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "LobbyGameAvatar"), lobbyPlayerHolder.transform.position, lobbyPlayerHolder.transform.rotation);

            //Attach local player to joystick-controller object
            lobbyPlayerRef.transform.parent = lobbyPlayerHolder.transform;

            //Move to a random position on the board
            lobbyPlayerHolder.transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));
        }
        else
        if (lobbyMode == LobbyModes.Plain)
        {
            //Instantiate local player
            lobbyPlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlainLobbyAvatar"), Vector3.one, Quaternion.identity);

            //Attach local player to joystick-controller object
            lobbyPlayerRef.transform.parent = lobbyPlayerHolder.transform;

            //Scale the UI element
            lobbyPlayerHolder.transform.localScale = Vector3.one;

        }
        
        UpdatePlayerCount();

    }

    private void Update()
    {

        if(lobbyMode == LobbyModes.Game)
        {
            if (!isPlayerPlaced)
            {
                isPlayerPlaced = true;
                bool validPosition = false; //Check whether the gameobject intersects others at least once
                int spawnAttempts = 0;

                while (!validPosition && spawnAttempts < maxSpawnAttempts)
                {
                    spawnAttempts++;
                    lobbyPlayerHolder.transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));
                    validPosition = true;

                    foreach (GameLobbyPlayerHandler player in OtherGamePlayerList)
                    {
                        if (Vector3.Distance(player.gameObject.transform.position, lobbyPlayerHolder.gameObject.transform.position) < spawnCheckRadius) // Make sure local player is far from all other players
                        {
                            validPosition = false;
                        }
                    }
                }
            }
        }

    }

    private void SetStartButton(bool isActive)
    {
        //Make Start Button Interactable
        if (isActive)
        {
            startButton.GetComponentInChildren<TMP_Text>().text = "Start";
            startButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            startButton.GetComponentInChildren<TMP_Text>().text = "Wait...";
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    private void UpdatePlayerCount()
    {
        lobbyHeader.text = PhotonNetwork.CurrentRoom.Name.Split('_')[1] + '\n' + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString() + " people";
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

    IEnumerator WaitForPlayerUpdate() //To allow for Photon lag
    {
        //Pause for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        UpdatePlayerCount();
    }

    public void ToggleMute() //Only meant for the lobby scenes
    {
        if (PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled == true)
        {
            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false;
            muteButton.GetComponentInChildren<TMP_Text>().text = "Speak";
        }
        else
        {
            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = true;
            muteButton.GetComponentInChildren<TMP_Text>().text = "Mute";
        }
    }

    public void StartMainScene()
    {
        
        //Only allow the Host to start the scene
        if (!PhotonNetwork.IsMasterClient)
            return;

        //Close the room so no other players can join
        //PhotonNetwork.CurrentRoom.IsOpen = false;       
        
        /*
        //Testing other lobby player deletion
         
        if(lobbyMode == LobbyModes.Game)
        {
            foreach (GameLobbyPlayerHandler p in OtherGamePlayerList)
            {
                if (p.gameObject)
                {
                    p.gameObject.SetActive(false);
                }
                PhotonNetwork.Destroy(p.gameObject);
            }

        }
        else
        if(lobbyMode == LobbyModes.Plain)
        {
            foreach (PlainLobbyPlayerHandler p in OtherPlainPlayerList)
            {
                if (p.gameObject)
                {
                    p.gameObject.SetActive(false);
                }
                PhotonNetwork.Destroy(p.gameObject);
            }

        }
        */

        //Deleting local lobby player
        //PhotonNetwork.Destroy(lobbyPlayerRef.gameObject);

        PhotonNetwork.LoadLevel(mainSceneIndex);

    }

}
