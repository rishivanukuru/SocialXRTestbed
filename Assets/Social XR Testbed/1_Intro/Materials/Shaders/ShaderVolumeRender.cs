using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShaderVolumeRender : MonoBehaviour
{

    void Update()
    {
        Shader.SetGlobalMatrix("_WorldToBox", transform.worldToLocalMatrix);
    }
}
