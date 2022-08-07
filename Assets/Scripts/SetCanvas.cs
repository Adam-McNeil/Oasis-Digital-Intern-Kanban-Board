using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvas : MonoBehaviour
{
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
    }
}
