using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    GameObject circle;
    public LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        circle = Resources.Load<GameObject>("Prefabs/bomb");


    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D rayCastHit = Physics2D.Raycast(this.transform.position, transform.right);
        Vector3 new1 = new Vector3(rayCastHit.point.x, rayCastHit.point.y, 0);
        line.positionCount = 3;
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, rayCastHit.point);
        line.SetPosition(2, rayCastHit.point);

    }
}
