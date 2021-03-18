using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

public class BasicLobbyController : MonoBehaviourPunCallbacks
{

    //Room Text
    [SerializeField] private TMP_Text initialMessage;

    //Start button that is only available to the host
    [SerializeField] private GameObject startButton = null;

    private PhotonView myPhotonView;

    [SerializeField] private TMP_Text playerCount;
    [SerializeField] private TMP_Text playerNames;

    //Index of the AR Room Scene
    [SerializeField] private int mainSceneIndex;

    private void Start()
    {
        initialMessage.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name.Split('_')[1];

        

        //Enable start button if Host
        if(PlayerDataHolder.isHost)
        {
            SetStartButton(true);
        }
        else
        {
            SetStartButton(false);
        }
        
        //Make sure all players move to the next scene together !CHECK
        PhotonNetwork.AutomaticallySyncScene = true;

        UpdatePlayerCount();
        UpdatePlayerNames();

        myPhotonView = GetComponent<PhotonView>();

    }

    private void SetStartButton(bool isActive)
    {
        //Make Start Button Interactable
        if(isActive)
        {
            //startButton.GetComponent<Button>().interactable = true;
            startButton.GetComponent<Button>().enabled = true;
            startButton.GetComponentInChildren<TMP_Text>().text = "Start Room";
        
        }
        else
        {
            //startButton.GetComponent<Button>().interactable = false;
            startButton.GetComponent<Button>().enabled = false;
            startButton.GetComponentInChildren<TMP_Text>().text = "Waiting...";

        }
    }

    private void UpdatePlayerCount()
    {
        playerCount.text = "User Count: " + PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    private void UpdatePlayerNames()
    {
        playerNames.text = "";
        //Debug.Log("Players in the room right before text update: " + PhotonNetwork.PlayerList.Length.ToString());
        playerNames.text = "People in the room: \n";

        //Make sure that the 1st player in the list is assigned the host tag, also fixing punctuation
        bool isFirst = true;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerNames.text += (isFirst) ? player.NickName.ToString() : (", " + player.NickName.ToString());
            if (player.IsMasterClient)
            {
                playerNames.text += " (Host)";
            }

            isFirst = false;

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player Entered Room - " + newPlayer.NickName);
        base.OnPlayerEnteredRoom(newPlayer);
        //Pause before updating player count and names
        StartCoroutine(WaitForPlayerUpdate());
       
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Other Player Left Room - " + otherPlayer.NickName);
        base.OnPlayerLeftRoom(otherPlayer);
        //Pause before updating player count and names
        StartCoroutine(WaitForPlayerUpdate());
        
    }

    IEnumerator WaitForPlayerUpdate()
    {

        Debug.Log("Waiting for player to join room");

        //Pause for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        UpdatePlayerCount();
        UpdatePlayerNames();
    }

    public void StartMainScene()
    {
        //Only allow the Host to start the scene
        if (!PhotonNetwork.IsMasterClient)
            return;
        //Close the room so no other players can join
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(mainSceneIndex);
        
    }

}
