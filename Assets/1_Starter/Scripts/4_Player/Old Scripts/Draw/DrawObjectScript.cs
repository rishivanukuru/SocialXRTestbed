using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawObjectScript : MonoBehaviour
{
    public Material colorWhite;
    public Material colorRed;
    public Material colorBlack;
    public Material colorBlue;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().material = colorBlack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
