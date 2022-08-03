using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonFunctions : MonoBehaviour
{
    public void Exit()
    {

        Debug.Log("Game Exited");
        Application.Quit();
    }

    public void ExitServer()
    {
        Mirror.NetworkManager networkMangaer = GameObject.Find("Network Manager").GetComponent<Mirror.NetworkManager>();
        networkMangaer.StopClient();
        networkMangaer.StopHost();
    }

    public void SaveSetting(){
        PlayerPrefs.SetFloat("Sensitivity",SliderToText.senValue);
        PlayerPrefs.SetInt("Volume",SliderToText.volValue);  
        PlayerPrefs.Save();
    }

    public void RefreshButton()
    {
        Debug.Log("button was pressed");
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().addUserToServerCall();
    }

    public void SignOut()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().SignOut();
    }
}
