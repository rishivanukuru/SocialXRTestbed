using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerColorHandler : MonoBehaviour
{
    //Convoluted script to assign a unique-ish color to each player based on their nickname

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
        if(!setColor) //Making sure this happens in update once the PhotonViews have been linked
        {
            setColor = true;
            string nickname = myPhotonView.Owner.NickName.ToLower();
            float myH = ((float)((int)(nickname[0]) - 96) / 26f)*0.8f; //Hue is linked to the first letter of the name
            float myS = 0.5f + ((float)(nickname.Length) / 20f) / 3f; //Saturation is linked to the length of the name
            float myV = 0.99f; //Brightness is fixed

            Color myRGB = Color.HSVToRGB(myH, myS, myV);
            myMaterial.color = myRGB;
            //Debug.Log(nickname + ": " + myH.ToString() + ", " + myS.ToString() + ", " + myV.ToString());
        }
    }

}
