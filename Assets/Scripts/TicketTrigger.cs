using UnityEngine;
using Mirror;

public class TicketTrigger : MonoBehaviour
{
    [SerializeField] private Canvas screenSpaceCamera;
    private TicketController ticketController;
    private bool wasEPressed = false;
    private bool shouldGetKeyPresses;
    [HideInInspector]
    public bool beingEdited;
    static private GameObject editingPlayer = null;

    private void Start()
    {
        ticketController = GetComponentInParent<TicketController>();
    }

    private void Update()
    {
        if (shouldGetKeyPresses && editingPlayer == null)
        {
            // makes sure that when you click the code in OnTriggerStay is run
            wasEPressed = wasEPressed || Input.GetKeyDown(KeyCode.E);
        }
    }

    private void LateUpdate()
    {
        if (beingEdited && Input.GetKeyDown(KeyCode.Tab))
        {
            ExitEditMode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Local Player"))
        {
            return;
        }
        if (wasEPressed && !other.gameObject.GetComponent<PlayerController>().isEditing)
        {
            EnterEditMode(other.gameObject);
        }
        wasEPressed = false;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Local Player"))
        {
            shouldGetKeyPresses = true;
            wasEPressed = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Local Player"))
        {
            shouldGetKeyPresses = false;
            wasEPressed = false;
        }
    }

    private void EnterEditMode(GameObject player)
    {
        editingPlayer = player;
        ticketController.SetShouldDoMovement(true);
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerController>().isEditing = true;
        beingEdited = true;
        screenSpaceCamera.gameObject.SetActive(true);
        wasEPressed = false;
    }

    public void ExitEditMode()
    {
        ticketController.SetShouldDoMovement(false);
        Cursor.lockState = CursorLockMode.Locked;
        editingPlayer.GetComponent<PlayerController>().isEditing = false;
        editingPlayer = null;
        beingEdited = false;
        screenSpaceCamera.gameObject.SetActive(false);
    }
}
