using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAirTube : MonoBehaviour
{

    public GameObject airPad;
    private AirTube airTubeScript;
    private bool canInteract;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        canInteract = false;
        airTubeScript = airPad.GetComponent<AirTube>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canInteract && Input.GetMouseButtonDown(0) && !airTubeScript.isActive)
        {
            airTubeScript.isActive = true;
        }


        if(airTubeScript.isActive){
            timer += Time.deltaTime;
            Debug.Log(timer);

            if(timer > 2){
                Debug.Log("Turned off");
                airTubeScript.isActive = false; 
                timer = 0;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        canInteract = true;
    }

    private void OnTriggerExit(Collider other) 
    {
        canInteract = false;
    }

}
