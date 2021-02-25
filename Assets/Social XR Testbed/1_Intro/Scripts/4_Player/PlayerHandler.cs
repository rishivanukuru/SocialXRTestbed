using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;
using TMPro;

public class PlayerHandler : MonoBehaviourPun, IPunObservable
{

    private bool isParented;

    private float LerpMultiplier = 200.0f;
    private Vector3 NewPos;
    private Quaternion NewRot;
    private float NewScale;
    private Vector3 NewScaleVector;

    public GameObject avatarHolder;
    public GameObject handHolder;

    private PhotonView myPhotonView;
    
    [HideInInspector]
    public AvatarChangeHandler myAvatars;

    [HideInInspector]
    public AvatarDrawHandler myDraw;

    [HideInInspector]
    public AvatarFaceHandler myFaces;

    private void Awake()
    {
        myPhotonView = GetComponent<PhotonView>();
        myAvatars = GetComponent<AvatarChangeHandler>();
        myFaces = GetComponent<AvatarFaceHandler>();
        myDraw = GetComponent<AvatarDrawHandler>();
    }

    private void Start()
    {
        isParented = false;
        if (!photonView.IsMine)
        {

            if (RoomManager.instance.isPlaced)
            {
                //this.gameObject.transform.parent = RoomManager.instance.referenceObject.transform;
                isParented = true;
                MakeVisible();
            }

            RoomManager.instance.OtherPlayerList.Add(this);
        }


    }

    private void Update()
    {
        /*
        if(!photonView.IsMine)
        {
            if (!isParented)
            {
                if (RoomManager.instance.isPlaced)
                {
                    this.gameObject.transform.parent = RoomManager.instance.referenceObject.transform;
                    isParented = true;
                }
            }
        }
        */


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (RoomManager.instance.referenceObject != null)
            {
                stream.SendNext(RoomManager.instance.referenceObject.transform.InverseTransformPoint(transform.position));
                stream.SendNext(Quaternion.Inverse(RoomManager.instance.referenceObject.transform.rotation) * transform.rotation);
                //stream.SendNext(RoomManager.instance.referenceObject.transform.lossyScale.x);

            }

        }
        else
        {
            if(RoomManager.instance.referenceObject!=null)
            {
                NewPos = RoomManager.instance.referenceObject.transform.TransformPoint((Vector3)stream.ReceiveNext());
                NewRot = RoomManager.instance.referenceObject.transform.rotation * (Quaternion)stream.ReceiveNext();
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
        if (!avatarHolder.activeSelf && !photonView.IsMine)
        {
            myAvatars.playerAvatars[0].SetActive(true);
            avatarHolder.SetActive(true);
            
        }

        if (handHolder != null)
        {
            handHolder.SetActive(true);
        }
    }

    

}
