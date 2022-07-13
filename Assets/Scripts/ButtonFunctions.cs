using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
