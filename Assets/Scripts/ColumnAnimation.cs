using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Audio;

public class ColumnAnimation : NetworkBehaviour
{
    private Vector3 endPosition;
    private float maxMoveDistatnce = 75;
    private AudioSource audioSource;
    [SerializeField] private AudioClip jackHammer;
    [SerializeField] private AudioClip hammer;
    [SerializeField] private AudioClip hammer2;
    [SerializeField] private AudioClip saw;
    [SerializeField] private GameObject DustCloud;

    [SyncVar]
    public bool wasSpawned = false;

    private GameObject dustCloud;


    public void Start()
    {
        if (wasSpawned)
        {
            endPosition = transform.position + PlayerController.animationOffset;
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlaySoundEffect());
            dustCloud = Instantiate(DustCloud, endPosition, Quaternion.identity);
            if (isServer)
            {
                StartCoroutine(Animation());
            }
        }
    }

    IEnumerator Animation()
    {
        while (this.transform.position != endPosition)
        {
            yield return new WaitForSeconds(0.05f);
            this.transform.position = Vector3.MoveTowards(this.transform.position, endPosition, maxMoveDistatnce * Time.deltaTime);
        }
        wasSpawned = false;
    }

    IEnumerator PlaySoundEffect()
    {
        audioSource.PlayOneShot(jackHammer);
        yield return new WaitForSeconds(0.25f);
        audioSource.PlayOneShot(hammer);
        audioSource.PlayOneShot(hammer2);
        audioSource.PlayOneShot(saw);
        
        
    }
}
