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

    private void Update()
    {
        if(isActive){
            for(int i = 0; i <= objectsOnConveyor.Count -1; i++)
            {
                objectsOnConveyor[i].GetComponent<Rigidbody>().AddForce(transform.up * speed * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        if (otherRB != null)
        {
            objectsOnConveyor.Add(otherRB);
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
}
