using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Móvil
        /**/
        Vector3 tilt = Input.acceleration;
        tilt.Normalize();
        transform.Translate(tilt);

        

        /**/
    
        // PC
        /**
        if (Input.GetKey("a"))
        {
            transform.Rotate(Vector3.back, Space.Self);
        }

        if (Input.GetKey("d"))
        {
            transform.Rotate(Vector3.forward, Space.Self);
        }
        /**/
    }
}
