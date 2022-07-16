using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
using Mirror;

public class CustomCharacterCamera : NetworkBehaviour
{
    [SerializeField] private PickUp pickUpScript;
    [SerializeField] private FollowCamera followCameraScript;
    private PauseMenuController pauseMenuControllerScript;
    private GameObject activeCamera;
    public bool vrHeadsetAttached = false;
    public GameObject desktopCamera;
    public GameObject xrObject;
    public GameObject player;
    bool VrIsOn;

    void Start()
    {
        if (isLocalPlayer)
        {

            pauseMenuControllerScript = GameObject.Find("Pause Menu").GetComponent<PauseMenuController>();
            activeCamera = desktopCamera;
            var xrSettings = XRGeneralSettings.Instance;
            if (xrSettings != null)
            {
                var xrManager = xrSettings.Manager;
                if (xrManager != null)
                {
                    var xrLoader = xrManager.activeLoader;
                    if (xrLoader != null)
                    {
                        xrObject.SetActive(true);
                        activeCamera = xrObject;
                        VrIsOn = true;
                        desktopCamera.SetActive(false);
                        xrObject.transform.localPosition = new Vector3(0, 0, 0) + new Vector3(0, 0.4f, 0.55f);
                        UpdateOtherScriptsCamera();

                            return;
                    }
                }
            }
            VrIsOn = false;
            xrObject.SetActive(false);
            desktopCamera.SetActive(true);
            UpdateOtherScriptsCamera();
        }
    }

    private void UpdateOtherScriptsCamera()
    {
        pickUpScript.SetActiveCamera(activeCamera);
        pauseMenuControllerScript.SetActiveCamera(activeCamera);
        followCameraScript.SetActiveCamera(activeCamera);
    }

    private void Update()
    {
        if (VrIsOn)
        {
            player.transform.rotation = xrObject.transform.rotation;
            xrObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
