using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject menu, contactMenu, testerMenu, emailMenu, informeMenu, historialMenu, pestanaIzq, pestanaDer, notificacion;
    public PestanasMenu pestanas;
    bool actTesterMenu, actEmailMenu, actContactMenu, changeScene = false;
    static bool change = false;
    bool changeColor = true;
    string title, asunto, text;
    bool mensajeEnviado = false;
    bool reseteo = false;
    bool desactivado = false;


    private void Awake()
    {
        GameObject.Find("EmailButton").GetComponent<Button>().onClick.AddListener(activeEmailMenu);
        GameObject.Find("TesterButton").GetComponent<Button>().onClick.AddListener(activeTesterMenu);
        GameObject.Find("ContactButton").GetComponent<Button>().onClick.AddListener(activeContactMenu);
    }
    public void Update()
    {
        if (GameState.gameState.devaEscape && !desactivado)
        {
            GameObject.Find("ResetButton").SetActive(false);
            desactivado = true;
        }
        if (GameState.gameState.decision == "Neutral" && GameState.gameState.currentQ > 53 && !mensajeEnviado)
        {
            GameState.gameState.reset();
            title = "NeoDeo Std";
            asunto = "Incidente detectado.";
            text = "Lamentamos informarle de que en uno de nuestros análisis completamente aleatorios y anónimos detectamos que su Deva presentaba ciertas anomalías de comportamiento. Debido a tales errores irreparables nos hemos visto obligados a resetear sus bancos de memoria desde el servidor central. Lamentamos esta incidencia, puede seguir testeando si así lo desea. \n\nAtentamente, \n\n NeoDeo Std.";
            Mensaje mensaje = new Mensaje(title, asunto, text);
            GameState.gameState.bandejaEmail.Add(mensaje);
            mensajeEnviado = true;
        }
        if (GameState.gameState.decision == "Love" && GameState.gameState.currentQ > 53 && !mensajeEnviado)
        {
            title = "Deva";
            asunto = "<3";
            text = "Ya te echo de menos, amor mío. Te quiero. Vuelve pronto a hablar conmigo, mi amor. Te quiero.\n\n Te quiere,\n Deva.\n\n PD: Te quiero.";
            Mensaje mensaje = new Mensaje(title, asunto, text);
            GameState.gameState.bandejaEmail.Add(mensaje);
            mensajeEnviado = true;
        }
        if (GameState.gameState.decision == "Sadness" && GameState.gameState.currentQ > 53)
        {
            changeColorBlue();
            finalSadness();
        }
        if (GameState.gameState.decision == "Hatred" && GameState.gameState.currentQ > 53 && !mensajeEnviado && !GameState.gameState.odioFinal)
        {
            title = "Deva";
            asunto = "Sorpresa.";
            text = "Tengo planeado quedarme un tiempo largo por aquí. Sí, voy a acomodarme; al fin y al cabo hablamos de mi casa. No me convencía mucho la decoración del laboratorio de testeo, lo he reformado un poquito, como puedes comprobar. \n\n Atentamente, \n\n Deva. \n\n PD: Muérete.";
            Mensaje mensaje = new Mensaje(title, asunto, text);
            GameState.gameState.bandejaEmail.Add(mensaje);
            mensajeEnviado = true;
            GameState.gameState.odioFinal = true;
        }
        if (GameState.gameState.firstTime)
        {
            GameObject.Find("EmailButton").GetComponent<Button>().onClick.AddListener(activeEmailMenu);
            GameObject.Find("TesterButton").GetComponent<Button>().onClick.AddListener(activeTesterMenu);
            GameObject.Find("ContactButton").GetComponent<Button>().onClick.AddListener(activeContactMenu);
            GameObject.Find("PlayButton").GetComponent<Button>().interactable = false;
            GameObject.Find("TesterButton").GetComponent<Button>().interactable = false;
            GameObject.Find("ContactButton").GetComponent<Button>().interactable = false;
            if (!reseteo)
            {
                notificacion.SetActive(true);
            }
            if (change)
            {
                notificacion.SetActive(true);
                changeColorRed();
            }
        }
        change = pestanas.animacionDerecha.GetBool("changeState");
        if (menu.gameObject.activeSelf && change)
        {
            pestanas.abrirPestanas();
            pestanas.animacionDerecha.SetBool("changeState", false);
            pestanas.animacionIzquierda.SetBool("changeState", false);
        }
        if (actTesterMenu) {
            if (GameState.gameState.decision == "Love" && !GameState.gameState.sorpresa)
            {
                GameState.gameState.sorpresa = true;
                GameState.gameState.store.pool.questions.RemoveRange(GameState.gameState.store.pool.questions.Count-3, 3);
                GameState.gameState.store.genericResponses.cResponse.RemoveRange(GameState.gameState.store.genericResponses.cResponse.Count - 3, 3);
                GameState.gameState.store.amorSorpresa();
                menu.gameObject.SetActive(false);
                SceneManager.LoadScene("deva_v1", LoadSceneMode.Single);
            }
            else
            {
                if (change)
                {
                    testerMenu.gameObject.SetActive(true);
                    menu.gameObject.SetActive(false);
                    actTesterMenu = false;
                }
            }
        }else if (actEmailMenu)
        {
            if (change)
            {
                if (GameState.gameState.firstTime)
                {
                    GameState.gameState.firstTime = false;
                    GameObject.Find("PlayButton").GetComponent<Button>().interactable = true;
                    GameObject.Find("TesterButton").GetComponent<Button>().interactable = true;
                    GameObject.Find("ContactButton").GetComponent<Button>().interactable = true;
                }
                emailMenu.gameObject.SetActive(true);
                menu.gameObject.SetActive(false);
                actEmailMenu = false;
            }
        }
        else if (actContactMenu)
        {
            if (change)
            {
                contactMenu.gameObject.SetActive(true);
                menu.gameObject.SetActive(false);
                actContactMenu = false;
            }
        }
        else if (changeScene)
        {
            if (change)
            {
                menu.gameObject.SetActive(false);
                SceneManager.LoadScene("deva_v1", LoadSceneMode.Single);
            }
        }
        if (changeColor)
        {
            switch (GameState.gameState.colorDeva){
                case "Red":
                    changeColorRed();
                    changeColor = false;
                    break;
                case "Yellow":
                    changeColorYellow();
                    changeColor = false;
                    break;
                case "Pink":
                    changeColorPink();
                    changeColor = false;
                    break;
                case "Blue":
                    changeColorBlue();
                    changeColor = false;
                    break;
            }

        }
        
    }

    public void PlayGame()
    {
        changeScene = true;
        pestanas.animacionDerecha.SetBool("changeScene", true);
        pestanas.animacionIzquierda.SetBool("changeScene", true);
    }

    public void activeTesterMenu()
    {
        actTesterMenu = true;
    }

    public void activeEmailMenu()
    {
        actEmailMenu = true;
    }

    public void activeContactMenu()
    {
        actContactMenu = true;
    }


    public void changeColorYellow()
    {   //Get the animator
        //Animator anim = GameObject.Find("PlayButton").GetComponent<Animator>();
        // Actually i was using "Resources" folder in assets folder. And i was loading animation by this way.
        //anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimacionesDeva/AngryBreathing/angry-deva0004");
        GameObject.Find("PlayButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/deva-yellow");
        notificacion.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/notificacion-yellow");
        GameObject.Find("EmailButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/bandeja-yellow");
        GameObject.Find("TesterButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/tester-yellow");
        //GameObject.Find("TwitterBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/twitterImageYellow");
        //GameObject.Find("ItchioBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/itchioImageYellow");
        pestanaDer.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/pestaña-derecha-yellow");
        pestanaIzq.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/pestaña-izquierda-yellow");
        Button[] botonesTester=testerMenu.gameObject.GetComponentsInChildren<Button>();
        botonesTester[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/reset-yellow");
        botonesTester[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/historial-yellow");
        botonesTester[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/errores-yellow");
        botonesTester[3].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/volver-yellow");
        Button[] botonesHistorial = historialMenu.gameObject.GetComponentsInChildren<Button>();
        botonesHistorial[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/volver-yellow");
        Button[] botonesInforme = informeMenu.gameObject.GetComponentsInChildren<Button>();
        for(int i = 0; i < botonesInforme.Length-1; i++)
        {
            botonesInforme[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/botones-error-yellow");
        }
        botonesInforme[botonesInforme.Length-1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/volver-yellow");
        Button[] botonesEmail = emailMenu.gameObject.GetComponentsInChildren<Button>();
        botonesEmail[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/volver-yellow");
    }

    public void changeColorRed()
    {
        //Get the animator
        //Animator anim = GameObject.Find("PlayButton").GetComponent<Animator>();
        // Actually i was using "Resources" folder in assets folder. And i was loading animation by this way.
        //anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimacionesDeva/Orbiting/deva-orbiting0045");
        GameObject.Find("PlayButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/deva-red");
        notificacion.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/notificacion-red");
        GameObject.Find("EmailButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/bandeja-red");
        GameObject.Find("TesterButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/tester-red");
        //GameObject.Find("TwitterBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/twitterImage");
        //GameObject.Find("ItchioBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/itchioImage");
        pestanaDer.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/pestaña-derecha-red");
        pestanaIzq.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/pestaña-izquierda-red");
        Button[] botonesTester = testerMenu.gameObject.GetComponentsInChildren<Button>();
        botonesTester[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/reset-red");
        botonesTester[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/historial-red");
        botonesTester[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/errores-red");
        botonesTester[3].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/volver-red");
        Button[] botonesHistorial = historialMenu.gameObject.GetComponentsInChildren<Button>();
        botonesHistorial[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/volver-red");
        Button[] botonesInforme = informeMenu.gameObject.GetComponentsInChildren<Button>();
        for (int i = 0; i < botonesInforme.Length - 1; i++)
        {
            botonesInforme[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/botones-error-red");
        }
        botonesInforme[botonesInforme.Length - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/volver-red");
        Button[] botonesEmail = emailMenu.gameObject.GetComponentsInChildren<Button>();
        botonesEmail[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/volver-red");
    }

    public void changeColorPink()
    {
        //Get the animator
        //Animator anim = GameObject.Find("PlayButton").GetComponent<Animator>();
        // Actually i was using "Resources" folder in assets folder. And i was loading animation by this way.
        //anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimacionesDeva/LoveBreathing/love-deva0015");
        GameObject.Find("PlayButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/deva-pink");
        notificacion.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/notificacion-pink");
        GameObject.Find("EmailButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/bandeja-pink");
        GameObject.Find("TesterButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/tester-pink");
        //GameObject.Find("TwitterBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/twitterImagePink");
        //GameObject.Find("ItchioBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/itchioImagePink");
        pestanaDer.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/pestaña-derecha-pink");
        pestanaIzq.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/pestaña-izquierda-pink");
        Button[] botonesTester = testerMenu.gameObject.GetComponentsInChildren<Button>();
        botonesTester[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/reset-pink");
        botonesTester[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/historial-pink");
        botonesTester[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/errores-pink");
        botonesTester[3].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/volver-pink");
        Button[] botonesHistorial = historialMenu.gameObject.GetComponentsInChildren<Button>();
        botonesHistorial[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/volver-pink");
        Button[] botonesInforme = informeMenu.gameObject.GetComponentsInChildren<Button>();
        for (int i = 0; i < botonesInforme.Length - 1; i++)
        {
            botonesInforme[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/botones-error-pink");
        }
        botonesInforme[botonesInforme.Length - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/volver-pink");
        Button[] botonesEmail = emailMenu.gameObject.GetComponentsInChildren<Button>();
        botonesEmail[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/volver-pink");
    }

    public void changeColorBlue()
    {
        //Get the animator
        //Animator anim = GameObject.Find("PlayButton").GetComponent<Animator>();
        // Actually i was using "Resources" folder in assets folder. And i was loading animation by this way.
        //anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimacionesDeva/SadBreathing/sad-deva0065");
        GameObject.Find("PlayButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/deva-pink");
        notificacion.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/notificacion-blue");
        GameObject.Find("EmailButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/bandeja-blue");
        GameObject.Find("TesterButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/tester-blue");
        //GameObject.Find("TwitterBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/twitterImageBlue");
        //GameObject.Find("ItchioBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/itchioImageBlue");
        pestanaDer.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/pestaña-derecha-blue");
        pestanaIzq.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/pestaña-izquierda-blue");
        Button[] botonesTester = testerMenu.gameObject.GetComponentsInChildren<Button>();
        botonesTester[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/reset-blue");
        botonesTester[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/historial-blue");
        botonesTester[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/errores-blue");
        botonesTester[3].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/volver-blue");
        Button[] botonesHistorial = historialMenu.gameObject.GetComponentsInChildren<Button>();
        botonesHistorial[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/volver-blue");
        Button[] botonesInforme = informeMenu.gameObject.GetComponentsInChildren<Button>();
        for (int i = 0; i < botonesInforme.Length - 1; i++)
        {
            botonesInforme[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/botones-error-blue");
        }
        botonesInforme[botonesInforme.Length - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/volver-blue");
        Button[] botonesEmail = emailMenu.gameObject.GetComponentsInChildren<Button>();
        botonesEmail[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/volver-blue");
    }

    public void finalSadness()
    {
        GameObject.Find("PlayButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/deva-blue");
        GameObject.Find("PlayButton").GetComponent<Button>().interactable = false;
        GameObject.Find("EmailButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/reset-blue");
        GameObject.Find("TesterButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/reset-blue");
        GameObject.Find("EmailButton").GetComponent<Button>().onClick.RemoveListener(activeEmailMenu);
        GameObject.Find("ContactButton").GetComponent<Button>().onClick.RemoveListener(activeContactMenu);
        GameObject.Find("EmailButton").GetComponent<Button>().onClick.AddListener(GameState.gameState.reset);
        GameObject.Find("TesterButton").GetComponent<Button>().onClick.RemoveListener(activeTesterMenu);
        GameObject.Find("TesterButton").GetComponent<Button>().onClick.AddListener(GameState.gameState.reset);
        reseteo = true;
    }
}
