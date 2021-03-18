using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using EasyCurvedLine;
using HSVPicker;
using SplineMesh;


public class AvatarDrawHandler : MonoBehaviourPun
{
    [Header("Raise Hands")]
    public GameObject drawHand;
    public GameObject raiseHand;
    public bool handRaised;

    public GameObject drawPoint;
    [Header("Marker")]
    public GameObject playerMarkerPrefab;
    private GameObject currentMarker;
    private List<GameObject> playerMarkers;
    [Header("2D Line")]
    public GameObject LinePrefab;
    [Header("3D Line")]
    public GameObject Line3DPrefab;
    private LineRenderer CurrentLineRenderer;
    private float strokeWeight;
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

    private bool isPlaced;

    Color myRGB;

    void Start()
    {
        isPlaced = false;
        playerMarkers = new List<GameObject>();
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

        strokeWeight = 0.01f;
        drawPoint.transform.localScale = Vector3.one*strokeWeight;

        handRaised = false;



    }

    private void Update()
    {
        //Return if not owned by player
        if (!photonView.IsMine) return;

        if(!isPlaced)
        {
            if(RoomManager.instance.isPlaced)
            {
                isPlaced = true;
                RoomManager.instance.colorPicker.CurrentColor = myRGB;

                RoomManager.instance.weightSlider.onValueChanged.AddListener(delegate { ChangeWeight(); });

                RoomManager.instance.colorPicker.onValueChanged.AddListener(color =>
                {
                    drawPoint.GetComponent<Renderer>().material.color = color;
                    ChangeColor(color);
                }
                );               

            }
        }

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

        //If on Phone
        if(!((Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)))
        {
            //Return if there aren't any touches
            if (Input.touchCount == 0) return;

            Touch t = Input.GetTouch(0);

            if (RoomManager.instance.drawActionManager.actionState == 0) //Draw Mode
            {

                if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == true && is3Ddrawing == false)
                {
                    is3Ddrawing = true;

                    Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(drawPoint.transform.position);
                    //Vector3 point = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform.InverseTransformPoint(drawPoint.transform.position);
                    photonView.RPC("StartDrawing", RpcTarget.AllBuffered, point);

                }
                else
                if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == true && is3Ddrawing == true)
                {
                    Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(drawPoint.transform.position);
                    //Vector3 point = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform.InverseTransformPoint(drawPoint.transform.position);
                    photonView.RPC("UpdateDrawing", RpcTarget.AllBuffered, point);
                }
                else
                if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == false && is3Ddrawing == true)
                {
                    photonView.RPC("BakeDrawing", RpcTarget.AllBuffered);
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
                        //Vector3 point = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform.InverseTransformPoint(hit.point);
                        if (t.phase == TouchPhase.Began)
                        {
                            photonView.RPC("StartDrawing", RpcTarget.AllBuffered, point);
                        }
                        else if (t.phase == TouchPhase.Moved)
                            photonView.RPC("UpdateDrawing", RpcTarget.AllBuffered, point);
                        else if (t.phase == TouchPhase.Ended)
                            photonView.RPC("BakeDrawing", RpcTarget.AllBuffered);

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
        else //If PC
        if(((Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)))
        {

            if (RoomManager.instance.drawActionManager.actionState == 0) //Draw Mode
            {

                if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == true && is3Ddrawing == false)
                {
                    is3Ddrawing = true;

                    Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(drawPoint.transform.position);
                    //Vector3 point = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform.InverseTransformPoint(drawPoint.transform.position);
                    photonView.RPC("StartDrawing", RpcTarget.AllBuffered, point);

                }
                else
                if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == true && is3Ddrawing == true)
                {
                    Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(drawPoint.transform.position);
                    //Vector3 point = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform.InverseTransformPoint(drawPoint.transform.position);
                    photonView.RPC("UpdateDrawing", RpcTarget.AllBuffered, point);
                }
                else
                if (RoomManager.instance.drawActionManager.isCurrentlyDrawing == false && is3Ddrawing == true)
                {
                    photonView.RPC("BakeDrawing", RpcTarget.AllBuffered);
                    is3Ddrawing = false;
                }

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;

                    if(objectHit.gameObject.layer == 10)
                    {
                        Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(hit.point);
                        //Vector3 point = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform.InverseTransformPoint(hit.point);
                        if (Input.GetMouseButtonDown(0))
                        {
                            photonView.RPC("BakeDrawing", RpcTarget.AllBuffered);
                            photonView.RPC("StartDrawing", RpcTarget.AllBuffered, point);
                        }
                        else if (Input.GetMouseButton(0))
                            photonView.RPC("UpdateDrawing", RpcTarget.AllBuffered, point);
                        //else if (Input.GetMouseButtonUp(0))
                    }

                    // Do something with the object that was hit by the raycast.
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
                

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;

                    if (objectHit.gameObject.layer == 10)
                    {
                        Vector3 point = RoomManager.instance.referenceObject.transform.InverseTransformPoint(hit.point);
                        if (Input.GetMouseButtonDown(0))
                        {
                            photonView.RPC("PlaceMarker", RpcTarget.AllBuffered, point);
                        }
                        else if (Input.GetMouseButton(0))
                            photonView.RPC("MoveMarker", RpcTarget.AllBuffered, point);
                    }

                    // Do something with the object that was hit by the raycast.
                }
            }
        }

        

        
    }

    public static void BakeLine(LineRenderer line)
    {
        var lineRenderer = line;
        var meshFilter = line.gameObject.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;

        var meshRenderer = line.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = line.material;

        GameObject.Destroy(lineRenderer);
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
        /*
        if (DrawParent == null)
        {
            DrawParent = new GameObject(photonView.Owner.NickName);
            DrawParent.transform.parent = null;
            DrawParent.transform.position = Vector3.zero;
            DrawParent.transform.localPosition = Vector3.zero;
            DrawParent.transform.localRotation = Quaternion.identity;
            DrawParent.transform.localScale = Vector3.one;

        }
        */

        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        //Vector3 worldPos = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform.TransformPoint(pos);
        CurrentLineRenderer = Instantiate(LinePrefab, worldPos, Quaternion.identity).GetComponent<LineRenderer>();
        CurrentLineRenderer.gameObject.transform.parent = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform;
        //CurrentLineRenderer.useWorldSpace = true;
        Color currentColor = drawPoint.GetComponent<Renderer>().material.color;
        float currentWidth = drawPoint.transform.localScale.x;
        //CurrentLineRenderer.material = drawPoint.GetComponent<Renderer>().material;
        CurrentLineRenderer.material.color = currentColor;
        CurrentLineRenderer.startWidth = currentWidth;
        myLines.Add(CurrentLineRenderer);
    }

    [PunRPC]
    private void UpdateDrawing(Vector3 pos)
    {
        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        Vector3 localPos = CurrentLineRenderer.gameObject.transform.InverseTransformPoint(worldPos);
        //Vector3 worldPos = RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform.TransformPoint(pos);
        if (CurrentLineRenderer.positionCount > 0)
        {
            
            localPos = Vector3.Lerp(CurrentLineRenderer.GetPosition(CurrentLineRenderer.positionCount - 1), localPos, 0.7f);
            
        }
        CurrentLineRenderer.positionCount++;
        CurrentLineRenderer.SetPosition(CurrentLineRenderer.positionCount - 1, localPos);
    }

    [PunRPC]
    private void BakeDrawing()
    {
        //BakeLine(CurrentLineRenderer);
        //CurrentLineRenderer.useWorldSpace = false;
        CurrentLineRenderer = null;
    }

    [PunRPC]
    private void PlaceMarker(Vector3 pos)
    {
        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        currentMarker = Instantiate(playerMarkerPrefab, worldPos, Quaternion.identity, RoomManager.instance.referenceObject.GetComponent<TableObjectManager>().currentObject.transform);
        Color currentColor = drawPoint.GetComponent<Renderer>().material.color;
        float currentWidth = drawPoint.transform.localScale.x;

        //CurrentLineRenderer.material = drawPoint.GetComponent<Renderer>().material;
        currentMarker.GetComponent<Renderer>().material.color = currentColor;
        currentMarker.transform.localScale = Vector3.one * currentWidth;
        playerMarkers.Add(currentMarker);
    }

    [PunRPC]
    private void MoveMarker(Vector3 pos)
    {
        Vector3 worldPos = RoomManager.instance.referenceObject.transform.TransformPoint(pos);
        currentMarker.transform.position = worldPos;
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
        Color currentColor = drawPoint.GetComponent<Renderer>().material.color;
        //CurrentLineRenderer.material = drawPoint.GetComponent<Renderer>().material;
        CurrentLineRenderer.material.color = currentColor;
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

        if (RoomManager.instance.drawActionManager.actionState == 0) //Draw Mode
        {
            photonView.RPC("UndoLineNetwork", RpcTarget.AllBuffered);

        }
        else
       if (RoomManager.instance.drawActionManager.actionState == 1) //Marker Mode
        {
            photonView.RPC("UndoMarkerNetwork", RpcTarget.AllBuffered);

        }

    }

    [PunRPC]
    public void UndoLineNetwork()
    {

        if (myLines.Count > 0)
        {
            LineRenderer tempLine = myLines[myLines.Count - 1];
            myLines.RemoveAt(myLines.Count - 1);
            Destroy(tempLine);
        }

        CurrentLineRenderer = null;
        
    }

    [PunRPC]
    public void UndoMarkerNetwork()
    {


            if (playerMarkers.Count > 0)
            {
                GameObject tempMarker = playerMarkers[playerMarkers.Count - 1];
                playerMarkers.RemoveAt(playerMarkers.Count - 1);
                Destroy(tempMarker);
            }

        currentMarker = null;

    }

    public void EraseDrawing()
    {

        photonView.RPC("EraseDrawingNetwork", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void EraseDrawingNetwork()
    {


        if (myLines.Count > 0)
        {
            for(int i = myLines.Count-1; i>=0; i--)
            {
                LineRenderer temp = myLines[i];
                myLines.Remove(temp);
                Destroy(temp);
            }
        }

        myLines.Clear();

        CurrentLineRenderer = null;


        if (playerMarkers.Count > 0)
        {

            for (int i = playerMarkers.Count - 1; i >= 0; i--)
            {
                GameObject temp = playerMarkers[i];
                playerMarkers.Remove(temp);
                Destroy(temp);
            }

        }

        playerMarkers.Clear();

        currentMarker = null;

    }

    public void ChangeWeight()
    {
        float weight = RoomManager.instance.weightSlider.value;
        photonView.RPC("ChangeWeightNetwork", RpcTarget.AllBuffered, weight);
    }

    [PunRPC]
    public void ChangeWeightNetwork(float weight)
    {
        
       drawPoint.transform.localScale = Vector3.one * weight;
 
    }

    public void ChangeColor(Color color)
    {
        photonView.RPC("ChangeColorNetwork", RpcTarget.AllBuffered, color.r,color.g,color.b,color.a);
    }

    [PunRPC]
    public void ChangeColorNetwork(float r, float g, float b, float a)
    {
        drawPoint.GetComponent<Renderer>().material.color = new Color(r,g,b,a);
    }

    

}
