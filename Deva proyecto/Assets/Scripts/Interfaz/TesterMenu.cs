using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TesterMenu : MonoBehaviour
{
    public GameObject  menu, informeMenu, mainMenu, historialMenu;
    bool actMainMenu, actInformeMenu, actReset, actHistorialMenu;
    static bool change = false;
    public PestanasMenu pestanas;
    private bool desactivado = false;

    public void Update()
    {
        if(GameState.gameState.decision == "Hatred" && GameState.gameState.currentQ > 53 && !desactivado)
        {
            GameObject.Find("ResetButton").SetActive(false);
            desactivado = true;
        }
        change = pestanas.animacionDerecha.GetBool("changeState");
        if (menu.gameObject.activeSelf && change)
        {
            pestanas.abrirPestanas();
            pestanas.animacionDerecha.SetBool("changeState", false);
            pestanas.animacionIzquierda.SetBool("changeState", false);
        }
        if (actMainMenu)
        {
            if (change)
            {
                mainMenu.gameObject.SetActive(true);
                menu.gameObject.SetActive(false);
                actMainMenu = false;
            }
        }
        else if (actInformeMenu)
        {
            if (change)
            {
                informeMenu.gameObject.SetActive(true);
                menu.gameObject.SetActive(false);
                actInformeMenu = false;
            }
        }
        else if (actHistorialMenu)
        {
            if (change)
            {
                historialMenu.gameObject.SetActive(true);
                menu.gameObject.SetActive(false);
                actHistorialMenu = false;
            }
        }
        else if (actReset)
        {
            GameState.gameState.reset();
            if (change)
            {
                SceneManager.LoadScene("deva_menu");
            }
        }
    }

    public void activeMainMenu()
    {
        actMainMenu = true;
    }

    public void activeInformeMenu()
    {
        actInformeMenu = true;
    }

    public void activeHistorialMenu()
    {
        actHistorialMenu = true;
    }

    public void QuitGame()
    {
        actReset = true;
    }
}
