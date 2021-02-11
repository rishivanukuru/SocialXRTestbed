using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerColorHandler : MonoBehaviour
{
    public PhotonView myPhotonView;

    private Material myMaterial;

    bool setColor;

    void Start()
    {


        myMaterial = GetComponent<Renderer>().material;

        setColor = false;

       

    }

    private void Update()
    {
        if(!setColor)
        {
            setColor = true;

            string nickname = myPhotonView.Owner.NickName.ToLower();

            float myH = ((float)((int)(nickname[0]) - 96) / 26f)*0.8f;

            float myS = 0.5f + ((float)(nickname.Length) / 20f) / 3f;

            float myV = 0.99f;


            Color myRGB = Color.HSVToRGB(myH, myS, myV);

            

            myMaterial.color = myRGB;

            Debug.Log(nickname + ": " + myH.ToString() + ", " + myS.ToString() + ", " + myV.ToString());

        }
    }

}
