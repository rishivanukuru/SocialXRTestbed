using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    float m_Angle;
    public float speed = 10f;

    void Update()
    {
        m_Angle += Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(0, m_Angle,0);
    }
}
