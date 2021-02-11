using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Realtime;

public class PlainLobbyManager : MonoBehaviourPunCallbacks
{

    public PhotonView managerPhotonView;
    public static PlainLobbyManager instance;

    [HideInInspector]
    public List<PlainLobbyPlayerHandler> OtherPlayerList;


    //Room Text
    [Header("Room Text")]
    [SerializeField] private TMP_Text lobbyHeader;

    //Start button that is only available to the host
    [Header("Buttons")]
    [SerializeField] private GameObject startButton = null;
    [SerializeField] private GameObject muteButton = null;
    [SerializeField] private GameObject sceneButton = null;

    //Index of the AR Room Scene
    [SerializeField] private int[] SceneIndices;
    private int currentIndex;
    private int nextSceneIndex;
    [SerializeField] private string[] SceneNames;

    [Header("Lobby Objects")]
    public GameObject lobbyPlayerHolder;
    private GameObject lobbyPlayerRef;
    bool isPlayerPlaced;
    
    //private List<GameObject> lobbyPlayers;

    private void Awake()
    {
        if (instance is null)
            instance = this;
        else
            Destroy(instance);
    }

    private void Start()
    {

        managerPhotonView = GetComponent<PhotonView>();

        isPlayerPlaced = false;
        OtherPlayerList = new List<PlainLobbyPlayerHandler>();

        lobbyHeader.text = PhotonNetwork.CurrentRoom.Name.Split('_')[1] + '\n'; //Add Room name to Lobby Header

        //Make sure all players move to the next scene together
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsMasterClient)
        {
            SetStartButton(true);
            sceneButton.SetActive(true);
        }
        else
        {
            SetStartButton(false);
            sceneButton.SetActive(false);
        }

        muteButton.GetComponentInChildren<TMP_Text>().text = "Speak";

        nextSceneIndex = SceneIndices[0];
        currentIndex = 0;
        sceneButton.GetComponentInChildren<TMP_Text>().text = "Scene " + nextSceneIndex;

        lobbyPlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlainLobbyAvatar"), Vector3.one, Quaternion.identity);

        lobbyPlayerRef.transform.parent = lobbyPlayerHolder.transform;
        lobbyPlayerHolder.transform.localScale = Vector3.one;

     


        UpdatePlayerCount();

    }

    private void Update()
    {
      
    }

    private void SetStartButton(bool isActive)
    {
        //Make Start Button Interactable
        if (isActive)
        {
            startButton.GetComponent<Button>().interactable = true;
            startButton.GetComponent<Button>().enabled = true;
            startButton.GetComponentInChildren<TMP_Text>().text = "Start";
            startButton.GetComponent<Button>().onClick.AddListener(StartMainScene);

        }
        else
        {
            //startButton.SetActive(false);
            startButton.GetComponentInChildren<TMP_Text>().text = "Wait...";
            startButton.GetComponent<Image>().color = Color.gray;
            startButton.GetComponent<Button>().interactable = false;
            startButton.GetComponent<Button>().enabled = false;

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

    IEnumerator WaitForPlayerUpdate()
    {

        Debug.Log("Waiting for player to join room");

        //Pause for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        UpdatePlayerCount();
    }

    public void ToggleMute()
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

    public void GoToNextSceneIndex()
    {
        currentIndex++;
        if(currentIndex>=SceneIndices.Length)
        {
            currentIndex = 0;
        }
        nextSceneIndex = SceneIndices[currentIndex];
        sceneButton.GetComponentInChildren<TMP_Text>().text = "Scene " + nextSceneIndex;
    }

    public void StartMainScene()
    {

        //Only allow the Host to start the scene
        if (!PhotonNetwork.IsMasterClient)
            return;
        //Close the room so no other players can join
        //PhotonNetwork.CurrentRoom.IsOpen = false;

        foreach(PlainLobbyPlayerHandler p in OtherPlayerList)
        {
            if(p.gameObject)
            {
                p.gameObject.SetActive(false);
            }
            //PhotonNetwork.Destroy(p.gameObject);
        }

        lobbyPlayerRef.gameObject.SetActive(false);
        //PhotonNetwork.Destroy(lobbyPlayerRef.gameObject);

        PhotonNetwork.LoadLevel(nextSceneIndex);

    }
}
