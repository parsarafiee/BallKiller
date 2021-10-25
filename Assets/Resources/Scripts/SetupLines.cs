using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupLines : MonoBehaviour
{
    public Transform whiteBall;
    public Transform targetBall;
    public float LineWidth;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, whiteBall.position); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, targetBall.position); //x,y and z position of the end point of the line
    }
}
