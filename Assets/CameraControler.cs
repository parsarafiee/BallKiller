using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{

    public Transform bg;
    public float parallaxFactor;
    // Start is called before the first frame update
    public Transform mario;
    public Transform luigi;

    public Vector2 camSizeRange = new Vector2(6, 40);
    public Vector2 arenaDimensions = new Vector2(125, 52);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetPos = (mario.position + luigi.position) / 2;
        targetPos.z = -10;
        transform.position = targetPos;


        float xDistance = Mathf.Abs(mario.position.x - luigi.position.x);
        float yDistance = Mathf.Abs(mario.position.y - luigi.position.y);

        float xInterpolator = Mathf.Clamp01( xDistance / arenaDimensions.x);
        float yInterpolator = Mathf.Clamp01( yDistance / arenaDimensions.y);


        Camera.main.orthographicSize = Mathf.Lerp(camSizeRange.x, camSizeRange.y, xInterpolator > yInterpolator ? xInterpolator : yInterpolator);

        bg.position = transform.position * parallaxFactor;

    }
}
