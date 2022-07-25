using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OutlineNetworkController : NetworkBehaviour
{
    [SerializeField] private Outline outlineScript;

    [SyncVar(hook = nameof(UpdateOutlineHook))]
    private bool isOutlinedSyncVar;
    private bool isOutlined;

    private bool isLocked = false;

    private void UpdateOutlineHook(bool oldBool, bool newBool)
    {
        isOutlined = newBool;
        outlineScript.enabled = isOutlined;
    }

    [Command(requiresAuthority = false)]
    public void UpdateOutlineCmd(bool newBool, bool hasMasterAuthority)
    {
        if (isLocked && !hasMasterAuthority)
        {
            return;
        }
        isLocked = hasMasterAuthority && newBool;
        isOutlinedSyncVar = newBool;
        outlineScript.enabled = isOutlinedSyncVar;
    }

    public void UpdateOutlineLocal(bool newBool, bool hasMasterAuthority=false)
    {
        if (isServer)
        {
            if (isLocked && !hasMasterAuthority)
            {
                return;
            }
            isLocked = hasMasterAuthority && newBool;
            isOutlined = newBool;
            outlineScript.enabled = isOutlined;
        }
        else
        {
            if (isLocked && !hasMasterAuthority)
            {
                return;
            }
            isLocked = hasMasterAuthority && newBool;
            isOutlinedSyncVar = newBool;
            outlineScript.enabled = isOutlinedSyncVar;
        }
    }
}
