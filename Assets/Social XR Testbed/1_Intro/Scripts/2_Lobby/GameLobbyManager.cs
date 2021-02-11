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

public class GameLobbyManager : MonoBehaviourPunCallbacks
{


    public static GameLobbyManager instance;

    [HideInInspector]
    public List<GameLobbyPlayerHandler> OtherPlayerList;


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
    bool isPlayerPlaced;
    public int maxSpawnAttempts = 10;
    public float spawnCheckRadius = 0.5f;

    private GameObject lobbyPlayerRef;

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

        isPlayerPlaced = false;
        OtherPlayerList = new List<GameLobbyPlayerHandler>();

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
            sceneButton.SetActive(true);
        }

        muteButton.GetComponentInChildren<TMP_Text>().text = "Speak";

        nextSceneIndex = SceneIndices[0];
        currentIndex = 0;
        //sceneButton.GetComponentInChildren<TMP_Text>().text = "Scene " + nextSceneIndex;

        lobbyPlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "LobbyGameAvatar"), lobbyPlayerHolder.transform.position, lobbyPlayerHolder.transform.rotation);
        lobbyPlayerRef.transform.parent = lobbyPlayerHolder.transform;
        lobbyPlayerHolder.transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.25f, Random.Range(-3f, 3f));

        Debug.Log(lobbyPlayerHolder.transform.localPosition.ToString());

        UpdatePlayerCount();

    }

    private void Update()
    {
        if(!isPlayerPlaced)
        {


            isPlayerPlaced = true;
            


            
            bool validPosition = false;

            int spawnAttempts = 0;



            while(!validPosition && spawnAttempts < maxSpawnAttempts)
            {
                spawnAttempts++;

                lobbyPlayerHolder.transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.25f, Random.Range(-3f, 3f));

                validPosition = true;

                foreach(GameLobbyPlayerHandler player in OtherPlayerList)
                {
                    if(Vector3.Distance(player.gameObject.transform.position, lobbyPlayerHolder.gameObject.transform.position)<0.8)
                    {
                        validPosition = false;
                    }
                }

                /*

                lobbyPlayerHolder.transform.localPosition = new Vector3(Random.Range(-4.5f, 4.5f), 0, Random.Range(-4.5f, 4.5f));

                Debug.Log(lobbyPlayerHolder.transform.localPosition.ToString());

                Collider[] colliders = Physics.OverlapSphere(lobbyPlayerHolder.transform.position, spawnCheckRadius);

                validPosition = true;

                foreach (Collider col in colliders)
                {
                    if (col.tag == "Player")
                    {
                        validPosition = false;
                        Debug.Log("Collision with player detected");
                    }
                }

                if (validPosition)
                {
                    isPlayerPlaced = true;
                    lobbyPlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "LobbyGameAvatar"), lobbyPlayerHolder.transform.position, lobbyPlayerHolder.transform.rotation);
                    lobbyPlayerRef.transform.parent = lobbyPlayerHolder.transform;
                }

              */
            }
            

        }


    }

    private void SetStartButton(bool isActive)
    {
        //Make Start Button Interactable
        if (isActive)
        {
            startButton.GetComponent<Button>().interactable = true;
            startButton.GetComponent<Button>().enabled = true;
            startButton.GetComponentInChildren<TMP_Text>().text = "Start";

        }
        else
        {
            //startButton.SetActive(false);
            startButton.GetComponentInChildren<TMP_Text>().text = "Waiting...";
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
        //sceneButton.GetComponentInChildren<TMP_Text>().text = "Scene " + nextSceneIndex;
    }

    public void StartMainScene()
    {

        //Only allow the Host to start the scene
        if (!PhotonNetwork.IsMasterClient)
            return;
        //Close the room so no other players can join
        //PhotonNetwork.CurrentRoom.IsOpen = false;

        foreach(GameLobbyPlayerHandler p in OtherPlayerList)
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
