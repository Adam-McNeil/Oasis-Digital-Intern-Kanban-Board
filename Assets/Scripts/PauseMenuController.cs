using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private Vector3 farAway;
    [SerializeField] private TMP_InputField usernameInputField;
    private GameObject activeCamera;
    private bool oldGamePaused = false;
    private bool isGamePaused;

    private void Update()
    {
        isGamePaused = PlayerController.isGamePaused;
        if (isGamePaused != oldGamePaused)
        {
            if (isGamePaused)
            {
                Transform localPlayerTransform = activeCamera.transform;
                usernameInputField.enabled = true;
                this.transform.position = localPlayerTransform.position + localPlayerTransform.forward * offset;
                this.transform.LookAt(localPlayerTransform);
                this.transform.Rotate(0, 180, 0);
            }
            else
            {
                usernameInputField.enabled = false;
                this.transform.position = farAway;
            }
        }
        oldGamePaused = isGamePaused;
    }

    public void SetActiveCamera(GameObject camera)
    {
        this.transform.position = farAway;
        activeCamera = camera;
        GetComponentInChildren<Canvas>().worldCamera = activeCamera.GetComponent<Camera>();
    }
}
