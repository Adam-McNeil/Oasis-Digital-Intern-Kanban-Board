using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirTube : MonoBehaviour
{
    private List<Rigidbody> objectsOnConveyor = new List<Rigidbody>(); //Stores a list of gameobjects that are on the conveyor belt
   
    public bool isActive = true; 
    public Vector3 direction;                                            //Direction where the object is pushed 
    public float speed;                                                  //Speed for the movement of the object 
    private AudioSource audioSource;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private ParticleSystem movementParticle;

    public void Start() {
        if (isActive) audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        for(int i = 0; i < objectsOnConveyor.Count; i++)
        {
            objectsOnConveyor[i].GetComponent<Rigidbody>().AddForce(transform.up * speed * Time.deltaTime, ForceMode.Impulse);
            //Destroy(Instantiate(movementParticle, objectsOnConveyor[i].transform.position, movementParticle.transform.rotation), 1);
        }

    }

    private void OnTriggerEnter(Collider other) 
    {
        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        if (otherRB != null)
        {
            objectsOnConveyor.Add(otherRB);
            StartCoroutine(PlaySoundEffect());
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        if (otherRB != null)
        {
            objectsOnConveyor.Remove(otherRB);
        }
    }

    IEnumerator PlaySoundEffect()
    {
        try {
            audioSource.PlayOneShot(moveSound); 
        } catch (Exception e) { }
        yield return new WaitForSeconds(0f);
    }
}
