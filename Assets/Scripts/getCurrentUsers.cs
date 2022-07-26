using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class getCurrentUsers : MonoBehaviour
{

    public List<string> users = new List<string>();
    public List<string> usersOld = new List<string>();
    public List<string> noBody = new List<string>();
    private FirebaseManager firebaseManager;

    private void Start()
    {
        firebaseManager = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        noBody.Add("Not Assigned");
    }

    private void Update()
    {
        users = firebaseManager.userList;
        if (users != usersOld)
        {
            this.GetComponent<TMP_Dropdown>().ClearOptions();
            this.GetComponent<TMP_Dropdown>().AddOptions(noBody);
            this.GetComponent<TMP_Dropdown>().AddOptions(users);
            usersOld = users;
        }
    }
}

