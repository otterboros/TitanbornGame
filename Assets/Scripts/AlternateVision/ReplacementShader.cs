using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Change the shader in the main camera's renderer based on player input
/// </summary>
public class ReplacementShader : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.UniversalAdditionalCameraData universalAdditionalCameraData;
    private ExploreModeInputs _input;

    // TODO: Might be a way to get the actual renderer info better than setting an int
    /// <summary>
    /// Exposing the renderer index, because it's private in UniversalAdditionalCameraData.
    /// </summary>
    public int rendererIndex;

    Coroutine waitForButtonPress;
    bool isWaitForButtonPressRunning { get { return waitForButtonPress != null; } }

    // TODO: cache these somewhere global so each wire doesn't have to grab em individually
    private void Start()
    {
        universalAdditionalCameraData = GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        GameObject inputs = FindObjectOfType<ExploreModeInputs>().gameObject;
        _input = inputs.GetComponent<ExploreModeInputs>();
    }

    private void Update()
    {
        if (_input.changeVision && !isWaitForButtonPressRunning) { waitForButtonPress = StartCoroutine(WaitForButtonPress()); }
    }

    // TODO: gotta be a better way to just get the renderer itself so im not using a bool here.
    void ToggleRenderer()
    {
        if (rendererIndex == 0) { universalAdditionalCameraData.SetRenderer(1); rendererIndex = 1; }
        else { universalAdditionalCameraData.SetRenderer(0); rendererIndex = 0; }
    }

    /// <summary>
    /// ToggleRenderer once, then wait while ChangeVision is still true.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForButtonPress()
    {
        ToggleRenderer();

        while (_input.changeVision)
        {
            yield return new WaitForEndOfFrame();
        }

        waitForButtonPress = null;
    }
}
