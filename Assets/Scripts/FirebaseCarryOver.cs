using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirebaseCarryOver : MonoBehaviour
{

    public static string serverNameText;
    static private int firebaseCarryOverCount;
    private UIReferences uIReferences;

    // Start is called before the first frame update
    public void Awake()
    {
        if (firebaseCarryOverCount != 0)
        {
            Destroy(this.gameObject);
        }
        FindUIRefences();
        firebaseCarryOverCount++;
        DontDestroyOnLoad(this);
    }

    public void FindUIRefences()
    {
        uIReferences = GameObject.Find("UI Refence").GetComponent<UIReferences>();
        uIReferences.serverInput.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        serverNameText = uIReferences.serverInput.text;
        Debug.Log(serverNameText);
    }
}
