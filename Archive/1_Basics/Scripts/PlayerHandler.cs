using System.Collections;
using UnityEngine;
using Photon.Pun;

using TMPro;

public class PlayerHandler : MonoBehaviourPun, IPunObservable
{

    [SerializeField]
    private Renderer BodyRenderer;
    private float LerpMultiplier = 200.0f;
    private Vector3 NewPos;
    private Quaternion NewRot;
    private GameObject body;




    public TMP_Text nameTag;

    private void Start()
    {

        body = transform.GetChild(0).gameObject;
        body.SetActive(false);
        if (!photonView.IsMine)
        {
            if (BasicARSessionManager.instance.isPlaced)
                MakeVisible();
            BasicARSessionManager.instance.OtherPlayerList.Add(this);
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(BasicARSessionManager.instance.referenceObject.transform.InverseTransformPoint(transform.position));
            stream.SendNext(Quaternion.Inverse(BasicARSessionManager.instance.referenceObject.transform.rotation) * transform.rotation);
        }
        else
        {
            if(BasicARSessionManager.instance.referenceObject!=null)
            {
                NewPos = BasicARSessionManager.instance.referenceObject.transform.TransformPoint((Vector3)stream.ReceiveNext());
                NewRot = BasicARSessionManager.instance.referenceObject.transform.rotation * (Quaternion)stream.ReceiveNext();
                transform.position = Vector3.Lerp(transform.position, NewPos, Time.deltaTime * LerpMultiplier);
                transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, Time.deltaTime * LerpMultiplier);
            }
            
        }
    }



    public void MakeVisible()
    {
        if (!photonView.IsMine && !body.activeSelf)
        {
            body.SetActive(true);
            nameTag.text = photonView.Owner.NickName;
        }
        if(photonView.IsMine)
        {
        }
    }



}
