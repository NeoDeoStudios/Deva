using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsLink : MonoBehaviour
{
    public void onClickTwitter()
    {
        Application.OpenURL("https://twitter.com/DevaExe");
    }
    public void onClickItchio()
    {
        Application.OpenURL("https://neo-deo-studios.itch.io/");
    }
    public void onClickWebpage()
    {
        Application.OpenURL("https://neodeostudios.github.io/Portfolio/");
    }
}
