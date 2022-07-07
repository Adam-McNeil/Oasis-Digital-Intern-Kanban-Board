using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class cameraController : MonoBehaviour
  {

  public bool vrHeadsetAttached = false;
  public GameObject desktopCamera;
  public GameObject xrObject;
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
    desktopCamera.SetActive(false);
    }
  }
