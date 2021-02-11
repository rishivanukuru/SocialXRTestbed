using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using EasyCurvedLine;


public class AvatarDrawHandler : MonoBehaviourPun
{
    [Header("Raise Hands")]
    public GameObject drawHand;
    public GameObject raiseHand;
    public bool handRaised;

    public GameObject drawPoint;
    [Header("Marker")]
    public GameObject playerMarkerPrefab;
    private List<GameObject> playerMarkers;
    [Header("2D Line")]
    public GameObject LinePrefab;
    [Header("3D Line")]
    public GameObject Line3DPrefab;
    private LineRenderer CurrentLineRenderer;
    private List<LineRenderer> myLines;
    [Header("3D Materials")]
    public Material materialBlack;
    public Material materialWhite;
    public Material materialBlue;
    public Material materialRed;
    private GameObject DrawParent;
    private PlayerHandler PlayerHandlerRef;
    private Transform focusObjTransform;
    private Camera cam;

    private bool is3Ddrawing;

    Color myRGB;

    void Start()
    {
        myLines = new List<LineRenderer>();
        PlayerHandlerRef = GetComponent<PlayerHandler>();
        cam = Camera.main;
        is3Ddrawing = false;

        string nickname = gameObject.GetComponent<PhotonView>().Owner.NickName.ToLower();

        float myH = ((float)((int)(nickname[0]) - 96) / 26f) * 0.8f;

        float myS = 0.5f + ((float)(nickname.Length) / 20f) / 3f;

        float myV = 0.99f;


        myRGB = Color.HSVToRGB(myH, myS, myV);

        drawPoint.GetComponent<Renderer>().material = materialBlack;
        drawPoint.GetComponent<Renderer>().material.color = myRGB;
        handRaised = false;
    }

    private void Update()
    {
        //Return if not owned by player
        if (!photonView.IsMine) return;

        //Check for raised hand
        if (RoomManager.instance.isHandRaised && handRaised == false)
        {
            handRaised = true;
            photonView.RPC("RaiseHand", RpcTarget.AllBuffered, true);
            return;
        }
        else
        if (RoomManager.instance.isHandRaised == false && handRaised == true)
        {
            handRaised = false;
            photonView.RPC("RaiseHand", RpcTarget.AllBuffered, false);
            return;
        }

        //Return if there aren't any touches
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);
        
