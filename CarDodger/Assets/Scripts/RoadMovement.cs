using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{

    private const float defaultSpeed = -0.5f;
    private float speed = defaultSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
{
    float offset = Time.time * speed;
    GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, offset);
}

    public void Accelerate()
    {
         speed = defaultSpeed*3;
    }

    public void Deccelerate()
    {
        speed = defaultSpeed;
    }

}
