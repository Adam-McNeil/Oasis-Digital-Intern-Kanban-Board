using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyTicketController : NetworkBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip destroyClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [Server]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ticket"))
        {
            Debug.Log("destroying ticekt");
            PlaySoundRpc();
            NetworkServer.Destroy(collision.gameObject);
        }
    }

    [ClientRpc]
    private void PlaySoundRpc()
    {
        PlaySound();
    }

    private void PlaySound()
    {
       audioSource.PlayOneShot(destroyClip);
    }
}
