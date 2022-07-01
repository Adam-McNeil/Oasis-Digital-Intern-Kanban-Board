using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{

    public float speed;
    public Vector3 direction;
    private GameObject movingObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if(movingObject != null)
        {
            movingObject.transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Ticket"){
            movingObject = other.gameObject;
        }
        
    }
    private void OnTriggerExit(Collider other) 
    {
        movingObject = null;
    }
}
