using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class getCurrentUsers : MonoBehaviour
{

    static public List<string> users = new List<string>();
    static public List<string> usersOld = new List<string>();

    private FirebaseManager firebaseManager;
    private TMP_Dropdown dropDown;

    private void Start()
    {
        firebaseManager = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        dropDown = this.GetComponent<TMP_Dropdown>();
        UpdateDropDown();
    }

    public void UpdateDropDown()
    {
        dropDown.ClearOptions();
        users.Clear();
        users.Add("Nobody");
        foreach (string user in firebaseManager.userList)
        {
            Debug.Log("User Added: " + user);
            users.Add(user);
        }
        dropDown.AddOptions(users);
    }

}