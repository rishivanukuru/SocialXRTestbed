using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerPrefsManager : MonoBehaviour
{

    [Header("Host")]
    public TMP_InputField hostRoomName;

    public TMP_InputField roomSize;

    public TMP_InputField hostName;

    public TMP_InputField password;

    [Header("Guest")]
    public TMP_InputField guestName;

    public TMP_InputField guestRoomName;


    void Start()
    {

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

    void Update()
    {

    }

    public void SetPlayerPrefsHost()
    {
        PlayerPrefs.SetString("hostRoomName", hostRoomName.text);
        PlayerPrefs.SetString("roomSize", roomSize.text);
        PlayerPrefs.SetString("hostName", hostName.text);
        PlayerPrefs.SetString("password", password.text);
    }
    public void SetPlayerPrefsGuest()
    {
        PlayerPrefs.SetString("guestName", guestName.text);
        PlayerPrefs.SetString("guestRoomName", guestRoomName.text);
    }




}
