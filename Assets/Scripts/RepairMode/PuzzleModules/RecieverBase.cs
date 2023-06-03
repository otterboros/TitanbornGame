using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class handling objects that react to signals from wiresConnected.
/// </summary>
public class RecieverBase : MonoBehaviour
{
    [SerializeField] protected WireBase[] wiresConnected;

    // Each reciever type has different activation conditions.
    protected bool arePowerOnCondsMet
    {
        get
        {
            // if any connected wire is unpowered, return false. else return true.
            foreach (WireBase wire in wiresConnected) 
            { 
                if(!wire.isPowered) { return false; }
            }
            return true;
        }
    }
    protected bool isPowered;

    protected virtual void Update()
    {
        // run for only the first frame that activated was set true.
        if(arePowerOnCondsMet && !isPowered)
        {
            OnRecievingPower();
        }
    }

    /// <summary>
    /// What reciever does when its activated conditions are met, after being inactive.
    /// </summary>
    protected virtual void OnRecievingPower()
    {
        Debug.Log("Reciever " + transform.name + "is active!");

        // More automated way of doing this than using a bool?
        isPowered = true;
    }

    /// <summary>
    /// What reciever does when its activated conditions are no longer met, after being active.
    /// </summary>
    protected virtual void OnLosingPower() 
    {
        Debug.Log("Reciever " + transform.name + "is inactive!");

        // More automated way of doing this than using a bool?
        isPowered = false;
    }
}
