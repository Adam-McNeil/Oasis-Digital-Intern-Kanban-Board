using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ActivateAirTube : NetworkBehaviour
{

    public GameObject airPad;
    public AirTube airTubeScript;
    private float timer;

    void Start()
    {
        airTubeScript = airPad.GetComponent<AirTube>();
    }

    void Update()
    {
        if(airTubeScript.isActive){
            timer += Time.deltaTime;

            if(timer > 2){
                airTubeScript.isActive = false; 
                timer = 0;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void TurnOnAirTubeCmd()
    {
        airTubeScript.isActive = true;
    }
}
