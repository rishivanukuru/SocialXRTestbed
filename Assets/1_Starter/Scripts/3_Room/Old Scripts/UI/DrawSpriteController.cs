using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawSpriteController : MonoBehaviour
{

    Image myButtonImage;
    public Sprite drawSprite;
    public Sprite markerSprite;

    // Start is called before the first frame update
    void Start()
    {
        myButtonImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDrawSprite()
    {
        myButtonImage.sprite = drawSprite;
    }

    public void SetMarkerSprite()
    {
        myButtonImage.sprite = markerSprite;
    }
}
