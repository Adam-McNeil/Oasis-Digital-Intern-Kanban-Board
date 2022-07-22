using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirebaseCarryOver : MonoBehaviour
{

    public TMP_InputField serverName;
    public static string serverNameText;

    // Start is called before the first frame update
    public void Start()
    {
        DontDestroyOnLoad(this);
        serverName.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        serverNameText = serverName.text;
        Debug.Log(serverNameText);
    }
}
