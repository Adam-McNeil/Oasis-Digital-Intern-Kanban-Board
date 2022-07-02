using UnityEngine;

public class TicketController : MonoBehaviour
{
    [SerializeField] private Canvas screenSpaceCamera;

    void OnCollisionEnter(Collision collision)
    {
        
    }

    void OnCollisionExit(Collision collision)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("checking for collsion");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("enter player");
            screenSpaceCamera.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("enter player");
            screenSpaceCamera.gameObject.SetActive(false);
        }
    }
}
