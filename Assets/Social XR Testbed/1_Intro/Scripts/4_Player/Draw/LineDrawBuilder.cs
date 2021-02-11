using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyCurvedLine;

public class LineDrawBuilder : MonoBehaviour
{
    public GameObject linePrefab;

    public GameObject currentLine;

    public GameObject curvedLinePoint;

    public LineRenderer lineRenderer;


    public bool singleObject = false;

    public List<Vector3> linePositions;

    float distBuffer = 0.04f;

    bool lineStart = false;

    public bool StartDraw = false;

    private List<GameObject> myLines;


    // Start is called before the first frame update
    void Start()
    {

        myLines = new List<GameObject>();
        ChangeColor(0);
    }

    // Update is called once per frame
    void Update()
    {

        if (StartDraw == true)
        {
        

            if (!lineStart)
            {
                CreateLine();
                lineStart = true;
            }
            else
            {


                Vector3 tempLinePos = transform.position;

                if (Vector3.Distance(tempLinePos, linePositions[linePositions.Count - 1]) > distBuffer)
                {
                    //UpdateLine(tempLinePos);
                    AddCurvedPoint(tempLinePos);
                }
            }

        }
        else
        {
   

            lineStart = false;
        }


        if (RoomManager.instance != null)
        {
            if (RoomManager.instance.referenceObject != null)
            {
                foreach (GameObject line in myLines)
                {
                    line.GetComponent<CurvedLineRenderer>().lineWidth = 0.01f * (RoomManager.instance.referenceObject.transform.localScale.magnitude / currentLine.GetComponent<CurvedLineRenderer>().lineScale);
                }
            }
        }

        
    }

    public void ChangeColor(int colorIndex)
    {
        linePrefab.GetComponent<LineRenderer>().material = linePrefab.GetComponent<DrawVariables>().materialsArray[colorIndex];
    }

    public void ChangeSize(float scale)
    {
        linePrefab.GetComponent<EasyCurvedLine.CurvedLineRenderer>().lineWidth = (linePrefab.GetComponent<DrawVariables>().minWidth)*(scale);
    }

    public void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

        myLines.Add(currentLine);

        if (RoomManager.instance!=null)
        {
            if(RoomManager.instance.referenceObject!=null)
            {
                currentLine.transform.parent = RoomManager.instance.referenceObject.transform;
                currentLine.GetComponent<CurvedLineRenderer>().lineScale = RoomManager.instance.referenceObject.transform.localScale.magnitude;
            }
        }

        lineRenderer = currentLine.GetComponent<LineRenderer>();
        linePositions.Clear();
        linePositions.Add(transform.position);
        linePositions.Add(transform.position);
        lineRenderer.SetPosition(0, linePositions[0]);
        lineRenderer.SetPosition(1, linePositions[1]);
    }

    public void UpdateLine(Vector3 newLinePosition)
    {
        linePositions.Add(newLinePosition);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newLinePosition);
    }

    public void AddCurvedPoint(Vector3 newPointPosition)
    {
        GameObject curvedPoint = Instantiate(curvedLinePoint, newPointPosition, Quaternion.identity);
        curvedPoint.transform.SetParent(currentLine.transform);
        //curvedPoint.transform.parent = currentLine.transform;
    }

    public void DestroyMyLines()
    {
        foreach(GameObject line in myLines)
        {
            myLines.Remove(line);
            GameObject.Destroy(line);
        }
    }

    public void StartDrawMode()
    {
        StartDraw = true;
    }

    public void StopDrawMode()
    {
        StartDraw = false;
    }

}
