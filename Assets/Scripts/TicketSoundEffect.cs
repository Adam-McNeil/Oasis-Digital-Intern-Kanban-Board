using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TicketSoundEffect : NetworkBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip pickUpSoundEffect;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [ClientRpc]
    public  void PlaySoundEffectRpc()
    {
        PlaySoundEffect();
    }

    private void PlaySoundEffect()
    {
        audioSource.PlayOneShot(pickUpSoundEffect);
    }
}
