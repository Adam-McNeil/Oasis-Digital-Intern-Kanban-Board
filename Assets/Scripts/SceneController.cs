using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    static private int numberOfTimesSceneIsLoaded;
    public FirebaseManager firebaseManager;
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject serverSelectionMenu;

    private void Start()
    {
        numberOfTimesSceneIsLoaded++;
        firebaseManager = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();

        if (numberOfTimesSceneIsLoaded > 1)
        {
            firebaseManager.FindUIRefences();
            GameObject.Find("Firebase-Carry-Over").GetComponent<FirebaseCarryOver>().FindUIRefences();
            loginMenu.SetActive(false);
            serverSelectionMenu.SetActive(true);
        }
    }

    public void FirebaseLoginButton()
    {
        firebaseManager.LoginButton();
    }

    public void FirebaseGetServerData()
    {
        firebaseManager.GetServerData();
    }

    public void FirebaseRegisterButton()
    {
        firebaseManager.RegisterButton();
    }


}
