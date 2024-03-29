using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    static public GameObject localPlayerCamera;
    public float speed = 7.5f;
    public float jumpSpeed = 12f;
    public float gravity = 9.81f;
    public float lookXLimit = 45.0f;
    Vector3 moveDirection = Vector3.zero;
    public float rotationX = 0;
    public float rotationY = 0;
    [SerializeField]private float sensitivity = 30;

    private CharacterController characterController;

    static public bool isGamePaused;
    [HideInInspector] static public bool isEditing;

    private DescriptionMenuController myDescriptionMenu;

    [Header("Children Refences")]
    [SerializeField] private Transform cameraOffset;
    [SerializeField] private GameObject desktopCamera;
    [SerializeField] private TextMeshProUGUI usernameText;

    private TMP_InputField usernameInputField;
    [SyncVar(hook = nameof(ChangeUsername))]
    private string usernameSyncVar;

    [Header("Column Placement")]
    [SerializeField] private GameObject blueprint;
    [SerializeField] private GameObject column;
    [SerializeField] private LayerMask raycastLayerMask;
    private float rotationSnap = 22.5f;
    private GameObject myBlueprint;
    public bool isBuilding = false;
    private Vector3 farAway;

    [Header("Button Presses")]
    [SerializeField] private LayerMask buttonPressLayerMask;

    static public bool isEditingColumn;

    public AudioSource footStepsAudioSource;
    public AudioSource gruntsAudioSource;
    public AudioSource impactAudioSource;

    private float gridSize = 5f;

    private void Start()
    {
        footStepsAudioSource = this.gameObject.GetComponent<AudioSource>();
        farAway = new Vector3(10000, 10000, 0);
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        if (isLocalPlayer)
        {
            Destroy(usernameText);
            isGamePaused = false;
            gameObject.tag = "Local Player";
            FindInputField();
            SpawnBlueprintCmd(farAway, blueprint.transform.rotation);
            FindMyTicketDescriptionMenu();
        }
        else
        {
            gameObject.tag = "Nonlocal Player";
        }
    }

    private void Update()
    {
        if(PlayerPrefs.HasKey("Sensitivity")){
            sensitivity= PlayerPrefs.GetFloat("Sensitivity");
        }

        if (isLocalPlayer)
        {
            Escape();
            EditMode();
            if (!isGamePaused && !isEditing)
            {
                RotateCamera();
                if (!isEditingColumn)
                {
                    playerMovement();
                    PlayerRotation();
                    CheckButtonPresses();
                    PlaceColumnCheck();
                    CheckTicketHits();
                }
            }
        }
    }

    #region Pause
    private void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isEditing && characterController.isGrounded)
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }            
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        isGamePaused = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isGamePaused = false;
    }
    #endregion

    #region Movement
    private void playerMovement()
    {
        
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX =  speed * Input.GetAxis("Vertical");
        float curSpeedY =  speed * Input.GetAxis("Horizontal");
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            gruntsAudioSource.Play();
            moveDirection.y = jumpSpeed;
        }
        else
        { 
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
       
        if(characterController.isGrounded && (curSpeedX < 0 || curSpeedX > 0 || curSpeedY < 0 || curSpeedY > 0))
        {
            if(!footStepsAudioSource.isPlaying){
                footStepsAudioSource.Play();
            }
        }
        else
        {
            footStepsAudioSource.Stop();
        }
    }

    private void PlayerRotation()
    {
        if (Input.GetKey(KeyCode.LeftBracket)) {
            transform.Rotate(0, -(speed * Time.deltaTime * 5), 0);
        }
        if (Input.GetKey(KeyCode.RightBracket)) {
            transform.Rotate(0, speed * Time.deltaTime * 5, 0);
        }
    }

    void RotateCamera()
    {
          rotationX += -Input.GetAxis("Mouse Y") * sensitivity;
          rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
          desktopCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
          transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }

    #endregion

    #region Username

    private void FindInputField()
    {
        usernameInputField = GameObject.Find("Settings/Pause Menu").GetComponentInChildren<TMP_InputField>();
        usernameInputField.onValueChanged.AddListener(delegate { OnChangedInputField(); });
    }

    private void OnChangedInputField()
    {
        ChangeUsernameCmd(usernameInputField.text);
    }

    [Command(requiresAuthority = false)]
    private void ChangeUsernameCmd(string text)
    {
        usernameSyncVar = text;
    }

    private void ChangeUsername(string oldText, string newText)
    {
        usernameText.text = newText;
    }

    #endregion

    #region EditMode

    private void EditMode(){
        if (Input.GetKeyDown(KeyCode.E) && !isEditing)
        {
            RaycastHit hit;

            if(Physics.Raycast(desktopCamera.transform.position, desktopCamera.transform.forward, out hit, 6))
            {
                if (hit.transform.gameObject.CompareTag("Ticket"))
                {
                    Debug.Log("Edit table hit");
                    EditTable.instance.OnStartEdit(hit.transform.gameObject);
                    isEditing = true;
                    Cursor.lockState = CursorLockMode.None;
                    Debug.Log("Edit Mode enabled");
                }
            }
        }else if(Input.GetKeyDown(KeyCode.Escape) && isEditing){
            isEditing = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    #endregion

    #region PressButtons

    private void CheckButtonPresses()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(desktopCamera.transform.position, desktopCamera.transform.forward, out hit, 6, buttonPressLayerMask))
            {
                if (hit.transform.gameObject.CompareTag("Spawn Button"))
                {
                    hit.transform.gameObject.GetComponent<SpawnTickets>().SpawnTicketCmd(); 
                    hit.transform.gameObject.GetComponent<Animator>().Play("buttonPress");
                }

                if (hit.transform.gameObject.CompareTag("Activation Button"))
                {
                    hit.transform.gameObject.GetComponent<ActivateAirTube>().TurnOnAirTubeCmd();
                }

                if (hit.transform.gameObject.CompareTag("Column Title"))
                {
                    hit.transform.gameObject.GetComponentInParent<EditColumn>().SelectInputField();
                }
            }
        }
    }

    #endregion

    #region PlaceColumns

    private Vector3 blueprintOffset = new Vector3(0.75f, 0.75f, 0);
    private BlueprintColliderCounter blueprintColliderCounter;
    static public Vector3 animationOffset = new Vector3(0, 20, 0);

    private void PlaceColumnCheck()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
        }
        //Debug.Log(isBuilding);
        PlaceColumn();
    }

    private void PlaceColumn()
    {
        if (myBlueprint == null)
        {
            FindMyBlueprint();
        }
        else
        {
            if (isBuilding)
            {
                RaycastHit hit;
                if (Physics.Raycast(desktopCamera.transform.position, desktopCamera.transform.forward, out hit, 100, raycastLayerMask))
                {
                    //Debug.Log("Hit the floor");
                    myBlueprint.transform.position = SnapVector(hit.point) + blueprintOffset;
                }
                if (Input.GetKeyDown("q"))
                {
                    myBlueprint.transform.Rotate(Vector3.up * rotationSnap);
                }
                if (Input.GetKeyDown("e"))
                {
                    myBlueprint.transform.Rotate(Vector3.down * rotationSnap);
                }
                if (blueprintColliderCounter.canPlaceColumn && Input.GetKeyDown("f"))
                {
                    SpawnColumnCmd(myBlueprint.transform.position - animationOffset, myBlueprint.transform.rotation);
                }
            }
            else
            {
                myBlueprint.transform.position = farAway;
            }
        }
    }

    private Vector3 SnapVector(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x / gridSize) * gridSize,
            Mathf.Round(vector.y / gridSize) * gridSize,
            Mathf.Round(vector.z / gridSize) * gridSize);
    }

    [Command(requiresAuthority = false)]
    private void SpawnColumnCmd(Vector3 spawnLocation, Quaternion spawnRotation)
    {
        GameObject SpawnedObject = Instantiate(column, spawnLocation, spawnRotation);
        SpawnedObject.GetComponent<ColumnAnimation>().wasSpawned = true;
        NetworkServer.Spawn(SpawnedObject);
    }

    [Command(requiresAuthority = false)]
    private void SpawnBlueprintCmd(Vector3 spawnLocation, Quaternion spawnRotation)
    {
        GameObject spawnBlueprint = Instantiate(blueprint, spawnLocation, spawnRotation);
        NetworkServer.Spawn(spawnBlueprint);
        spawnBlueprint.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }



    private void FindMyBlueprint()
    {
        GameObject[] blueprintArray = GameObject.FindGameObjectsWithTag("Column Blueprint");
        foreach (GameObject blueprint in blueprintArray)
        {
            if (blueprint.GetComponent<NetworkIdentity>().hasAuthority)
            {
                myBlueprint = blueprint;
                blueprintColliderCounter = myBlueprint.GetComponent<BlueprintColliderCounter>();
                //Debug.Log("Found my Blueprint");
                return;
            }
        }
    }

    #endregion

    #region TicketDescription

    private void CheckTicketHits()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit hit;

            if (Physics.Raycast(desktopCamera.transform.position, desktopCamera.transform.forward, out hit, 100, buttonPressLayerMask))
            {
                if (hit.transform.gameObject.CompareTag("Ticket"))
                {
                    myDescriptionMenu.DisplayTicket(hit.transform.gameObject);
                    return;
                }
            }
            myDescriptionMenu.MoveFarAway();
        }
    }

    private void FindMyTicketDescriptionMenu()
    {
        myDescriptionMenu = GameObject.FindGameObjectWithTag("Description Menu").GetComponent<DescriptionMenuController>();
        Debug.Log("Found my Description Menu");
    }

    #endregion
}
