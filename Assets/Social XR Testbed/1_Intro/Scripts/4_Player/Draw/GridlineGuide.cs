using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridlineGuide : MonoBehaviour
{
    [Header("Gridline Variables")]
    [SerializeField]
    float lineWidth = 0.01f;
    [SerializeField]
    float lineSpacing = 0.05f;
    [SerializeField]
    int lineCount = 2;
    int linesPerDim;

    public Color lineColor;
    public Material lineMaterial;

    GameObject[,] lineRenderers;

    void Start()
    {
        linesPerDim = lineCount * 2 + 1;
        lineRenderers = new GameObject[3,linesPerDim];

        for(int i = 0; i<3;i++)
        {
            for(int j = 0; j < linesPerDim; j++)
            {
                lineRenderers[i, j] = new GameObject();
                lineRenderers[i, j].name = "gridline_" + i.ToString() + "_" + j.ToString();
                lineRenderers[i, j].AddComponent<LineRenderer>();
                lineRenderers[i, j].GetComponent<LineRenderer>().startWidth = lineWidth;
                lineRenderers[i, j].GetComponent<LineRenderer>().endWidth = lineWidth;
                lineRenderers[i, j].GetComponent<LineRenderer>().startColor = lineColor;
                lineRenderers[i, j].GetComponent<LineRenderer>().material = lineMaterial;

                Vector3 startPos = new Vector3(0,0,0);
                Vector3 endPos = new Vector3(0,0,0);

                lineRenderers[0, j].GetComponent<LineRenderer>().positionCount = 2;
                lineRenderers[0, j].GetComponent<LineRenderer>().SetPosition(0, startPos);
                lineRenderers[0, j].GetComponent<LineRenderer>().SetPosition(1, endPos);
                //lineRenderers[i, j].SetPosition(0,this.transform.position);
            }
        }
    }

    void Update()
    {
        RenderGridlineGuide();
    }


    void RenderGridlineGuide()
    {
        float currentX = this.gameObject.transform.position.x;
        float currentY = this.gameObject.transform.position.y;
        float currentZ = this.gameObject.transform.position.z;

        float closestX = ((int)(currentX * 100f)) + ((((int)(currentX * 100f)) % lineSpacing * 100) > lineSpacing * 50 ? -1 * ((int)(currentX * 100f)) % lineSpacing * 100 : 1*(lineSpacing * 100 - ((int)(currentX * 100f)) % lineSpacing * 100));
        float closestY = ((int)(currentY * 100f)) + ((((int)(currentY * 100f)) % lineSpacing * 100) > lineSpacing * 50 ? -1 * ((int)(currentY * 100f)) % lineSpacing * 100 : 1*(lineSpacing * 100 - ((int)(currentY * 100f)) % lineSpacing * 100));
        float closestZ = ((int)(currentZ * 100f)) + ((((int)(currentZ * 100f)) % lineSpacing * 100) > lineSpacing * 50 ? -1 * ((int)(currentZ * 100f)) % lineSpacing * 100 : 1*(lineSpacing * 100 - ((int)(currentZ * 100f)) % lineSpacing * 100));

        for (int j = 0; j < linesPerDim; j++)
        {
            Vector3 startPosX = new Vector3(((closestX) / 100 - lineCount * lineSpacing), closestY / 100, (((closestZ) / 100) + (j - lineCount) * lineSpacing));
            Vector3 endPosX = new Vector3((closestX) / 100 + lineCount * lineSpacing, closestY / 100, (closestZ) / 100 + (j - lineCount) * lineSpacing);

            lineRenderers[0, j].GetComponent<LineRenderer>().positionCount = 2;
            lineRenderers[0, j].GetComponent<LineRenderer>().SetPosition(0, startPosX);
            lineRenderers[0, j].GetComponent<LineRenderer>().SetPosition(1, endPosX);

            Vector3 startPosZ = new Vector3(((closestX) / 100 + (j - lineCount) * lineSpacing), closestY / 100, (((closestZ) / 100) - (lineCount) * lineSpacing));
            Vector3 endPosZ = new Vector3((closestX) / 100 + (j - lineCount) * lineSpacing, closestY / 100, (closestZ) / 100 + (lineCount) * lineSpacing);

            lineRenderers[1, j].GetComponent<LineRenderer>().positionCount = 2;
            lineRenderers[1, j].GetComponent<LineRenderer>().SetPosition(0, startPosZ);
            lineRenderers[1, j].GetComponent<LineRenderer>().SetPosition(1, endPosZ);

            Vector3 startPosY = new Vector3(((closestX) / 100 + (j - lineCount) * lineSpacing), (closestY) / 100 - lineCount * lineSpacing, closestZ / 100);
            Vector3 endPosY = new Vector3((closestX) / 100 + (j - lineCount) * lineSpacing, (closestY) / 100 + lineCount * lineSpacing, (closestZ) / 100);

            lineRenderers[2, j].GetComponent<LineRenderer>().positionCount = 2;
            lineRenderers[2, j].GetComponent<LineRenderer>().SetPosition(0, startPosY);
            lineRenderers[2, j].GetComponent<LineRenderer>().SetPosition(1, endPosY);
        }


    }
}
