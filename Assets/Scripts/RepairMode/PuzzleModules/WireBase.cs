using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wires consist of the IREmitterBase class and an input which provides power.
/// </summary>
public class WireBase : IREmitterBase
{
    //[SerializeField] IPowerProvider input;
    [SerializeField] GameObject input;

    protected override bool isEmittingHeat
    {
        get
        {
            if (input.TryGetComponent(out IPowerProvider powerProvider)) { return powerProvider.isOn; }
            else { return false; }
        }
    }
}