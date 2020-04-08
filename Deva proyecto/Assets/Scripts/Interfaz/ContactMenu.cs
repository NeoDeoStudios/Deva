using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContactMenu : MonoBehaviour
{
    public GameObject menu, mainMenu, twitterBtn, itchioBtn;
    bool actMainMenu;
    static bool change = false;
    public PestanasMenu pestanas;
    private bool desactivado = false;

    public void Update()
    {
        if (GameState.gameState.decision == "Hatred" && GameState.gameState.currentQ > 53 && !desactivado)
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
    }

    public void activeMainMenu()
    {
        actMainMenu = true;
    }
}
