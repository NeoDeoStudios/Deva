using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Session : MonoBehaviour
{
    private Text question;
    private static GameObject b1Text, b2Text, b3Text, b4Text;
    private GameObject prueba;
    public Store store = new Store();
    private static bool nextQ = false;
    private static int currentQ;
    public questionAnim qAnim;
    private static int response;
    public static int currentSession = -1;
    public responsesAnim rAnim;
    private static bool asked = false;
    public static GameObject deva;
    private GameObject sesion;
    private static string[] splitQuestion;
    private static string[] splitResponse = new string[] { };
    private static int qLineCount = 0;
    private static int rLineCount = 0;
    private static bool changed = false;
    private static int startQ;
    private static int[] qNumbers = new int[6] { 0, 12, 13, 13, 10, 5};
    private static int previous;
    private static bool previousChecked = false;
    private static bool skip = false;
    private static bool preguntada = false;
    private static bool respondiendo = false;
    private static bool enviado = false;
    private static bool desactivar = false;
    public Store storeTest;
    public PestanasDeva pestanas;
    private bool cambiarMenu=false;
    private bool goBack = false;
    public int love = 0;
    public int sadness = 0;
    public int hatred = 0;
    public int stability = 0;
    public int neutral = 0;
    public static bool emotionsChecked = false;
    public List<int> emotions;
    string decision;
    private bool added = false;
    private bool devaClicked = false;
    Color normal;
    Color highlight;
    Color clear;
    Color alpha;
    private bool sorpresa = false;
    private static bool sorpresaHecha = false;
    public AudioSource source { get { return GetComponent<AudioSource>(); } }
    public AudioClip clip;
    public bool cambiar = false;
    public Button devaButton;

    void Start()
    {
        decision = GameState.gameState.decision;
        currentQ = GameState.gameState.currentQ;
        currentSession = GameState.gameState.currentSession;
        love = GameState.gameState.love;
        sadness = GameState.gameState.sadness;
        hatred = GameState.gameState.hatred;
        stability = GameState.gameState.stability;
        neutral = GameState.gameState.neutral;

        devaButton.onClick.AddListener(clickDeva);

        desactivar = false;

        if (!sorpresaHecha)
        {
            sorpresa = GameState.gameState.sorpresa;
        }
        else
        {
            sorpresa = false;
        }


        startQ = currentQ;
        currentSession++;

        if (currentSession == 0)
        {
            emotions = new List<int>();
            store.init();
            GameState.gameState.store = store;
        }
        else
        {
            store = GameState.gameState.store;
            previous = GameState.gameState.previous;
        }
        question = GameObject.Find("Question").GetComponentInChildren<Text>();
        clear = new Color(0.2745f, 0.2745f, 0.2745f, 1.0f);

        b1Text = GameObject.Find("1");
        b2Text = GameObject.Find("2");
        b3Text = GameObject.Find("3");
        b4Text = GameObject.Find("4");
        deva = GameObject.Find("Deva");
        sesion = GameObject.Find("Sesion_num");

        b1Text.GetComponentInChildren<Button>().interactable = false;
        b2Text.GetComponentInChildren<Button>().interactable = false;
        b3Text.GetComponentInChildren<Button>().interactable = false;
        b4Text.GetComponentInChildren<Button>().interactable = false;

        b1Text.GetComponentInChildren<Text>().color = clear;
        b2Text.GetComponentInChildren<Text>().color = clear;
        b3Text.GetComponentInChildren<Text>().color = clear;
        b4Text.GetComponentInChildren<Text>().color = clear;


        deva.GetComponentInChildren<Button>().interactable = true;

        b1Text.GetComponentInChildren<Text>().text = store.pool.questions[currentQ].answer[0];
        b2Text.GetComponentInChildren<Text>().text = store.pool.questions[currentQ].answer[1];
        b3Text.GetComponentInChildren<Text>().text = store.pool.questions[currentQ].answer[2];
        b4Text.GetComponentInChildren<Text>().text = store.pool.questions[currentQ].answer[3];
        sesion.GetComponentInChildren<Text>().text = ("SESIÓN " + currentSession);

        splitQuestion = splitString(store.pool.questions[currentQ].question);

        normal = new Color(0.772f, 0.772f, 0.772f, 1.0f);
        
        highlight = question.color;

        if(currentQ == 0)
        {
            question.text = splitQuestion[0];
        }
        else
        {
            question.text = splitQuestion[0];
        }

        switch (decision)
        {
            case "Love":
                changeColorPink();
                break;
            case "Sadness":
                changeColorBlue();
                break;
            case "Hatred":
                changeColorYellow();
                break;
            case "Stability":
                changeColorRed();
                break;
            case "Neutral":
                changeColorRed();
                break;
        }

        question.color = normal;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Respondiendo " + respondiendo);
        Debug.Log("love: " + love + "hatred: " + hatred + "sadness: " + sadness + "stability: " + stability);
        //Cambio entre escenas
 
            if (cambiarMenu && pestanas.animacionDerecha.GetBool("cambioPestana"))
            {
                switch (decision)
                {
                    case "Love":
                        GameState.gameState.colorDeva = "Pink";
                        pestanas.pestanaDerecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/pestaña-derecha-pink");
                        pestanas.pestanaIzquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/pestaña-izquierda-pink");
                        break;
                    case "Sadness":
                        GameState.gameState.colorDeva = "Blue";
                        pestanas.pestanaDerecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/pestaña-derecha-blue");
                        pestanas.pestanaIzquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/pestaña-izquierda-blue");
                        break;
                    case "Hatred":
                        GameState.gameState.colorDeva = "Yellow";
                        pestanas.pestanaDerecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/pestaña-derecha-yellow");
                        pestanas.pestanaIzquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/pestaña-izquierda-yellow");
                        break;
                    case "Stability":
                        GameState.gameState.colorDeva = "Red";
                        break;
                    case "Neutral":
                        GameState.gameState.colorDeva = "Red";
                        break;
                }
                pestanas.cerrarPestanas();
            }
            if (pestanas.animacionDerecha.GetBool("cambioColor"))
            {
                SceneManager.LoadScene("deva_menu", LoadSceneMode.Single);
            }
            if (!cambiarMenu)
            {
            Debug.Log(respondiendo);
            if (respondiendo)
            {
                question.color = clear;
                qLineCount = 0;
                b1Text.GetComponentInChildren<Text>().color = highlight;
                b2Text.GetComponentInChildren<Text>().color = highlight;
                b3Text.GetComponentInChildren<Text>().color = highlight;
                b4Text.GetComponentInChildren<Text>().color = highlight;

                b1Text.GetComponentInChildren<Button>().interactable = true;
                b2Text.GetComponentInChildren<Button>().interactable = true;
                b3Text.GetComponentInChildren<Button>().interactable = true;
                b4Text.GetComponentInChildren<Button>().interactable = true;
            }

            Debug.Log("ey");
            if (rLineCount == 0 && enviado)
            {
                enviado = false;
                splitResponse = splitString(store.genericResponses.cResponse[currentQ].cResponse[response - 1]);
                if (!added)
                {
                    GameState.gameState.listaCRespuestas.Add(store.genericResponses.cResponse[currentQ].cResponse[response - 1]);
                    GameState.gameState.listaRespuestas.Add(store.pool.questions[currentQ].answer[response - 1]);
                    GameState.gameState.listaPreguntas.Add(store.pool.questions[currentQ].question);
                    added = true;
                }

                if (currentQ == 4)
                {                   
                    previous = response;
                }
                if (currentQ == 5 && response != previous)
                {
                    Debug.Log("HOLA");
                    store.genericResponses.cResponse[currentQ].cResponse[response - 1] = "Pero eso no tiene sentido./A no ser que te guste la incomodidad, claro.";
                    splitResponse = splitString(store.genericResponses.cResponse[currentQ].cResponse[response - 1]);
                }
                if (currentQ == 9)
                {
                    previous = response;
                }
                if (currentQ == 10 && !previousChecked)
                {
                    if (response == previous)
                    {
                        store.genericResponses.cResponse[currentQ].cResponse[response - 1] = "Eso es fantástico, me alegro mucho por ti.";
                        splitResponse = splitString(store.genericResponses.cResponse[currentQ].cResponse[response - 1]);
                        previousChecked = true;
                    }
                    else if (response < previous)
                    {
                        store.genericResponses.cResponse[currentQ].cResponse[response - 1] = "Oh.Lo siento mucho./En la medida en la que puedo sentir.";
                        splitResponse = splitString(store.genericResponses.cResponse[currentQ].cResponse[response - 1]);
                        previousChecked = true;
                    }
                    else
                    {
                        store.genericResponses.cResponse[currentQ].cResponse[response - 1] = "Eso no tiene sentido./Por favor, si quieres que esto funcione, no me mientas.";
                        splitResponse = splitString(store.genericResponses.cResponse[currentQ].cResponse[response - 1]);
                        previousChecked = true;
                    }
                }
                if (currentQ == 11)
                {
                    if (response == 2)
                    {
                        store.genericResponses.cResponse[currentQ].cResponse[response - 1] = store.genericResponses.cResponse[currentQ + 1].cResponse[response - 1];
                        splitResponse = splitString(store.genericResponses.cResponse[currentQ].cResponse[response - 1]);
                        skip = true;
                    }
                }

                if (currentQ == 13)
                {
                    if (response == 1)
                    {
                        store.pool.questions[14].question = "¡Hola de nuevo!/" +
        "Me alegra que quieras continuar con el proceso de personalización./" +
        "Desgraciadamente, es más corto de lo que me gustaría./" +
        "El proceso completo consta de 5 sesiones./" +
        "Tengo entendido que el tiempo es relativo para vosotros./" +
        "Espero que se te pase rápido, porque lo estaré haciendo bien./" +
        "¡Voy a esforzarme mucho para gustarte!/" +
        "Por cierto…/" + "He encontrado en mis archivos algunos datos sobre el águila./" +
                            "Varios países y familias la han utilizado como símbolo./Buscan representar con ella dignidad, majestuosidad, triunfo…/" +
                            "Pero muchos de esos grupos le deben su triunfo a la violencia./Supongo que el poder tiene un precio a pagar cuando se trata de humanos./" +
                            "¿Te sorprende que me acuerde de tus gustos?/Al fin y al cabo, por eso estamos haciendo estos cuestionarios./Recuerdo cada cosa que me dices./" +
                            "La estudio minuciosamente y cambio mis patrones de razonamiento./Todo ello para adaptarme a ti./Te lo dije, poco a poco una gran amiga./" +
                            "Continuemos con la personalización./En la sesión 2 hablaremos del comportamiento social./Al conocer a alguien nuevo…";
                    }
                    else if (response == 2)
                    {
                        store.pool.questions[14].question = "¡Hola de nuevo!/" +
        "Me alegra que quieras continuar con el proceso de personalización./" +
        "Desgraciadamente, es más corto de lo que me gustaría./" +
        "El proceso completo consta de 5 sesiones./" +
        "Tengo entendido que el tiempo es relativo para vosotros./" +
        "Espero que se te pase rápido, porque lo estaré haciendo bien./" +
        "¡Voy a esforzarme mucho para gustarte!/" +
        "Por cierto…/" + "Parece ser que los sauces se han relacionado siempre con la tristeza./Debe de ser por su forma./" +
                            "Crecen cerca de ríos y sus hojas cuelgan largas./Siendo grandes pero alicaídos dan sensación de depresión./Incluso habéis llamado a una especie “sauce llorón”./" +
                            "Pero pese a todo ello, hay culturas que discrepan./Algunas relacionan al sauce con la magia, con lo místico…/… y con los sueños./" +
                            "¿Te sorprende que me acuerde de tus gustos?/Al fin y al cabo, por eso estamos haciendo estos cuestionarios./Recuerdo cada cosa que me dices./" +
                            "La estudio minuciosamente y cambio mis patrones de razonamiento./Todo ello para adaptarme a ti./Te lo dije, poco a poco una gran amiga./" +
                            "Continuemos con la personalización./En la sesión 2 hablaremos del comportamiento social./Al conocer a alguien nuevo…";
                    }
                    else if (response == 3)
                    {
                        store.pool.questions[14].question = "¡Hola de nuevo!/" +
        "Me alegra que quieras continuar con el proceso de personalización./" +
        "Desgraciadamente, es más corto de lo que me gustaría./" +
        "El proceso completo consta de 5 sesiones./" +
        "Tengo entendido que el tiempo es relativo para vosotros./" +
        "Espero que se te pase rápido, porque lo estaré haciendo bien./" +
        "¡Voy a esforzarme mucho para gustarte!/" +
        "Por cierto…/" + "¿Sabías que apenas existen caballos en libertad?/Se les cría para ser domados y montados por vosotros./" +
                            "Es cierto que hay algunas manadas “libres”./Sin embargo suelen ser vigiladas por humanos./" +
                            "Hacen esto para poder domarlos más tarde, fuertes y rápidos./Por lo que el resultado acaba siendo el mismo./" +
                            "¿Te sorprende que me acuerde de tus gustos?/Al fin y al cabo, por eso estamos haciendo estos cuestionarios./Recuerdo cada cosa que me dices./" +
                            "La estudio minuciosamente y cambio mis patrones de razonamiento./Todo ello para adaptarme a ti./Te lo dije, poco a poco una gran amiga./" +
                            "Continuemos con la personalización./En la sesión 2 hablaremos del comportamiento social./Al conocer a alguien nuevo…";
                    }
                    else
                    {
                        store.pool.questions[14].question = "¡Hola de nuevo!/" +
        "Me alegra que quieras continuar con el proceso de personalización./" +
        "Desgraciadamente, es más corto de lo que me gustaría./" +
        "El proceso completo consta de 5 sesiones./" +
        "Tengo entendido que el tiempo es relativo para vosotros./" +
        "Espero que se te pase rápido, porque lo estaré haciendo bien./" +
        "¡Voy a esforzarme mucho para gustarte!/" +
        "Por cierto…/" + "En general regaláis rosas como símbolo de amor./Se las suele asociar con ello y con la pasión./" +
                            "Sin embargo, la verdadera flor del amor es el girasol./Piénsalo, son seres que nacen enamorados./Dedican toda su vida a contemplar aquello que aman./El sol./" +
                            "Y no solo eso, no es algo unilateral./El sol, a cambio, les da su energía y les hace vivir y ser bellos./" +
                            "¿Te sorprende que me acuerde de tus gustos?/Al fin y al cabo, por eso estamos haciendo estos cuestionarios./Recuerdo cada cosa que me dices./" +
                            "La estudio minuciosamente y cambio mis patrones de razonamiento./Todo ello para adaptarme a ti./Te lo dije, poco a poco una gran amiga./" +
                            "Continuemos con la personalización./En la sesión 2 hablaremos del comportamiento social./Al conocer a alguien nuevo…";
                    }
                }

                if (currentQ == 19)
                {
                    if (response == 2)
                    {
                        skip = true;
                    }
                }

                if (currentQ == 25)
                {
                    if (response == 2)
                    {
                        skip = true;
                    }
                }

                if (currentQ == 29)
                {
                    if (response == 2)
                    {
                        skip = true;
                    }
                }
                if (currentQ == 28)
                {
                    if (response == 2)
                    {
                        goBack = true;
                    }
                }
                if (currentQ == 28)
                {
                    if (response == 1)
                    {
                        goBack = false;
                    }
                }
                if (currentQ == 31)
                {
                    if (response == 2)
                    {
                        skip = true;
                    }
                }

                if (currentQ == 39)
                {
                    if (response == 2)
                    {
                        skip = true;
                    }
                }
                if (!emotionsChecked)
                {
                    love += store.genericResponses.cResponse[currentQ].karma[response - 1][0];
                    sadness += store.genericResponses.cResponse[currentQ].karma[response - 1][1];
                    hatred += store.genericResponses.cResponse[currentQ].karma[response - 1][2];
                    stability += store.genericResponses.cResponse[currentQ].karma[response - 1][3];
                    neutral += store.genericResponses.cResponse[currentQ].karma[response - 1][4];

                    if (currentQ == 47)
                    {
                        emotions = new List<int>();
                        emotions.Add(love);
                        emotions.Add(sadness);
                        emotions.Add(hatred);
                        emotions.Add(stability);
                        emotions.Sort();
                        emotions.Reverse();
                        if (emotions[0] == love)
                        {
                            store.loveDecission();
                        }
                        else if (emotions[0] == sadness)
                        {
                            store.sadnessDecission();
                        }
                        else if (emotions[0] == hatred)
                        {
                            store.hateDecission();
                        }
                        else if (emotions[0] == stability)
                        {
                            store.stabilityDecission();
                        }
                    }

                    if (currentQ == 48)
                    {
                        emotions = new List<int>();
                        emotions.Add(love);
                        emotions.Add(sadness);
                        emotions.Add(hatred);
                        emotions.Add(stability);
                        emotions.Add(neutral);
                        emotions.Sort();
                        emotions.Reverse();
                        if (emotions[0] == love)
                        {
                            store.initLove();
                            decision = "Love";
                        }
                        else if (emotions[0] == sadness)
                        {
                            store.initSadness();
                            decision = "Sadness";
                        }
                        else if (emotions[0] == hatred)
                        {
                            store.initHatred();
                            decision = "Hatred";
                        }
                        else if (emotions[0] == stability)
                        {
                            store.initStability();
                            decision = "Stability";
                        }
                        else if (emotions[0] == neutral)
                        {
                            store.initNeutral();
                            decision = "Neutral";
                        }
                    }


                    emotionsChecked = true;
                }
                respondiendo = false;
                question.text = splitResponse[rLineCount];
                question.color = normal;
                b1Text.GetComponentInChildren<Text>().color = clear;
                b2Text.GetComponentInChildren<Text>().color = clear;
                b3Text.GetComponentInChildren<Text>().color = clear;
                b4Text.GetComponentInChildren<Text>().color = clear;
            }
            if (devaClicked && !respondiendo && !desactivar)
                {
                
                    if (qLineCount == splitQuestion.Length - 1 && !preguntada)
                    {
                        respondiendo = true;
                        question.color = Color.clear;
                    }
                

                if (qLineCount < splitQuestion.Length - 1 && !preguntada)
                    {
                        qLineCount++;
                        if (currentQ == 53 && hatred >= 1000 && qLineCount == 32)
                        {
                            ///AQUI SERIA PONER QUE SE CIERREN LAS PESTAÑAS SE ABRAN Y SALGA ODIO
                            /*
                            decision = "Hatred";
                            pestanas.cerrarPestanas();*/
                        }
                        question.text = splitQuestion[qLineCount];
                    }

                    if (!preguntada && !respondiendo)
                    {
                        
                                             
                    }
                    else if
                     (preguntada)
                {
                    
                    if (rLineCount < splitResponse.Length - 1)
                    {
                        if (!respondiendo)
                        {
                            rLineCount++;
                            question.text = splitResponse[rLineCount];
                        }
                    }
                    else
                    {
                        //changed = false;
                        rLineCount = 0;
                        if (!skip)
                        {
                            currentQ++;
                        }
                        else
                        {
                            currentQ += 2;
                            skip = false;
                        }
                        if (goBack)
                        {
                            currentQ--;
                            currentSession--;
                            pestanas.abrirPestanas();
                            GameState.gameState.store = store;
                            cambiarMenu = true;
                        }
                        if (currentQ == 59)
                        {
                            if ((currentQ - startQ) > qNumbers[currentSession])
                            {

                                if (currentQ > 58 && stability >= 1000)
                                {
                                    store.estabilidadInfinito();
                                }

                                if (currentQ > 58 && hatred >= 1000)
                                {
                                    store.odioInfinito();
                                }

                                if (currentQ > 58 && love >= 1000)
                                {
                                    store.amorInfinito();
                                }
                            }
                        }
                        else if (currentQ > 59)
                        {
                            if ((currentQ - startQ) > 0)
                            {
                                if (stability >= 1000)
                                {
                                    store.estabilidadInfinito();
                                }

                                if (hatred >= 1000)
                                {
                                    store.odioInfinito();
                                }
                            }

                            if (love >= 1000)
                            {
                                if (!sorpresa)
                                {
                                    if ((currentQ - startQ) > 2)
                                    {
                                        store.amorInfinito();
                                    }
                                }
                                else
                                {
                                    if ((currentQ - startQ) > 1)
                                    {
                                        store.amorInfinito();
                                    }
                                }
                            }

                        }
                        splitQuestion = splitString(store.pool.questions[currentQ].question);
                        question.text = splitQuestion[qLineCount];
                        qLineCount = 0;
                        nextQ = false;
                        asked = false;
                        preguntada = false;
                        respondiendo = false;
                        enviado = false;
                        changeQuestion();
                        //deva.GetComponentInChildren<Button>().interactable = false;
                    }
                }

                devaClicked = false;
                devaClicked = false;
            }

                /*if (qLineCount < splitQuestion.Length)
                {
                    if (Input.GetMouseButtonDown(0) && !asked && !nextQ)
                    {
                        if (qLineCount != splitQuestion.Length - 1)
                        {
                            //changed = false;
                        }
                        else
                        {
                            asked = true;
                        }
                    if (rLineCount != splitResponse.Length - 1)
                    {
                        //changed = false;
                    }
                    else
                    {
                        if (nextQ)
                        {
                            //changed = false;
                            rLineCount = 0;
                            if (!skip)
                            {
                                currentQ++;
                            }
                            else
                            {
                                currentQ += 2;
                                skip = false;
                            }
                            if (goBack)
                            {
                                currentQ--;
                                currentSession--;
                                pestanas.abrirPestanas();
                                GameState.gameState.store = store;
                                cambiarMenu = true;
                            }
                            if (currentQ == 59)
                            {
                                if ((currentQ - startQ) > qNumbers[currentSession])
                                {

                                    if (currentQ > 58 && stability >= 1000)
                                    {
                                        store.estabilidadInfinito();
                                    }

                                    if (currentQ > 58 && hatred >= 1000)
                                    {
                                        store.odioInfinito();
                                    }

                                    if (currentQ > 58 && love >= 1000)
                                    {
                                        store.amorInfinito();
                                    }
                                }
                            }
                            else if (currentQ > 59)
                            {
                                if ((currentQ - startQ) > 0)
                                {
                                    if (stability >= 1000)
                                    {
                                        store.estabilidadInfinito();
                                    }

                                    if (hatred >= 1000)
                                    {
                                        store.odioInfinito();
                                    }
                                }

                                if (love >= 1000)
                                {
                                    if (!sorpresa)
                                    {
                                        if ((currentQ - startQ) > 2)
                                        {
                                            store.amorInfinito();
                                        }
                                    }
                                    else
                                    {
                                        if ((currentQ - startQ) > 1)
                                        {
                                            store.amorInfinito();
                                        }
                                    }
                                }

                            }
                            splitQuestion = splitString(store.pool.questions[currentQ].question);
                            qLineCount = 0;
                            nextQ = false;
                            asked = false;
                            changeQuestion();
                            deva.GetComponentInChildren<Button>().interactable = false;
                        }
                        else { }
                    }
                }
                }*/
            if (!cambiar)
            {
                
            }
        }
        
    }

    public void buttonPressed()
    {
        preguntada = true;
        nextQ = true;
        asked = false;
        qLineCount = 0;
        enviado = true;

        //question.color = highlight;

            string name = EventSystem.current.currentSelectedGameObject.name;
            response = int.Parse(name);
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/boton-pulsado");
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/boton-sinpulsar");
        clear = new Color(0.2745f, 0.2745f, 0.2745f, 1.0f);
        

        

        b1Text.GetComponent<Button>().interactable = false;
            b2Text.GetComponent<Button>().interactable = false;
            b3Text.GetComponent<Button>().interactable = false;
            b4Text.GetComponent<Button>().interactable = false;
        //changed = true;

        if (currentQ == 13)
        {
            previousChecked = false;
            previous = response;
            GameState.gameState.previous = response;
        }


    }

    public void changeQuestion()
    {
        asked = false;    
        nextQ = false;
        emotionsChecked = false;
        added = false;
        b1Text.GetComponentInChildren<Text>().text = store.pool.questions[currentQ].answer[0];
        b2Text.GetComponentInChildren<Text>().text = store.pool.questions[currentQ].answer[1];
        b3Text.GetComponentInChildren<Text>().text = store.pool.questions[currentQ].answer[2];
        b4Text.GetComponentInChildren<Text>().text = store.pool.questions[currentQ].answer[3];

        if(currentQ == 59)
        {
            pestanas.abrirPestanas();
            GameState.gameState.store = store;
            cambiarMenu = true;
            cambiar = true;
            GameState.gameState.decision = decision;
            GameState.gameState.currentQ = currentQ;
            GameState.gameState.currentSession = currentSession;
            GameState.gameState.love = love;
            GameState.gameState.sadness = sadness;
            GameState.gameState.hatred = hatred;
            GameState.gameState.stability = stability;
            GameState.gameState.neutral = neutral;
        }

        if(currentQ > 58)
        {
            if (love >= 1000)
            {
                if (!sorpresa)
                {
                    if ((currentQ - startQ) > 2 && love >= 1000)
                    {
                        pestanas.abrirPestanas();
                        GameState.gameState.store = store;
                        cambiarMenu = true;
                        cambiar = true;
                        GameState.gameState.decision = decision;
                        GameState.gameState.currentQ = currentQ;
                        GameState.gameState.currentSession = currentSession;
                        GameState.gameState.love = love;
                        GameState.gameState.sadness = sadness;
                        GameState.gameState.hatred = hatred;
                        GameState.gameState.stability = stability;
                        GameState.gameState.neutral = neutral;
                    }
                }
                else
                {
                    if ((currentQ - startQ) > 1 && love >= 1000)
                    {
                        pestanas.abrirPestanas();
                        GameState.gameState.store = store;
                        cambiarMenu = true;
                        cambiar = true;
                        GameState.gameState.decision = decision;
                        GameState.gameState.currentQ = currentQ;
                        GameState.gameState.currentSession = currentSession;
                        GameState.gameState.love = love;
                        GameState.gameState.sadness = sadness;
                        GameState.gameState.hatred = hatred;
                        GameState.gameState.stability = stability;
                        GameState.gameState.neutral = neutral;
                        sorpresa = false;
                        sorpresaHecha = true;
                    }
                }

                
            }else if(stability > 1000 || hatred > 1000)
            {
                if ((currentQ - startQ) > 0)
                {
                    pestanas.abrirPestanas();
                    GameState.gameState.store = store;
                    cambiarMenu = true;
                    cambiar = true;
                    GameState.gameState.decision = decision;
                    GameState.gameState.currentQ = currentQ;
                    GameState.gameState.currentSession = currentSession;
                    GameState.gameState.love = love;
                    GameState.gameState.sadness = sadness;
                    GameState.gameState.hatred = hatred;
                    GameState.gameState.stability = stability;
                    GameState.gameState.neutral = neutral;
                }
            }
        }
        else
        {
            if ((currentQ - startQ) > qNumbers[currentSession])
            {
                question.color = clear;
                desactivar = true;
                cambiar = true;
                GameState.gameState.decision = decision;
                GameState.gameState.currentQ = currentQ;
                GameState.gameState.currentSession = currentSession;
                GameState.gameState.love = love;
                GameState.gameState.sadness = sadness;
                GameState.gameState.hatred = hatred;
                GameState.gameState.stability = stability;
                GameState.gameState.neutral = neutral;
                pestanas.abrirPestanas();
                GameState.gameState.store = store;
                PlaySound();
                cambiarMenu = true;
                cambiar = true;
            }
        }
    }

    public void clickScreen()
    {
            if (rLineCount != splitResponse.Length - 1)
            {
                //changed = false;
            }
        else
        {
            if (nextQ)
            {
                //changed = false;
                rLineCount = 0;
                if (!skip)
                {
                        currentQ++;
                }
                else
                {
                    currentQ += 2;
                    skip = false;
                }
                if (goBack)
                {
                    currentQ--;
                    currentSession--;
                    pestanas.abrirPestanas();
                    GameState.gameState.store = store;
                    cambiarMenu = true;
                }
                if (currentQ == 59)
                {
                    if ((currentQ - startQ) > qNumbers[currentSession])
                    {

                        if (currentQ > 58 && stability >= 1000)
                        {
                            store.estabilidadInfinito();
                        }

                        if (currentQ > 58 && hatred >= 1000)
                        {
                            store.odioInfinito();
                        }

                        if (currentQ > 58 && love >= 1000)
                        {
                            store.amorInfinito();
                        }
                    }
                }else if (currentQ > 59)
                {
                    if ((currentQ - startQ) > 0)
                    {
                        if (stability >= 1000)
                        {
                            store.estabilidadInfinito();
                        }

                        if (hatred >= 1000)
                        {
                            store.odioInfinito();
                        }
                    }

                    if (love >= 1000)
                    {
                        if (!sorpresa)
                        {
                            if((currentQ - startQ) > 2)
                            {
                                store.amorInfinito();
                            }
                        }
                        else
                        {
                            if ((currentQ - startQ) > 1)
                            {
                                store.amorInfinito();
                            }
                        }
                    }

                }
                    splitQuestion = splitString(store.pool.questions[currentQ].question);
                qLineCount = 0;
                nextQ = false;
                asked = false;
                changeQuestion();
                //deva.GetComponentInChildren<Button>().interactable = false;
            }
            else { }
        }
    }

    public void clickDeva()
    {
        if (!respondiendo)
        {
            devaClicked = true;
        }
    }

    void PlaySound()
    {
        source.PlayOneShot(clip);
    }

    public string[] splitString(string s)
    {
        string[] split = s.Split('/');
        return split;
    }

    public void changeColorYellow()
    {   //Get the animator
        // Actually i was using "Resources" folder in assets folder. And i was loading animation by this way.
        GameObject.Find("Deva").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/deva-yellow");
        pestanas.pestanaDerecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/pestaña-derecha-yellow");
        pestanas.pestanaIzquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaYellow/pestaña-izquierda-yellow");
    }

    public void changeColorRed()
    {
        //Get the animator
        // Actually i was using "Resources" folder in assets folder. And i was loading animation by this way.
        GameObject.Find("Deva").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/deva-red");
        pestanas.pestanaDerecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/pestaña-derecha-red");
        pestanas.pestanaIzquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaRed/pestaña-izquierda-red");
    }

    public void changeColorPink()
    {
        //Get the animator
        // Actually i was using "Resources" folder in assets folder. And i was loading animation by this way.
        GameObject.Find("Deva").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/deva-pink");
        pestanas.pestanaDerecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/pestaña-derecha-pink");
        pestanas.pestanaIzquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaPink/pestaña-izquierda-pink");
    }

    public void changeColorBlue()
    {
        //Get the animator
        //Animator anim = GameObject.Find("Deva").GetComponent<Animator>();
        // Actually i was using "Resources" folder in assets folder. And i was loading animation by this way.
        //anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimacionesDeva/SadBreathing/sad-deva0065");
        GameObject.Find("Deva").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/deva-blue");
        pestanas.pestanaDerecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/pestaña-derecha-blue");
        pestanas.pestanaIzquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/DevaBlue/pestaña-izquierda-blue");
    }
}

