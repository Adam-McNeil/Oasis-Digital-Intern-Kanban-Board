using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class getCurrentUsers : MonoBehaviour
{

    public List<string> users = new List<string>();
    public List<string> usersOld = new List<string>();

  private void Update()
    {
      users = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().userList;
      if (users != usersOld)
      {
        this.GetComponent<TMP_Dropdown>().ClearOptions();
        this.GetComponent<TMP_Dropdown>().AddOptions(users);
        usersOld = users;
      }
    }
  }
