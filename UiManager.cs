using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
 
    public GameObject mainMenu;
    public GameObject Gui;
    public GameObject WinnerMenu;
    public Animator mainMenuAnimator;
    public Animator guiAnimator;
    public Animator WinnerMenuAnimator;

    public TMP_Text bombsTxt;

    void Awake(){
        mainMenuAnimator = mainMenu.GetComponent<Animator>();
        guiAnimator = Gui.GetComponent<Animator>();
        WinnerMenuAnimator = WinnerMenu.GetComponent<Animator>();
    }




    public void hideMainMenu(){
        mainMenuAnimator.SetBool("hideMainMenu" , true);
        showGui();
    }
    public void showMainMenu(){
        mainMenuAnimator.SetBool("hideMainMenu" , false);
        hideGui();
    }

    public void hideGui(){
        guiAnimator.SetBool("hideGui" , true);
    }
    public void showGui(){
        guiAnimator.SetBool("hideGui" , false);
    }

    public void UpdateBombsTxt(string txt){
        bombsTxt.text = txt;
    }

    public void hideWinnerMenu(){
        WinnerMenuAnimator.SetBool("winner" , false);
    }
    public void showWinnerMenu(){
        WinnerMenuAnimator.SetBool("winner" , true);
    }

    public void Quit(){
        Application.Quit();
    }

}
