
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cue : MonoBehaviour
{
    [Header("Cue Info")]

    public GameObject whiteBall;
    public Transform headOfCue;
    public LineRenderer line;

    public int NumberOfPoints;
    public Gradient colorToChange;
    public Gradient white;
    public Gradient Green;

    [Header("Ball Info")]
    [Space(20)]

    public float ballRadiusRb;
    public float speed;



    List<Vector2> directions = new List<Vector2>();

    float correntDirection;
    bool playerHelper = true;
    bool hitTheBall = false;
    int counteToHitWhiteBallToWalls = 0;
    Rigidbody2D ballRb;

    Vector3 start_dir;

    void Start()
    {

        ballRb = whiteBall.GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        Debug.Log(directions.Count);

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(directions.Count);

        ControllerPlayer();
        if (playerHelper)
        {
            MakeAllReflections(NumberOfPoints, headOfCue.transform, this.transform.TransformDirection(Vector2.right));
        }
        if (hitTheBall)
        {

            HitTheBallAction( start_dir, speed);
            hitTheBall = false;
        }
    }
    void ControllerPlayer()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            hitTheBall = true;
            playerHelper = false;

        }


    }
    void HitTheBallAction(Vector3 dirs, float ballSpeed)
    {

        hitTheBall = true;
        //Vector2 ballDir = dirs[counteToHitWhiteBallToWalls];
        //Collider2D[] hitObject = Physics2D.OverlapCircleAll(ballToHit.transform.position, 2);

        //    Debug.Log(hitObject.Length);

        //if (hitObject.Length >1 )
        //{
        //    counteToHitWhiteBallToWalls++;
        //}

        //if (hitObject.Length==2)
        //{
        //    hitTheBall = false;
        //    return;

        //}

        ballRb.AddForce(dirs * ballSpeed * Time.deltaTime,ForceMode2D.Impulse);





    }

    void MakeAllReflections(int positionSizeLines, Transform startPoint, Vector3 startDirection)
    {
        directions.Clear();
        line.positionCount = positionSizeLines + 1;
        line.SetPosition(0, startPoint.position);
        Transform Point = startPoint;
        Vector3 dir = startDirection;
        start_dir = startDirection;
        directions.Add(dir);
        RaycastHit2D rayCastHit = Physics2D.Raycast(Point.position, dir, 99999, LayerMask.GetMask("Box", "otherball"));
        line.SetPosition(1, rayCastHit.point);
        if (rayCastHit.collider.name == "Object")
        {
            line.positionCount = 2;

            line.colorGradient = colorToChange;
            return;

        }
        for (int i = 1; i < positionSizeLines; i++)
        {
            line.colorGradient = white;
            Vector2 newDir = WallRefletion(dir, rayCastHit);
            rayCastHit = Physics2D.Raycast(rayCastHit.point, newDir, 99999, LayerMask.GetMask("UI", "otherball"));
            dir = newDir;
            directions.Add(dir);
            line.SetPosition(i + 1, rayCastHit.point);

            if (i == positionSizeLines - 1 && rayCastHit.collider.name == "Object")
            {
                Debug.Log("hello baby");
                line.colorGradient = Green;

                return;
            }
            if (rayCastHit.collider.name == "Object")
            {
                line.positionCount = i + 2;

                line.colorGradient = colorToChange;
                return;
            }

        }
    }
    Vector2 WallRefletion(Vector2 direction, RaycastHit2D rayCastHit)
    {

        Vector2 reflectionAngle = Vector2.zero;
        ////topWall Slop -
        if (direction.x < 0 && direction.y > 0 && rayCastHit.collider.name =="top1")
        {
            float find_angle = 180 + (90 - Vector3.Angle(-direction, rayCastHit.normal));
            reflectionAngle = new Vector2(Mathf.Cos(find_angle * Mathf.Deg2Rad), Mathf.Sin(find_angle * Mathf.Deg2Rad));
            // Debug.Log(reflectionAngle);
            return reflectionAngle;
        }
        //topWall Slop +
        if (direction.x > 0 && direction.y > 0 && rayCastHit.collider.name == "top1")
        {
            float find_angle = -(90 - Vector3.Angle(-direction, rayCastHit.normal));
            reflectionAngle = new Vector2(Mathf.Cos(find_angle * Mathf.Deg2Rad), Mathf.Sin(find_angle * Mathf.Deg2Rad));
            // Debug.Log(reflectionAngle);
            return reflectionAngle;
        }
        //rightWall slope +
        if (direction.x > 0 && direction.y > 0 && rayCastHit.collider.name == "right1")
        {
            float find_angle = 90 + (90 - Vector3.Angle(-direction, rayCastHit.normal));
            reflectionAngle = new Vector2(Mathf.Cos(find_angle * Mathf.Deg2Rad), Mathf.Sin(find_angle * Mathf.Deg2Rad));
            // Debug.Log(reflectionAngle);
            return reflectionAngle;
        }

        //rightWall slope -
        if (direction.x >= 0 && direction.y < 0 && rayCastHit.collider.name == "right1")
        {
            float find_angle = -90 - (90 - Vector3.Angle(-direction, rayCastHit.normal));
            reflectionAngle = new Vector2(Mathf.Cos(find_angle * Mathf.Deg2Rad), Mathf.Sin(find_angle * Mathf.Deg2Rad));
            // Debug.Log(reflectionAngle);
            return reflectionAngle;
        }


        // botWall slop + 
        if (direction.x < 0 && direction.y < 0 && rayCastHit.collider.name == "bot1")
        {
            float find_angle = 180 - (90 - Vector3.Angle(-direction, rayCastHit.normal));
            reflectionAngle = new Vector2(Mathf.Cos(find_angle * Mathf.Deg2Rad), Mathf.Sin(find_angle * Mathf.Deg2Rad));
            //  Debug.Log(reflectionAngle);
            return reflectionAngle;
        }
        // bot Wall Slop -
        if (direction.x > 0 && direction.y < 0 && rayCastHit.collider.name == "bot1")
        {
            float find_angle = (90 - Vector3.Angle(-direction, rayCastHit.normal));
            reflectionAngle = new Vector2(Mathf.Cos(find_angle * Mathf.Deg2Rad), Mathf.Sin(find_angle * Mathf.Deg2Rad));
            //  Debug.Log(reflectionAngle);
            return reflectionAngle;
        }

        //leftWall slop -
        if (direction.x < 0 && direction.y > 0 && rayCastHit.collider.name == "left1")
        {
            float find_angle = 90 - (90 - Vector3.Angle(-direction, rayCastHit.normal));
            reflectionAngle = new Vector2(Mathf.Cos(find_angle * Mathf.Deg2Rad), Mathf.Sin(find_angle * Mathf.Deg2Rad));
            //  Debug.Log(reflectionAngle);
            return reflectionAngle;
        }
        //leftWall slop +

        if (direction.x < 0 && direction.y < 0 && rayCastHit.collider.name == "left1")
        {
            float find_angle = -90 + (90 - Vector3.Angle(-direction, rayCastHit.normal));
            reflectionAngle = new Vector2(Mathf.Cos(find_angle * Mathf.Deg2Rad), Mathf.Sin(find_angle * Mathf.Deg2Rad));
            // Debug.Log(reflectionAngle);
            return reflectionAngle;
        }

        return reflectionAngle;
    }

}

