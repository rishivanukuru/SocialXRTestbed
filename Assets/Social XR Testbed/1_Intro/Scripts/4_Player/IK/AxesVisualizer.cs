using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class AxesVisualizer : MonoBehaviour
{
    public bool Forward, Up, Right;
    private void Update()
    {
        if (Forward) Debug.DrawRay(transform.position, transform.forward, Color.blue);
        if (Up) Debug.DrawRay(transform.position, transform.up, Color.green);
        if (Right) Debug.DrawRay(transform.position, transform.right, Color.red);

    }
}
