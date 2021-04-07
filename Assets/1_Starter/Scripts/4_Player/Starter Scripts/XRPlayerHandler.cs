using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;
using TMPro;

public class XRPlayerHandler : MonoBehaviourPun, IPunObservable
{
    //Core networked player script 
    //Controls whether a player is visible or not
    //Networks the transform (position & rotation) relative to room reference object

    [Header("Body Holder")]
    public GameObject bodyHolder; //Everything about the Avatar except for the photon view and transform

    //Photon Transform View Variables
    private float LerpMultiplier = 200.0f;
    private Vector3 NewPos;
    private Quaternion NewRot;
    private float NewScale;
    private Vector3 NewScaleVector;

    //Current Photon View   
    private PhotonView myPhotonView;

    private void Awake()
    {
        myPhotonView = GetComponent<PhotonView>();
        bodyHolder.SetActive(false);
    }

    private void Start()
    {
        bodyHolder.SetActive(false);

        if (!photonView.IsMine)
        {
            XRRoomManager.instance.OtherPlayerList.Add(this);

            if (XRRoomManager.instance.isPlaced)
            {
                MakeVisible();
            }
        }
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //Networked position and rotation
    {
        if (stream.IsWriting)
        {
            if (XRRoomManager.instance.referenceObject != null)
            {
                stream.SendNext(XRRoomManager.instance.referenceObject.transform.InverseTransformPoint(transform.position));
                stream.SendNext(Quaternion.Inverse(XRRoomManager.instance.referenceObject.transform.rotation) * transform.rotation);
                //stream.SendNext(RoomManager.instance.referenceObject.transform.lossyScale.x);
            }

        }
        else
        {
            if (XRRoomManager.instance.referenceObject != null)
            {
                NewPos = XRRoomManager.instance.referenceObject.transform.TransformPoint((Vector3)stream.ReceiveNext());
                NewRot = XRRoomManager.instance.referenceObject.transform.rotation * (Quaternion)stream.ReceiveNext();
                //NewScale = (float)stream.ReceiveNext();
                //NewScale = RoomManager.instance.referenceObject.transform.lossyScale.x / NewScale;
                //NewPos = NewPos * (NewScale);
                //NewScaleVector = Vector3.one * NewScale;
                transform.position = Vector3.Lerp(transform.position, NewPos, Time.deltaTime * LerpMultiplier);
                transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, Time.deltaTime * LerpMultiplier);
                //transform.localScale = Vector3.Lerp(transform.localScale, NewScaleVector, Time.deltaTime * LerpMultiplier);
            }

        }
    }



    public void MakeVisible()
    {
        if (!bodyHolder.activeSelf)
        {
            bodyHolder.SetActive(true);
        }
    }



}
