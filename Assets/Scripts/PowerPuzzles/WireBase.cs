using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wires consist of the IREmitterBase class and an input which provides power.
/// </summary>
public class WireBase : IREmitterBase
{
    /// <summary>
    /// The object that provides power to this wire when it's isOn is true. Typically a PowerSource or Switch.
    /// </summary>
    [SerializeField] GameObject input;

    protected override bool isPowered
    {
        get
        {
            // TODO: the return false isn't a helpful result from trygetcomponent failing. Could use try/catch exception handling?
            if (input.TryGetComponent(out IPowerProvider powerProvider)) { return powerProvider.isOn; }
            else { return false; }
        }
    }
}