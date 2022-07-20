using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BlueprintColliderCounter : NetworkBehaviour
{
    public int colliderCounter = 0;

    [SerializeField] private List<Material> blueprintMaterials = new List<Material>();

    [SyncVar(hook = nameof(ChangeBlueprintMaterial))]
    private int blueprintMaterialIndex = -1;

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
    }

    [Command]
    private void ChangeBlueprintMaterialCmd(int index)
    {
        //Debug.Log("called cahnge material command");
        blueprintMaterialIndex = index;
    }

    private void ChangeBlueprintMaterial(int oldMaterial, int newMaterial)
    {
        //Debug.Log("called the change material hook");

        Renderer[] renderArray = GetComponentsInChildren<Renderer>();
        Debug.Log("found this number of renders:" + renderArray.Length);
        foreach (Renderer renderer in renderArray)
        {
            renderer.material = blueprintMaterials[newMaterial];
        }
    }
}
