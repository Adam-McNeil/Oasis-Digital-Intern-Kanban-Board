using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.SimpleWeb;
using TMPro;

public class NetworkManagerController : MonoBehaviour
{
    private NetworkManager networkManager;
    private SimpleWebTransport networkTransport;
    [SerializeField] private TMP_InputField serverInputField;
    [SerializeField] private TMP_InputField portInputField;

    private void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        networkTransport = GetComponent<SimpleWebTransport>();
    }

    public void SetServerAndPort()
    {
        networkManager.networkAddress = serverInputField.text;
        networkTransport.port = System.UInt16.Parse(portInputField.text);
    }
}