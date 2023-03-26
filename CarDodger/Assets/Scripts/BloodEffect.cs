using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BloodEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer bloodScreen;
    //private SpriteRenderer bloodScreen;
    public bool sangreActiva = false;

    void Start()
    {
        // Obtener el componente SpriteRenderer del objeto Blood Screen
        bloodScreen = GetComponent<SpriteRenderer>();

        // Deshabilitar el componente SpriteRenderer al inicio
        bloodScreen.color = new Color(255,255,255,0);
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void activarSangre()
    {
        bloodScreen.color = new Color(255, 255, 255, 255);
        sangreActiva = true;
    }

    //public void desactivarSangre()
    //{
    //    sangreCan.enabled = false;
    //    sangreIm.enabled = false;
    //}

    public void FadeBloodScreen()
    {

        // Iniciar la corrutina 
        
        bloodScreen.color -= new Color(0, 0, 0, 55);


    }

  

}
