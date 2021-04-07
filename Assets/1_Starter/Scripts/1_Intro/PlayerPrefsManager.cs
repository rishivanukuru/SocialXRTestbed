using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerPrefsManager : MonoBehaviour
{

    //Script to capture user input and pre-fill the input fields for next time
    //Drag in the respective input fields (TMP) that capture the following details:

    [Header("Room Info")]
    public TMP_InputField hostRoomName;     //Name of the room being created
    public TMP_InputField roomSize;         //Size of the Room (20 max on the free Photon Plan)

    [Header("Host Info")]
    public TMP_InputField hostName;         //Name of the Host User
    public TMP_InputField password;         //Password to only allow authorized users to open rooms (revisit before making the app more generally available)

    [Header("Guest")]
    public TMP_InputField guestName;        //Name of the User
    public TMP_InputField guestRoomName;    //Name of the room being joined


    void Start()
    {

        //Check for pre-filled information and populate

        if(PlayerPrefs.HasKey("hostRoomName"))
        {
            hostRoomName.text = PlayerPrefs.GetString("hostRoomName");
        }

        if (PlayerPrefs.HasKey("roomSize"))
        {
            roomSize.text = PlayerPrefs.GetString("roomSize");
        }

        if (PlayerPrefs.HasKey("hostName"))
        {
            hostName.text = PlayerPrefs.GetString("hostName");
        }

        if (PlayerPrefs.HasKey("password"))
        {
            password.text = PlayerPrefs.GetString("password");
        }

        if (PlayerPrefs.HasKey("guestName"))
        {
            guestName.text = PlayerPrefs.GetString("guestName");
        }

        if (PlayerPrefs.HasKey("guestRoomName"))
        {
            guestRoomName.text = PlayerPrefs.GetString("guestRoomName");
        }
     
    } 

    
    public void SetPlayerPrefsHost()        //Stores information for Host mode
    {
        PlayerPrefs.SetString("hostRoomName", hostRoomName.text);
        PlayerPrefs.SetString("roomSize", roomSize.text);
        PlayerPrefs.SetString("hostName", hostName.text);
        PlayerPrefs.SetString("password", password.text);
    }
    public void SetPlayerPrefsGuest()       //Stores information for Guest mode
    {
        PlayerPrefs.SetString("guestName", guestName.text);
        PlayerPrefs.SetString("guestRoomName", guestRoomName.text);
    }




}
