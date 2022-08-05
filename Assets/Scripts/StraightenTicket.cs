using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StraightenTicket : NetworkBehaviour
{
    private Rigidbody ticketRigidbody;
    private Vector3 largeScale = new Vector3(4.5f, .5f, 2);
    private Vector3 smallScale = new Vector3(1, 1, 1);
    private float slerpSpeed = 0.1f;
    public bool shouldSlerp = false;



    private void Start()
    {
        if (isClientOnly)
        {
            Destroy(this.GetComponent<StraightenTicket>());
        }

        ticketRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (shouldSlerp)
        {
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, smallScale, slerpSpeed);
            shouldSlerp = !(this.transform.localScale == smallScale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Straighten Ticket"))
        {
            Debug.Log("entered trigger");
            this.transform.localScale = largeScale;
            shouldSlerp = false;
            Transform spawnTransform = other.GetComponentInChildren<Transform>();
            this.gameObject.transform.position = spawnTransform.position;
            this.gameObject.transform.rotation = spawnTransform.rotation;
            this.gameObject.transform.Rotate(-90, 0, 0);
            ticketRigidbody.velocity = Vector3.zero;
            ticketRigidbody.angularVelocity = Vector3.zero;
            ticketRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        }
    }
}
