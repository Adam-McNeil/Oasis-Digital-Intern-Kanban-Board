using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMenuController : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private Vector3 farAway;
    [SerializeField] public TMP_Dropdown users;

    private AllTickets allTicketsScript;
    private PlayerController playerControllerScript;
    private bool flag = false;

    private GameObject activeCamera;

    private void Update()
    {
        flag = PlayerController.isCheckingPlayer;
        
        if(flag)
        {
            Cursor.lockState = CursorLockMode.None;
            Transform localPlayerTransform = activeCamera.transform;
            this.transform.position = localPlayerTransform.position + localPlayerTransform.forward * offset;
            this.transform.LookAt(localPlayerTransform);
            this.transform.Rotate(0, 180, 0);
        }
        else
        {
            this.transform.position = farAway;
        }
    }

    public void SetActiveCamera(GameObject camera)
    {
        this.transform.position = farAway;
        activeCamera = camera;
        GetComponentInChildren<Canvas>().worldCamera = activeCamera.GetComponent<Camera>();
    }

    public void FindTickets()
    {
        int index = users.value;
        Debug.Log(users.value);
        users.GetComponent<AllTickets>().GetCertainTickets(users.value);
    }
}