        if(RoomManager.instance.drawActionManager.actionState == 0) //Draw Mode
        {

            if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == true && is3Ddrawing == false)
            {
                is3Ddrawing = true;

                Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(drawPoint.transform.position);
                photonView.RPC("StartDrawing", RpcTarget.AllBuffered, point);

            }
            else
        if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == true && is3Ddrawing == true)
            {
                Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(drawPoint.transform.position);
                photonView.RPC("UpdateDrawing", RpcTarget.AllBuffered, point);
            }
            else
        if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == false && is3Ddrawing == true)
            {
                is3Ddrawing = false;
            }

            if (t.phase != TouchPhase.Began && t.phase != TouchPhase.Moved) return;

            if (!EventSystem.current.IsPointerOverGameObject(t.fingerId))
            {
                Ray r = cam.ScreenPointToRay(t.position);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(r, out hit) && hit.collider.gameObject.layer == 10)
                {
                    Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(hit.point);
                    if (t.phase == TouchPhase.Began)
                    {
                        photonView.RPC("StartDrawing", RpcTarget.AllBuffered, point);
                    }
                    else if (t.phase == TouchPhase.Moved)
                        photonView.RPC("UpdateDrawing", RpcTarget.AllBuffered, point);
                }
            }

        }
        else
        if (RoomManager.instance.drawActionManager.actionState == 1) //Marker Mode
        {
            if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == true && is3Ddrawing == false)
            {
                is3Ddrawing = true;

                Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(drawPoint.transform.position);
                photonView.RPC("PlaceMarker", RpcTarget.AllBuffered, point);

            }
            else
            if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == true && is3Ddrawing == true)
            {
                //Do Nothing
            }
            else
            if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == false && is3Ddrawing == true)
            {
                is3Ddrawing = false;
            }

            if (t.phase != TouchPhase.Began && t.phase != TouchPhase.Moved) return;

            if (!EventSystem.current.IsPointerOverGameObject(t.fingerId))
            {
                Ray r = cam.ScreenPointToRay(t.position);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(r, out hit) && hit.collider.gameObject.layer == 10)
                {
                    Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(hit.point);
                    if (t.phase == TouchPhase.Began)
                    {
                        photonView.RPC("PlaceMarker", RpcTarget.AllBuffered, point);
                    }
                    else if (t.phase == TouchPhase.Moved)
                        photonView.RPC("MoveMarker", RpcTarget.AllBuffered, point);
                }
            }
        }

        
    }

    [PunRPC]
    private void RaiseHand(bool raiseStatus)
    {

        if(raiseStatus)
        {
            raiseHand.SetActive(true);
            drawHand.SetActive(false);
        }
        else
        {
            raiseHand.SetActive(false);
            drawHand.SetActive(true);
        }
    }

    [PunRPC]
    private void StartDrawing(Vector3 pos)
    {
        if (DrawParent == null)
        {
            DrawParent = new GameObject(photonView.Owner.NickName);
            DrawParent.transform.position = RoomManager.instance.referenceObject.transform.position;
            DrawParent.transform.localPosition = Vector3.zero;
            DrawParent.transform.localRotation = Quaternion.identity;
            DrawParent.transform.parent = RoomManager.instance.referenceObject.transform;
        }

        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        CurrentLineRenderer = Instantiate(LinePrefab, worldPos, Quaternion.identity, DrawParent.transform).GetComponent<LineRenderer>();
        CurrentLineRenderer.material = drawPoint.GetComponent<Renderer>().material;
        myLines.Add(CurrentLineRenderer);
    }

    [PunRPC]
    private void UpdateDrawing(Vector3 pos)
    {
        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        if (CurrentLineRenderer.positionCount > 0)
            worldPos = Vector3.Lerp(CurrentLineRenderer.GetPosition(CurrentLineRenderer.positionCount - 1), worldPos, 0.7f);
        CurrentLineRenderer.positionCount++;
        CurrentLineRenderer.SetPosition(CurrentLineRenderer.positionCount - 1, worldPos);
    }


    [PunRPC]
    private void PlaceMarker(Vector3 pos)
    {
        if (DrawParent == null)
        {
            DrawParent = new GameObject(photonView.Owner.NickName);
            DrawParent.transform.position = RoomManager.instance.referenceObject.transform.position;
            DrawParent.transform.localPosition = Vector3.zero;
            DrawParent.transform.localRotation = Quaternion.identity;
            DrawParent.transform.parent = RoomManager.instance.referenceObject.transform;
        }

        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        CurrentLineRenderer = Instantiate(LinePrefab, worldPos, Quaternion.identity, DrawParent.transform).GetComponent<LineRenderer>();
        CurrentLineRenderer.material = drawPoint.GetComponent<Renderer>().material;
        myLines.Add(CurrentLineRenderer);
    }

    [PunRPC]
    private void MoveMarker(Vector3 pos)
    {
        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        if (CurrentLineRenderer.positionCount > 0)
            worldPos = Vector3.Lerp(CurrentLineRenderer.GetPosition(CurrentLineRenderer.positionCount - 1), worldPos, 0.7f);
        CurrentLineRenderer.positionCount++;
        CurrentLineRenderer.SetPosition(CurrentLineRenderer.positionCount - 1, worldPos);
    }

    [PunRPC]
    private void Start3DDrawing(Vector3 pos)
    {
        if (DrawParent == null)
        {
            DrawParent = new GameObject(photonView.Owner.NickName);
            DrawParent.transform.position = RoomManager.instance.referenceObject.transform.position;
            DrawParent.transform.localPosition = Vector3.zero;
            DrawParent.transform.localRotation = Quaternion.identity;
            DrawParent.transform.parent = RoomManager.instance.referenceObject.transform;
        }

        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        CurrentLineRenderer = Instantiate(Line3DPrefab, worldPos, Quaternion.identity, DrawParent.transform).GetComponent<LineRenderer>();
        CurrentLineRenderer.material = drawPoint.GetComponent<Renderer>().material;
        myLines.Add(CurrentLineRenderer);


    }

    [PunRPC]
    private void Update3DDrawing(Vector3 pos)
    {
        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        if (CurrentLineRenderer.positionCount > 0)
            worldPos = Vector3.Lerp(CurrentLineRenderer.GetPosition(CurrentLineRenderer.positionCount - 1), worldPos, 0.7f);
        CurrentLineRenderer.positionCount++;
        CurrentLineRenderer.SetPosition(CurrentLineRenderer.positionCount - 1, worldPos);
    }

    public void UndoLine()
    {
        photonView.RPC("UndoLineNetwork", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void UndoLineNetwork()
    {
        if(myLines.Count>0)
        {
            LineRenderer tempLine = myLines[myLines.Count - 1];
            myLines.RemoveAt(myLines.Count - 1);
            Destroy(tempLine);
        }
    }

    public void EraseDrawing()
    {
        photonView.RPC("EraseDrawingNetwork", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void EraseDrawingNetwork()
    {
        myLines.Clear();
        Destroy(DrawParent);
    }

    public void ChangeColorRed()
    {
        photonView.RPC("ChangeColorRedNetwork", RpcTarget.AllBuffered);
    }

    public void ChangeColorBlue()
    {
        photonView.RPC("ChangeColorBlueNetwork", RpcTarget.AllBuffered);
    }

    public void ChangeColorWhite()
    {
        photonView.RPC("ChangeColorWhiteNetwork", RpcTarget.AllBuffered);
    }

    public void ChangeColorBlack()
    {
        photonView.RPC("ChangeColorBlackNetwork", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ChangeColorRedNetwork()
    {
        //CurrentLineRenderer.GetComponent<LineRenderer>().material = materialRed;
        //Current3DLineRenderer.GetComponent<LineRenderer>().material = materialRed;
        drawPoint.GetComponent<Renderer>().material = materialRed;
    }

    [PunRPC]
    public void ChangeColorBlueNetwork()
    {
        //CurrentLineRenderer.GetComponent<LineRenderer>().material = materialBlue;
        //Current3DLineRenderer.GetComponent<LineRenderer>().material = materialBlue;
        drawPoint.GetComponent<Renderer>().material = materialBlue;
    }

    [PunRPC]
    public void ChangeColorWhiteNetwork()
    {
        //CurrentLineRenderer.GetComponent<LineRenderer>().material = materialWhite;
        //Current3DLineRenderer.GetComponent<LineRenderer>().material = materialWhite;
        drawPoint.GetComponent<Renderer>().material = materialWhite;
    }

    [PunRPC]
    public void ChangeColorBlackNetwork()
    {

        //CurrentLineRenderer.GetComponent<LineRenderer>().material = materialBlack;
        //Current3DLineRenderer.GetComponent<LineRenderer>().material = materialBlack;
        drawPoint.GetComponent<Renderer>().material = materialBlack;
    }


}
