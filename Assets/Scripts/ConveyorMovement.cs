using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{

    public List<GameObject> objectsOnConveyor = new List<GameObject>();
    public float speed;
    public Vector3 direction;
    private GameObject movingObject;

    private void FixedUpdate() 
    {
        for(int i = 0; i <= objectsOnConveyor.Count -1; i++)
        {
            objectsOnConveyor[i].transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Ticket")
        {
            objectsOnConveyor.Add(other.gameObject);
        }
        
    }
    private void OnTriggerExit(Collider other) 
    {
        objectsOnConveyor.Remove(other.gameObject);
    }
}
