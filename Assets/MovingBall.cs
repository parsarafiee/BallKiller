using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBall : MonoBehaviour
{
    public GameObject cue;
    public GameObject whiteBall;
    public Transform cueHead;
    float transmition = 0;
    Vector3 desiredPosition;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {

        if (cue.transform.forward != transform.forward)
        {
            ChangingDirection();
        }
        transmition += 0.01f;

        //this.transform.position = new Vector3(transmition, 0, 0);
        // Vector3 desiredPosition = this.transform.position + (this.transform.right * -currentFollowDist);

        desiredPosition = this.transform.right + this.transform.position;

        this.transform.position = desiredPosition;
        Debug.Log(transmition);
    }


    void ChangingDirection()
    {
        this.transform.position = whiteBall.transform.position;
        this.transform.forward = cue.transform.forward;


    }
}
