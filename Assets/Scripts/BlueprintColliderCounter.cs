using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BlueprintColliderCounter : NetworkBehaviour
{
    public bool canPlaceColumn = false;

    [SerializeField] private List<Material> blueprintMaterials = new List<Material>();
    PlayerController localPlayerController;
    Vector3 overlapTestBlueprintScale = new Vector3(15, 20, 4);
    float xOffset = -3.5f;
    float yOffset = 10f;
    Collider[] overlapedColliders;

    [SyncVar(hook = nameof(ChangeBlueprintMaterial))]
    private int blueprintMaterialIndex = -1;

    private void Start()
    {
        localPlayerController = GameObject.FindGameObjectWithTag("Local Player").GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (localPlayerController.isBuilding)
        {
            if (Physics.OverlapBox(this.transform.position + this.transform.right * xOffset + this.transform.up * yOffset, overlapTestBlueprintScale / 2, this.transform.rotation).Length == 0)
            {
                ChangeBlueprintMaterialCmd(1);
                canPlaceColumn = true;
            }
            else
            {
                ChangeBlueprintMaterialCmd(0);
                canPlaceColumn = false;
            }
        }
    }

    [Command(requiresAuthority = false)]
    private void ChangeBlueprintMaterialCmd(int index)
    {
        blueprintMaterialIndex = index;
    }

    private void ChangeBlueprintMaterial(int oldMaterial, int newMaterial)
    {
        Renderer[] renderArray = GetComponentsInChildren<Renderer>();
        Debug.Log("found this number of renders:" + renderArray.Length);
        foreach (Renderer renderer in renderArray)
        {
            renderer.material = blueprintMaterials[newMaterial];
        }
    }
/*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enterd collision");
        colliderCounter++;
        if (colliderCounter == 1)
        {
            ChangeBlueprintMaterialCmd(0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("exited collision");
        colliderCounter--;
        if (colliderCounter == 0)
        {
            ChangeBlueprintMaterialCmd(1);
        }
    }*/
}
