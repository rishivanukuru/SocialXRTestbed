using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawActionManager : MonoBehaviour
{

    public int actionState;
    public bool isCurrentlyDrawing;
    [Header("UI")]
    public Button drawButton;
    public Button markerButton;
    public Button main3DActionButton;
    public Button undoButton;
    public Button eraseButton;
    


    private bool setUIBindings;

    void Start()
    {
        setUIBindings = false;
        //SetDrawMode();
        actionState = 0;
        drawButton.gameObject.GetComponent<Image>().color = Color.green;
        markerButton.gameObject.GetComponent<Image>().color = Color.gray;

    }

    void Update()
    {

        if(RoomManager.instance.isPlaced)
        {
            if (!setUIBindings)
            {
                setUIBindings = true;
                eraseButton.onClick.AddListener(RoomManager.instance.PlayerRef.GetComponent<AvatarDrawHandler>().EraseDrawing);
                undoButton.onClick.AddListener(RoomManager.instance.PlayerRef.GetComponent<AvatarDrawHandler>().UndoLine);
                drawButton.onClick.AddListener(SetDrawMode);
                markerButton.onClick.AddListener(SetMarkerMode);
            }
        }

    }


    public void StartDrawing()
    {
        isCurrentlyDrawing = true;
    }

    public void StopDrawing()
    {
        isCurrentlyDrawing = false;
    }

    public void SetDrawMode()
    {
        actionState = 0;
        drawButton.gameObject.GetComponent<Image>().color = Color.green;
        markerButton.gameObject.GetComponent<Image>().color = Color.gray;
        main3DActionButton.GetComponent<DrawSpriteController>().SetDrawSprite();
    }

    public void SetMarkerMode()
    {
        actionState = 1;
        drawButton.gameObject.GetComponent<Image>().color = Color.gray;
        markerButton.gameObject.GetComponent<Image>().color = Color.green;
        main3DActionButton.GetComponent<DrawSpriteController>().SetMarkerSprite();
    }


}
