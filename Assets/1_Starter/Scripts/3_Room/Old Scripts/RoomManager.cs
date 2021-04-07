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
using HSVPicker;

public class RoomManager : MonoBehaviourPunCallbacks
{
    //For the singleton
    public static RoomManager instance;
    [Header("Photon View")]
    PhotonView myPhotonView;


    [HideInInspector]
    public GameObject PlayerRef;
    public GameObject referenceObject;

    [HideInInspector]
    public Vector3 RefPosition;

    [HideInInspector]
    public bool isPlaced = false;

    [HideInInspector]
    public List<PlayerHandler> OtherPlayerList;

    public GameObject screenOrigin;
    public GameObject spawnPrefab;
    public GameObject spawnedObject;

    private PlayerHandler playerHandler;

    [Header("Room Info")]
    public TMP_Text roomInfo;

    [Header("AR Player Start Transform")]
    public GameObject camera_AR;

    [Header("Screen Player Start Transform")]
    public GameObject playerTransformScreen;
    public GyroCamera playerGyro;

    [Header("Drawing")]
    public bool DrawMode = false;
    public bool PartyMode = false;
    public bool ChangeMode = false;
    [HideInInspector]
    public DrawActionManager drawActionManager;

    public ColorPicker colorPicker;
    public Slider weightSlider;

    [Header("Raising Hand")]
    public bool isHandRaised;

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
        isLocked = false;
        displayOtherAvatars = true;

        myPhotonView = GetComponent<PhotonView>();

        if (roomInfo != null)
        roomInfo.text = PhotonNetwork.CurrentRoom.Name.Split('_')[1] + '\n'; //Add Room name to Info Text


        if (DrawMode)
        {
            drawActionManager = GetComponent<DrawActionManager>();
        }
        OtherPlayerList = new List<PlayerHandler>();

        UpdatePlayerCount();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && PlayerRef==null)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {

        Transform cam;

        cam = camera_AR.transform;

        if(DrawMode)
        {
            if(PartyMode)
            {
                PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatarIKParty"), cam.position, cam.rotation);

            }
            else
            if (ChangeMode)
            {
                PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatarIKDrawChange"), cam.position, cam.rotation);
            }
            else
            {
                PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatarIKDraw"), cam.position, cam.rotation);

            }

        }
        else
        {
            PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatarIK"), cam.position, cam.rotation);

        }


        PlayerRef.transform.parent = cam;

        if (PhotonVoiceNetwork.Instance.ClientState == Photon.Realtime.ClientState.Joined)
        {
            Debug.Log("Joined Voice");
            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false;
        }


        foreach (PlayerHandler p in OtherPlayerList)
        {
            if(referenceObject)
            {
                //p.gameObject.transform.parent = referenceObject.transform;
            }
            p.MakeVisible();
            //p.ChangeAvatarDisplay(currentAvatar);
        }

        playerHandler = PlayerRef.GetComponent<PlayerHandler>();

        /*
        if (isDraw)
        {
            eraseButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().EraseDrawing);
            blackButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorBlack);
            whiteButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorWhite);
            redButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorRed);
            blueButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorBlue);

            eraseButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().EraseDrawing);
            blackButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorBlack);
            whiteButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorWhite);
            redButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorRed);
            blueButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorBlue);
        }
        */

        isPlaced = true;
    }

    public void SpawnPlayerScreen()
    {
        
        playerGyro.CalibrateFunction();

        StartCoroutine(WaitForSpawnUpdate());
       
    }

    public void SpawnPlayerObjectScreen()
    {
        Transform cam;

        cam = playerTransformScreen.transform;

        if (DrawMode)
        {
            if (PartyMode)
            {
                PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatarIKParty"), cam.position, cam.rotation);

            }
            else
            if (ChangeMode)
            {
                PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatarIKDrawChange"), cam.position, cam.rotation);
            }
            else
            {
                PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatarIKDraw"), cam.position, cam.rotation);

            }
        }
        else
        {
            PlayerRef = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatarIK"), cam.position, cam.rotation);
        }



        PlayerRef.transform.parent = playerTransformScreen.transform;

        if(PhotonVoiceNetwork.Instance)
        {
            if (PhotonVoiceNetwork.Instance.ClientState == Photon.Realtime.ClientState.Joined)
            {
                Debug.Log("Joined Voice");
                PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false;
            }
        }


        foreach (PlayerHandler p in OtherPlayerList)
        {
            //p.gameObject.transform.parent = referenceObject.transform;
            p.MakeVisible();
            //p.ChangeAvatarDisplay(currentAvatar);
        }

        playerHandler = PlayerRef.GetComponent<PlayerHandler>();

        /*
        if (isDraw)
        {
            eraseButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().EraseDrawing);
            blackButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorBlack);
            whiteButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorWhite);
            redButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorRed);
            blueButtonAR.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorBlue);

            eraseButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().EraseDrawing);
            blackButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorBlack);
            whiteButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorWhite);
            redButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorRed);
            blueButton.onClick.AddListener(PlayerRef.GetComponent<AvatarDrawHandler>().ChangeColorBlue);
        }
        */

        playerHandler.MakeVisible();

        isPlaced = true;
    }

    public void SpawnPlayerRefScreen()
    {

        spawnedObject = Instantiate(spawnPrefab, screenOrigin.transform.position, screenOrigin.transform.rotation);

        spawnedObject.transform.Rotate(Vector3.up, -45f * RoomManager.instance.OtherPlayerList.Count, Space.Self);

        referenceObject = spawnedObject;

    }

    IEnumerator WaitForSpawnUpdate()
    {

        //Pause for 0.5 seconds
        yield return new WaitForSeconds(0.1f);

        SpawnPlayerRefScreen();
        SpawnPlayerObjectScreen();

    }


    public void ToggleOtherAvatars()
    {
        if(displayOtherAvatars)
        {
            displayOtherAvatars = false;
            HideOtherAvatars();
        }
        else
        {
            displayOtherAvatars = true;
            ShowOtherAvatars();
        }
    }

    public void HideOtherAvatars()
    {
        foreach (PlayerHandler p in OtherPlayerList)
        {
            p.myAvatars.HideAvatar();
        }
    }

    public void ShowOtherAvatars()
    {
        foreach (PlayerHandler p in OtherPlayerList)
        {
            p.myAvatars.ShowAvatar();
        }
    }


    public void RaiseHand()
    {
        isHandRaised = true;
    }

    public void DropHand()
    {
        isHandRaised = false;
    }

    public void ObjectLock()
    {
        isLocked = true;
    }

    public void ObjectLockToggle()
    {
        isLocked = isLocked ? false : true;
    }

    public bool IsObjectLocked()
    {
        return isLocked;
    }

    public bool DisplayOtherAvatars()
    {
        return displayOtherAvatars;
    }



    private void UpdatePlayerCount()
    {
        if(roomInfo!=null)
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
