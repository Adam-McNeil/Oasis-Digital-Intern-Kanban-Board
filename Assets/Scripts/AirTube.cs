using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTube : MonoBehaviour
{

    private List<GameObject> objectsOnConveyor = new List<GameObject>(); //Stores a list of gameobjects that are on the conveyor belt
    private GameObject movingObject;                                     //Objects that will be stored in the list to be moved  
    private Rigidbody movingObjectRB;

    public Vector3 direction;                                            //Direction where the object is pushed 
    public float speed;                                                  //Speed for the movement of the object 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        for(int i = 0; i <= objectsOnConveyor.Count -1; i++)
        {
            objectsOnConveyor[i].transform.position += direction * speed * Time.deltaTime;
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
        movingObjectRB = other.gameObject.GetComponent<Rigidbody>();
        movingObjectRB.useGravity = false;
        movingObjectRB.constraints = RigidbodyConstraints.FreezeAll;
    }


    private void OnTriggerExit(Collider other) 
    {
        movingObjectRB.constraints = RigidbodyConstraints.None;
        movingObjectRB.useGravity = true;
        objectsOnConveyor.Remove(other.gameObject);
    }
}
