using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTickets : MonoBehaviour
{
    public GameObject[] tickets;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(.1f));
    }
 
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        tickets = GameObject.FindGameObjectsWithTag("Ticket");
        Debug.Log("(AllTickets)This was Ran");
    }


}
