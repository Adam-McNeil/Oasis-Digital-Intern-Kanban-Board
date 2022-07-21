using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyTicketController : NetworkBehaviour
{
    [Server]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ticket"))
        {
            Debug.Log("destroying ticekt");
            NetworkServer.Destroy(collision.gameObject);
        }
    }
}
