using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffOnStart : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        this.gameObject.SetActive(false);
    }
}
