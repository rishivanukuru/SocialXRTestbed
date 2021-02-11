﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine.UI;


public class PhotonVoiceControls : MonoBehaviour
{

    public TMP_Text muteButtonText;
    public bool isSprite = false;
    private bool isMute;
    private Button myButton;

    void Start()
    {
        muteButtonText.text = "Speak";
        isMute = true;
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ToggleMute);

    }

    public void ToggleMute()
    {
        if(isMute)
        {
            VoiceSpeak();
            isMute = false;
            GetComponent<MuteSpriteController>().Speak();

        }
        else
        {
            VoiceMute();
            isMute = true;
            GetComponent<MuteSpriteController>().Mute();

        }
    }

    public void VoiceMute()
    {

        if (PhotonVoiceNetwork.Instance.ClientState == Photon.Realtime.ClientState.Joined)
        {
            muteButtonText.text = "Speak";

            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false;
        }

    }

    public void VoiceSpeak()
    {
        if (PhotonVoiceNetwork.Instance.ClientState == Photon.Realtime.ClientState.Joined)
        {
            muteButtonText.text = "Mute";

            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = true;
        }
    }
}
