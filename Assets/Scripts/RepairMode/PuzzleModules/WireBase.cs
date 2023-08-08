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

    /// <summary>
    /// The Switch that, when isOn, will NOT the input isOn.
    /// </summary>
    [SerializeField] SwitchBase notInput;

    protected override bool isPowered
    {
        get
        {
            if(notInput == null)
            {
                // TODO: the return false isn't a helpful result from trygetcomponent failing. Could use try/catch exception handling?
                if (input.TryGetComponent(out IPowerProvider powerProvider)) { return powerProvider.isOn; }
                else { return false; }
            }
            else
            {
                if (input.TryGetComponent(out IPowerProvider powerProvider)) 
                {
                    if (notInput.isOn) { return !powerProvider.isOn; }
                    else { return powerProvider.isOn; }
                }
                else { return false; }
            }
        }
    }
}