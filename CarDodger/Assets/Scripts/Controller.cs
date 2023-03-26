using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    private GameObject car;
    private GameObject enemigos;
    private GameObject interfazInGame;
    private GameObject interfazMuerte;
    private GameObject interfazInicio;
    private GameObject interfazControles;
    private GameObject interfazFirstTime;
    [SerializeField] private GameObject [] Enemigos;

    private AudioPeer audioMicro;

    private Rigidbody rbcar;
    private string _selectedDevice;

    public bool playing;
    private int score;
    public int highScore;
    private string scoreText;
    private string highScoreText;

    private SaveAndLoad saveGame;

    private Vector3 SpawnPosition;
    private bool generando;
    private bool permiteGenerar;

    private bool boost = false;

    public BloodEffect bloodEffect;
    public bool colision = false;

    public Camera cam;
    AudioSource audioSource = new AudioSource();
    int tGeneracion = 3;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        car = GameObject.Find("Police_car");
        enemigos = GameObject.Find("Enemigos");

        interfazInGame = GameObject.Find("Interfaz in-Game");
        interfazMuerte = GameObject.Find("Interfaz Muerte");
        interfazInicio = GameObject.Find("Interfaz Inicial");
        interfazControles = GameObject.Find("Interfaz Controles");
        interfazFirstTime = GameObject.Find("Interfaz FirstTime");

        rbcar = car.GetComponent<Rigidbody>();
        
        interfazMuerte.SetActive(false);
        interfazInGame.SetActive(false);
        interfazFirstTime.SetActive(false);
        interfazControles.SetActive(false);
        car.SetActive(false);
        enemigos.SetActive(false);
        
        playing = false;
        score = 0;
        scoreText = "";
        highScoreText = "";

        saveGame = FindObjectOfType<SaveAndLoad>();
        audioMicro = FindObjectOfType<AudioPeer>();

        generando = false;
        permiteGenerar = false;
        
        FormatearHighScore();
        interfazInicio.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = highScoreText;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (car.active == true)
        {
            audioSource.volume = 0.0f;
            print (car.active);
        } else
        {
            audioSource.volume = 1;
        }

        if (playing)
        {
            // Aqui gestionamos el soplido en el microfono
            if (audioMicro._AmplitudeBuffer > 0.5f && interfazInGame.GetComponentInChildren<Slider>().value > 0) 
            {
                // Limpiar sangre de la patalla
                bloodEffect.FadeBloodScreen();
            }
           
            

            //StartCoroutine(EsperaGenerarCoroutine());

            if (boost == false)
            {
                score++;
                FormatearScore();
                interfazInGame.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = scoreText;

                if (!generando /*&& permiteGenerar*/)
                    StartCoroutine(GenerarCoroutine());

                enemigos.transform.position -= new Vector3(0, 0.02f, 0);
            } else
            { //Si esta acelerando
                score = score + 4;
                FormatearScore();
                interfazInGame.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = scoreText;

                if (!generando /*&& permiteGenerar*/)
                    StartCoroutine(GenerarCoroutine());

                enemigos.transform.position -= new Vector3(0, 0.10f, 0);
            }

            
        }
    }

    IEnumerator EsperaGenerarCoroutine()
    {
        permiteGenerar = false;
        yield return new WaitForSeconds(2);
        permiteGenerar = true;
    }
    
    void GenerarEnemigo()
    {
        SpawnPosition = this.transform.position + Random.onUnitSphere * 2;
        SpawnPosition = new Vector3(SpawnPosition.x, SpawnPosition.y, -0.032f); // Esto es para colocar los coches sobre el plano
        GameObject agujero = Instantiate(Enemigos[Random.Range(0, 5)], SpawnPosition, Quaternion.Euler(-90,0,0)); // Se crea instancia de enemigo
        agujero.transform.SetParent(enemigos.transform);
    }

    IEnumerator GenerarCoroutine()
    {
        generando = true;
        GenerarEnemigo();
        yield return new WaitForSeconds(tGeneracion);
        generando = false;
    }


    public void Colision()
    {
        if (interfazInGame.GetComponentInChildren<Slider>().value < 100)
        {

            interfazInGame.GetComponentInChildren<Slider>().value += 33.4f;
            bloodEffect.activarSangre();
            StartCoroutine(ShakeCamera());
            colision = true;


        }
        else
        {
            this.GameOverScene();
            colision=false;
        }
    }


   
    public void ActivarBoost()
    {
        boost = true;
        tGeneracion = 1;
    }

    public void DesactivarBoost()
    {
        boost = false;
        tGeneracion = 3;
    }

    public void GameOverScene()
    {
        foreach (Transform child in enemigos.transform)
        {
            Destroy(child.gameObject);
        }
        car.SetActive(false);
        bloodEffect.bloodScreen.color -= new Color(0, 0, 0, 255);
        enemigos.SetActive(false);
        interfazInGame.SetActive(false);
        interfazMuerte.SetActive(true);
        playing = false;
        if (score > highScore)
        {
            highScore = score;
        }
        FormatearScore();
        FormatearVida(); //Formateamos vida para que en la siguiente partida la barra este a 0
        FormatearHighScore();
        interfazMuerte.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = scoreText;
        interfazMuerte.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = highScoreText;
        saveGame.SaveData(highScore);
    }

    public void InicioScene()
    {
        interfazMuerte.SetActive(false);
        interfazControles.SetActive(false);
        interfazInicio.SetActive(true);
        FormatearHighScore();
        interfazInicio.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = highScoreText;
    }

    public void ControlesScene()
    {
        interfazMuerte.SetActive(false);
        interfazInicio.SetActive(false);
        interfazControles.SetActive(true);
    }

    public void GameScene()
    {
        if (saveGame.firstTime == 0)
        {
            saveGame.SaveFirstTime();
            interfazInicio.SetActive(false);
            interfazFirstTime.SetActive(true);
            StartCoroutine(SalirDelJuegoCoroutine());
        }
        else
        {
            interfazInicio.SetActive(false);
            interfazMuerte.SetActive(false);
            interfazInGame.SetActive(true);
            car.transform.rotation = Quaternion.Euler(-90, 0, 0);
            car.transform.position = new Vector3(0, -3.2f, -0.287f);
            rbcar.velocity = new Vector3(0, 0, 0);
            rbcar.angularVelocity = new Vector3(0, 0, 0);
            car.SetActive(true);
            enemigos.SetActive(true);
            GenerarEnemigo();
            playing = true;
            score = 0;
            interfazInGame.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "0000";
            FormatearHighScore();
            interfazInGame.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = highScoreText;
        }
    }

    IEnumerator ShakeCamera()
    {
        // Agita la cámara durante 1 segundo
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            // Obtén una posición aleatoria dentro de un cierto radio
            float x = Random.Range(-1f, 1f) * 0.5f;
            float y = Random.Range(-1f, 1f) * 0.5f;

            // Mueve la cámara a la posición aleatoria
            cam.transform.localPosition = new Vector3(x, y, cam.transform.localPosition.z);

            // Aumenta el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            // Espera un frame antes de continuar
            yield return null;
        }

        // Vuelve a la posición inicial de la cámara
        cam.transform.localPosition = new Vector3(0,0, -8.04f);
    }


    IEnumerator SalirDelJuegoCoroutine()
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }

    void FormatearScore()
    {
        if (score >= 0 && score < 10)
        {
            scoreText = "000" + score;
        }
        else if (score >= 10 && score < 100)
        {
            scoreText = "00" + score;
        }
        else if (score >= 100 && score < 1000)
        {
            scoreText = "0" + score;
        }
        else if (score >= 1000)
        {
            scoreText = "" + score;
        }
    }

    void FormatearVida()
    {
        interfazInGame.GetComponentInChildren<Slider>().value = 0.0f;
    }

    void FormatearHighScore()
    {
        if (highScore >= 0 && highScore < 10)
        {
            highScoreText = "000" + highScore;
        }
        else if (highScore >= 10 && highScore < 100)
        {
            highScoreText = "00" + highScore;
        }
        else if (highScore >= 100 && highScore < 1000)
        {
            highScoreText = "0" + highScore;
        }
        else if (highScore >= 1000)
        {
            highScoreText = "" + highScore;
        }
    }

    
}
