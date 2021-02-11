using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine.UI;

public class AvatarCanvasDisplay : MonoBehaviour
{

    [Header("Photon Stuff")]
    public PhotonView playerPhotonView;
    public PhotonVoiceView playerVoiceView;

    [Header("Canvas Stuff")]
    public TMP_Text nameTag;
    public Image speakingIndicator;

    void Start()
    {
        nameTag.text = playerPhotonView.Owner.NickName;
        speakingIndicator.enabled = false;
    }

    void Update()
    {
        if(playerVoiceView.IsSpeaking)
        {
            speakingIndicator.enabled = true;
        }
        else
        {
            speakingIndicator.enabled = false;
        }
    }
}
