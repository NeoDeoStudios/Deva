using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class GameState : MonoBehaviour
{
    public string colorDeva;

    public static GameState gameState;

    public Store store;
    public List<Mensaje> bandejaEmail = new List<Mensaje>();

    public int previous = 0;
    public bool firstTime = true;
    public string decision;
    public int currentQ = 0;
    public int currentSession = -1;
    public int love,sadness,hatred,stability,neutral=0;
    public List<string> listaPreguntas;
    public List<string> listaRespuestas;
    public List<string> listaCRespuestas;
    public bool sorpresa = false;
    public int error5count = 0;
    public bool devaEscape = false;
    public bool odioFinal = false;
    


    public void Awake()
    {
        if (gameState == null)
        {
            gameState = this;
            DontDestroyOnLoad(gameObject);
        } else if (gameState != this)
        {
            Destroy(gameObject);
        }
    }

    public void reset()
    {
        GameState.gameState.decision = "";
        GameState.gameState.currentQ = 0;
        GameState.gameState.currentSession = -1;
        GameState.gameState.love = 0;
        GameState.gameState.sadness = 0;
        GameState.gameState.hatred = 0;
        GameState.gameState.stability = 0;
        GameState.gameState.neutral = 0;
        GameState.gameState.firstTime = true;
        colorDeva = "Red";
        bandejaEmail.Clear();
        listaCRespuestas.Clear();
        listaPreguntas.Clear();
        listaRespuestas.Clear();
    }

}
