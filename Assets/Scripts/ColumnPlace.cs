using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPlace : MonoBehaviour
{

    [SerializeField] private int placeRange = 500;
    public static bool isBeingHeld = false;
    Vector3 movePoint;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject prefabBluePrint;
    [SerializeField] private GameObject playerCopyRotation;
    GameObject blueprint;

    [SerializeField] private LayerMask raycastLayerMask;


  private void Start()
    {
      blueprint = Instantiate(prefabBluePrint);
    }
  // Update is called once per frame
  void Update()
  {
    if (GetComponent<Rigidbody>().useGravity == false)
    {
      blueprint.SetActive(true);
      RaycastHit hit;
            Debug.DrawRay(playerCopyRotation.transform.position, playerCopyRotation.transform.forward*10, new Color(0, 0, 0), 3f);
      if (Physics.Raycast(playerCopyRotation.transform.position, playerCopyRotation.transform.forward, out hit, placeRange, raycastLayerMask))
      {
        if (hit.transform.gameObject.tag == "floor")
        {
          blueprint.transform.position = hit.point;
          blueprint.transform.rotation = Quaternion.Euler(0, playerCopyRotation.transform.rotation.y, 0);
        }
      }
    } else
    {
      blueprint.SetActive(false);
    }
  }
}
