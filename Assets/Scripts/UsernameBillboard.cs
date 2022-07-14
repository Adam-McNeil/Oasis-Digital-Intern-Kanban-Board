using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameBillboard : MonoBehaviour
{
    private GameObject localPlayer;

    private void Update()
    {
        if (localPlayer == null)
        {
            localPlayer = GameObject.FindGameObjectWithTag("Local Player");

        }
        else
        {
            this.transform.LookAt(localPlayer.transform.position);
            this.transform.Rotate(0, 180, 0);
        }
    }
}
