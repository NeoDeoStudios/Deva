using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AjustesMenu : MonoBehaviour
{
    public GameObject menu, mainMenu;
    bool actMainMenu;
    static bool change = false;
    public PestanasMenu pestanas;
    private bool desactivado = false;

    public void Update()
    {
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
