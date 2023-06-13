using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: RecieverBase shares all of this functionality. Combine or created a poweredObject base?
/// <summary>
/// Base class for wires. These carry "power" through their isPowered bool.
/// </summary>
public class WireBase : MonoBehaviour
{
    [HideInInspector] public bool isPowered;
    ReplacementShader replacementShader;

    private void Start()
    {
        replacementShader = Camera.main.GetComponent<ReplacementShader>();
    }

    private void Update()
    {
        if (replacementShader.rendererIndex == 1 && isPowered)
        {
            gameObject.layer = 6;
        }
        else if (replacementShader.rendererIndex == 1 && !isPowered)
        {
            gameObject.layer = 0;
        }
    }
}