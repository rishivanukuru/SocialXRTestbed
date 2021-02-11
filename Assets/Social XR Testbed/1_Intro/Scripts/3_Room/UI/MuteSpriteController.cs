﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteSpriteController : MonoBehaviour
{


    public Sprite speakSprite;
    public Sprite muteSprite;
    private Image mySprite;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<Image>();
        mySprite.sprite = muteSprite;
    }

    public void Mute()
    {
        mySprite.sprite = muteSprite;
    }

    public void Speak()
    {
        mySprite.sprite = speakSprite;
    }


}
