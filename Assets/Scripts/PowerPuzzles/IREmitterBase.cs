using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objects that can change color in "IR" mode.
/// </summary>
public abstract class IREmitterBase : MonoBehaviour
{
    protected ReplacementShader replacementShader;
    protected abstract bool isEmittingHeat { get; }

    protected virtual void Start()
    {
        replacementShader = Camera.main.GetComponent<ReplacementShader>();
    }

    protected virtual void Update()
    {
        // if replacementShader is in IR mode
        if (replacementShader.rendererIndex == 1) { IREmissionManager(); }
    }

    protected void IREmissionManager()
    {
        if (isEmittingHeat)
        {
            gameObject.layer = 6;
        }
        else
        {
            gameObject.layer = 0;
        }
    }
}
