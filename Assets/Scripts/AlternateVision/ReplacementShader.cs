using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementShader : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().SetRenderer(1);
    }

    private void OnDisable()
    {
        GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().SetRenderer(0);
    }
}
