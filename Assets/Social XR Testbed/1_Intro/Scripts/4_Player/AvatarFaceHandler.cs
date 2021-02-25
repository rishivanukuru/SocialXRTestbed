using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;
using TMPro;
using UnityEngine.UI;

public class AvatarFaceHandler : MonoBehaviourPun
{ 
    public Image faceImage;
    public Sprite blankSprite;

    public string[] peopleNames;
    public Sprite[] peopleImages;

    private PhotonView myPhotonView;


    // Start is called before the first frame update
    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();


        faceImage.sprite = blankSprite;
        faceImage.enabled = false;
      
        AssignAvatarFace();
        
    }


    void AssignAvatarFace()
    {
        for(int i = 0; i<peopleImages.Length; i++)
        {
            if(photonView.Owner.NickName.ToLower() == peopleNames[i].ToString().ToLower())
            {
                faceImage.sprite = peopleImages[i];
                return;
            }
        }

        faceImage.sprite = blankSprite;
    }

    public void DisplayAvatarFace(bool isDisplay)
    {
        if(isDisplay)
        {
            faceImage.enabled = true;
        }
        else
        {
            faceImage.enabled = false;
        }
    }

}
