using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// Base class handling objects that react to signals from wiresConnected.
/// </summary>
public abstract class ReceiverBase : IREmitterBase
{
    [SerializeField] protected GameObject[] inputs;

    protected int poweredOnThreshold = 10;

    /// <summary>
    /// Is this receiver in it's "on" state by default
    /// </summary>
    [SerializeField] protected bool default_on = false;

    /// <summary>
    /// If the power supplied by all connected power sources clears this objects's poweredOnThreshold, return true.
    /// </summary>
    protected override bool isPowered
    {
        get
        {
            var wattsSuppled = 0;

            if (inputs != null)
            {
                foreach (GameObject obj in inputs)
                {
                    if (obj.TryGetComponent(out IPowerProvider powerProvider))
                    {
                        if (powerProvider.isOn) { wattsSuppled += powerProvider.wattsProvided; }
                    }
                }
            }

            if (wattsSuppled >= poweredOnThreshold) { return true; }
            else { return false; }
        }
    }

    protected bool isPoweredLastFrame = false;

    protected override void Update()
    {
        base.Update();

        if (isPowered && isPoweredLastFrame != isPowered)
        {
            OnReceivingPower_Receiver();
        }
        else if (!isPowered && isPoweredLastFrame != isPowered)
        {
            OnLosingPower_Receiver();
        }

        // Must be at end of update
        isPoweredLastFrame = isPowered;
    }

    protected abstract void OnReceivingPower_Receiver();
    protected abstract void OnLosingPower_Receiver();
}
