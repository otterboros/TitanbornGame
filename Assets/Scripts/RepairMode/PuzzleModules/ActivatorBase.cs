using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objects that, when activated, affect wiresConnected or signals.
/// </summary>
public class ActivatorBase : MonoBehaviour
{
    [SerializeField] protected WireBase[] wiresConnected;

    [HideInInspector] public bool isActive;

    // TODO: flesh out puzzle design. not all activators will activate a wire
    /// <summary>
    /// What activator does when activated.
    /// </summary>
    public void OnActivated()
    {
        foreach(WireBase wire in wiresConnected)
        {
            wire.isPowered = !wire.isPowered;
        }

        // More automated way of doing this than using a bool?
        isActive = true;
    }

    /// <summary>
    /// What activator does when deactivated.
    /// </summary>
    public void OnDeactivated()
    {
        foreach (WireBase wire in wiresConnected)
        {
            wire.isPowered = !wire.isPowered;
        }

        // More automated way of doing this than using a bool?
        isActive = false;
    }
}
