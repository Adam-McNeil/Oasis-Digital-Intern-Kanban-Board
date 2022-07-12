using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class XrReference
{
  public static GameObject XrOrigin;
  public static GameObject XrCamera;
  
  public static GameObject XrRig
    {
    get
      {
        if (XrOrigin == null)
        {
          XrOrigin = GameObject.FindGameObjectWithTag("XrOrigin");
        }
        return XrOrigin;
      }
    }
  public static GameObject Camera
    {
    get
      {
        if (XrCamera == null)
        {
          XrCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        return XrCamera;
      }
    }

}
