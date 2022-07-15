using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirTube : MonoBehaviour
{
    private List<GameObject> objectsOnConveyor = new List<GameObject>(); //Stores a list of gameobjects that are on the conveyor belt
    private GameObject movingObject;                                     //Objects that will be stored in the list to be moved  
    private Rigidbody movingObjectRB;

    public bool isActive = true; 
    public Vector3 direction;                                            //Direction where the object is pushed 
    public float speed;                                                  //Speed for the movement of the object 

    void Start()
    {

    }

    private void Update()
    {
        if(isActive){
            for(int i = 0; i <= objectsOnConveyor.Count -1; i++)
            {
                if (objectsOnConveyor[i] != null)
                {
                    objectsOnConveyor[i].transform.position += transform.up * speed * Time.deltaTime;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Ticket Trigger"))
        {
            return;
        }
        objectsOnConveyor.Add(other.gameObject);
        
    }

    private void OnTriggerStay(Collider other) {
        if(isActive && other.gameObject.CompareTag("Ticket"))
        {
            movingObjectRB = other.gameObject.GetComponent<Rigidbody>();
            movingObjectRB.useGravity = false;
            movingObjectRB.constraints = RigidbodyConstraints.FreezeAll;
        }
    }


    private void OnTriggerExit(Collider other) 
    {
        if(isActive && other.gameObject.CompareTag("Ticket")){
            movingObjectRB.constraints = RigidbodyConstraints.None;
            movingObjectRB.useGravity = true; 
        }
        objectsOnConveyor.Remove(other.gameObject);
    }
}
