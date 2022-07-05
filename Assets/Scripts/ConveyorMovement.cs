using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{
    private List<GameObject> objectsOnConveyor = new List<GameObject>(); //Stores a list of gameobjects that are on the conveyor belt
    private GameObject movingObject;                                     //Objects that will be stored in the list to be moved  

    public Vector3 direction;                                            //Direction where the object is pushed 
    public float speed;                                                  //Speed for the movement of the object 

    private void Update()
    {
        for(int i = 0; i <= objectsOnConveyor.Count -1; i++)
        {
            objectsOnConveyor[i].transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        objectsOnConveyor.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other) 
    {
        objectsOnConveyor.Remove(other.gameObject);
    }
}
