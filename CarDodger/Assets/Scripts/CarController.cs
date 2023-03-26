using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Controller _controller;
    private RoadMovement _roadMovement;
    public AudioSource colisionFx = new AudioSource();

    // Start is called before the first frame update
    void Start()
    {
        colisionFx = GetComponent<AudioSource>();

        _controller = FindObjectOfType<Controller>();
        _roadMovement = FindObjectOfType<RoadMovement>();
    }

    private void Update()
    {
        Vector3 tilt = Input.acceleration;
        transform.Translate(tilt.x, 0, 0);

        if (Input.touchCount > 0) //Comprobamos si el boost está activado y lo aplicamos, si es el caso.
        {
            _controller.ActivarBoost();
            _roadMovement.Accelerate();
        }
        else
        {
            _controller.DesactivarBoost();
            _roadMovement.Deccelerate();
        }
    }

    private void OnColisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Muerte"))
        {
            //Llamamos al metodo colision, que restara vida al objeto y comprobara si ha perdido la suficiente como par aque sea game over
            _controller.Colision(); 

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coche"))
        {
            _controller.Colision(); //Llamamos a colision
            colisionFx.enabled = true;


        }
    }

    private void OnTriggerExit()
    {

        colisionFx.enabled = false;

    }
}
