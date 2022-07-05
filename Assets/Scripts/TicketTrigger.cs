using UnityEngine;

public class TicketTrigger : MonoBehaviour
{
    [SerializeField] private Canvas screenSpaceCamera;
    private bool wasEPressed = false;
    private bool wasEscPressed = false;
    private bool shouldGetKeyPresses;
    private bool beingEdited;

    private void Update()
    {
        if (shouldGetKeyPresses)
        {
            GetKeyPresses();
        }
    }

    private void GetKeyPresses()
    {
        // makes sure that when you click the code 
        Debug.Log("Getting key presses");
        wasEPressed = wasEPressed || Input.GetKeyDown(KeyCode.E);
        wasEscPressed = wasEscPressed || Input.GetKeyDown(KeyCode.Tab);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Local Player"))
        {
            return;
        }
        if (wasEPressed)
        {
            other.gameObject.GetComponent<PlayerController>().isEditing = true;
            Cursor.lockState = CursorLockMode.None;
            screenSpaceCamera.gameObject.SetActive(true);
            wasEPressed = false;
            beingEdited = true;
        }
        if (wasEscPressed && beingEdited)
        {
            Cursor.lockState = CursorLockMode.Locked;
            screenSpaceCamera.gameObject.SetActive(false);
            other.gameObject.GetComponent<PlayerController>().isEditing = false;
            wasEscPressed = false;
            beingEdited = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Local Player"))
        {
            shouldGetKeyPresses = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Local Player"))
        {
            shouldGetKeyPresses = false;
        }
    }
}
