using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class attatchToPLayer : NetworkBehaviour
  {
  private GameObject player = null;
  private GameObject playerCamera = null;

  // Update is called once per frame
  void Update()
    {
    if (playerCamera == null)
      {
      player = GameObject.FindGameObjectWithTag("Local Player");
      if (player != null)
        {
        playerCamera = GameObject.FindGameObjectWithTag("playerCamera");
        }
      }
    else
      {
      transform.position = playerCamera.transform.position;
      transform.rotation = playerCamera.transform.rotation;
      }
    }
  }
