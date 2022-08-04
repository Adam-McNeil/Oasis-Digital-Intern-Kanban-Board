using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject userDataUI;
    public GameObject MainMenuUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //Functions to change the login screen UI
    public void ClearScreen() //Turn off all screens
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        MainMenuUI.SetActive(false);
    }

    public void LoginScreen() //Back button
    {
        ClearScreen();
        Debug.Log("we called login screen");
        loginUI.SetActive(true);
    }
    public void RegisterScreen() // Regester button
    {
        ClearScreen();
        registerUI.SetActive(true);
    }

    public void MainMenuScreen()
    {
      ClearScreen();
      MainMenuUI.SetActive(true);
    }
  public void UserDataScreen()
    {
      ClearScreen();
      userDataUI.SetActive(true);
    }
}