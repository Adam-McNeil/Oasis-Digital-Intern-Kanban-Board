using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColumnPlace : NetworkBehaviour
  {

  [SerializeField] private int placeRange = 500;
  public static bool isBeingHeld = false;
  [SerializeField] private GameObject prefab;
  [SerializeField] private GameObject prefabBluePrint;
  [SerializeField] private GameObject playerCopyRotation;
  GameObject blueprint;

  [SerializeField] private LayerMask raycastLayerMask;
  [SerializeField] private float rotationSpeed = 1;


  private void Start()
    {
        blueprint = Instantiate(prefabBluePrint);
        blueprint.SetActive(false);
    }
  // Update is called once per frame
  void Update()
    {
   // if (isLocalPlayer)
      //{
      if (GetComponent<Rigidbody>().useGravity == false)
        {
        blueprint.SetActive(true);
        RaycastHit hit;
        Debug.DrawRay(playerCopyRotation.transform.position, playerCopyRotation.transform.forward * 10, new Color(0, 0, 0), 3f);
        if (Physics.Raycast(playerCopyRotation.transform.position, playerCopyRotation.transform.forward, out hit, placeRange, raycastLayerMask))
          {
          if (hit.transform.gameObject.tag == "floor")
            {
            blueprint.transform.position = hit.point;
            if (Input.GetKey("q"))
              {
              blueprint.transform.Rotate(Vector3.up * rotationSpeed);
              }
            if (Input.GetKey("e"))
              {
              blueprint.transform.Rotate(Vector3.down * rotationSpeed);
              }
            if (Input.GetKeyDown("f"))
              {
              blueprint.SetActive(false);
              spawnPrefabCmd(new Vector3(hit.point.x + .75f, .75f, hit.point.z), blueprint.transform.rotation);
              blueprint.SetActive(true);
              }
            }
          }
        }
      else
        {
        blueprint.SetActive(false);
        }
      //}
    }

    [Command(requiresAuthority = false)]
    private void spawnPrefabCmd(Vector3 spawnLocation, Quaternion spawnRotation)
    {
        Debug.Log("COMMAND");
        GameObject SpawnedObject = Instantiate(prefab, spawnLocation, spawnRotation);
        NetworkServer.Spawn(SpawnedObject);
    }
  }
