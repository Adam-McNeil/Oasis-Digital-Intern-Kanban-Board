using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TicketHighlight : NetworkBehaviour
{
    private Renderer ticketRenderer;
    private float metallic;
    private float metallicChange = 0.05f;

    [SyncVar(hook = nameof(UpdateHighlight))]
    private bool isHighlighted;


    private void Start()
    {
        ticketRenderer = GetComponent<Renderer>();
        metallic = ticketRenderer.material.GetFloat("_Metallic");
    }

    private void Update()
    {
        if (isHighlighted)
        {
            Debug.Log(metallic);

        }
        else
        {
           
        }
    }

    private void StartHighlight()
    {
        StartCoroutine(IncreaseMetallic());
    }

    private void StopHighlight()
    {
        StopCoroutine(IncreaseMetallic());
        StopCoroutine(DecreaseMetallic());
    }

    IEnumerator IncreaseMetallic()
    {
        while (metallic < 1)
        {
            yield return new WaitForSeconds(.05f);
            ticketRenderer.material.SetFloat("_Metallic", metallic);
            metallic += metallicChange;
        }
        StartCoroutine(DecreaseMetallic());
    }

    IEnumerator DecreaseMetallic()
    {
        while (metallic > 0)
        {
            yield return new WaitForSeconds(.05f);
            ticketRenderer.material.SetFloat("_Metallic", metallic);
            metallic -= metallicChange;
        }
        StartCoroutine(IncreaseMetallic());
    }

    private void UpdateHighlight(bool oldBool, bool newBool)
    {
        isHighlighted = newBool;
        if (isHighlighted)
        {
            StartHighlight();
        }
        else
        {
            StopHighlight();
            ticketRenderer.material.SetFloat("_Metallic", 0);
            metallic = 0;
        }
    }

    [Command(requiresAuthority = false)]
    public void UpdateHighlightCmd(bool newBool)
    {
        isHighlighted = newBool;
    }
}
