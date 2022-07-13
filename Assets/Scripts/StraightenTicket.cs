using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StraightenTicket : NetworkBehaviour
{
    private Rigidbody ticketRigidbody;

    private void Start()
    {
        if (isClientOnly)
        {
            Destroy(this.GetComponent<StraightenTicket>());
        }
        ticketRigidbody = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Straighten Ticket"))
        {
            Transform spawnTransform = other.GetComponentInChildren<Transform>();
            this.gameObject.transform.position = spawnTransform.position;
            this.gameObject.transform.rotation = spawnTransform.rotation;
            this.gameObject.transform.Rotate(-90, 0, 0);
            ticketRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            this.GetComponent<Animator>().Play("Ticket_Grow");
        }
    }
}
