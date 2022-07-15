using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GoToHoldingPlayer : NetworkBehaviour
{
    [SerializeField] private Transform goToHoldingPlayerTransform;
    private GameObject playerHoldingHammer = null;

    void Update()
    {
        if (playerHoldingHammer == null)
        {
            return;
        }
        else
        {
            goToHoldingPlayerTransform.position = playerHoldingHammer.transform.position;
            goToHoldingPlayerTransform.rotation = playerHoldingHammer.transform.rotation;
        }
    }

    [Command(requiresAuthority = false)]
    public void SetPlayerHoldingHammerCmd(uint ID)
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Nonlocal Player");
        foreach (GameObject player in playerList)
        {
            if (player.GetComponent<NetworkIdentity>().netId == ID)
            {
                playerHoldingHammer = player;
                return;
            }
        }
        playerHoldingHammer = GameObject.FindGameObjectWithTag("Local Player").GetComponentInChildren<Camera>().gameObject;
    }
}