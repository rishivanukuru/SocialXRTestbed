using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AvatarSpriteHandler : MonoBehaviour
{
    Image myImage;
    bool avatarDisplay = true;
    Button myButton;

    void Start()
    {
        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ToggleDisplay);
        avatarDisplay = true;
        myImage.color = avatarDisplay ? Color.white : Color.gray;
    }

    void Update()
    {

    }

    public void ToggleDisplay()
    {
        if (RoomManager.instance != null)
        {
            RoomManager.instance.ToggleOtherAvatars();
            avatarDisplay = RoomManager.instance.DisplayOtherAvatars();
            myImage.color = avatarDisplay ? Color.white : Color.gray;
        }

    }
}
