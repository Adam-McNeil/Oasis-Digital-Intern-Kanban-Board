using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject playerCamera;
    public void SetActiveCamera(GameObject camera)
    {
        playerCamera = camera;
    }

    private void Update()
    {
        if (playerCamera == null)
        {
            return;
        }
        this.transform.position = playerCamera.transform.position;
        this.transform.rotation = playerCamera.transform.rotation;
    }
}
