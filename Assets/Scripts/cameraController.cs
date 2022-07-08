using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class cameraController : MonoBehaviour
  {

  public bool vrHeadsetAttached = false;
  public GameObject desktopCamera;
  public GameObject xrObject;
  public GameObject player;
  bool VrIsOn;
  // Start is called before the first frame update
  void Start()
    {
    var xrSettings = XRGeneralSettings.Instance;
    if (xrSettings == null)
      {
      return;
      }
    var xrManager = xrSettings.Manager;
    if (xrManager == null)
      {
      return;
      }
    var xrLoader = xrManager.activeLoader;
    if (xrLoader == null)
      {
      xrObject.SetActive(false);
      desktopCamera.SetActive(true);
      return;
      }
    xrObject.SetActive(true);
    VrIsOn = true;
    desktopCamera.SetActive(false);
    xrObject.transform.localPosition = new Vector3(0, 0, 0) + new Vector3(0, 0.4f, 0.55f);
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

