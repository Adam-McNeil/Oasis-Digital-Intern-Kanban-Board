using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSync : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private TMP_Text myText;

    private void Start()
    {
        myText = GetComponent<TMP_Text>();
        //inputField.onValueChanged.AddListener(delegate { ChangeText(); });
    }

    private void ChangeText()
    {
        myText.text = inputField.text;
    }
}
