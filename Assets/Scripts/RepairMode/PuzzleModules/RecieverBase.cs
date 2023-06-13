using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class handling objects that react to signals from wiresConnected.
/// </summary>
public class RecieverBase : MonoBehaviour
{
    [SerializeField] protected WireBase[] wiresConnected;
    [HideInInspector] public bool isPowered;
    ReplacementShader replacementShader;

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

    private void Start()
    {
        replacementShader = Camera.main.GetComponent<ReplacementShader>();
    }

    protected virtual void Update()
    {
        // run for only the first frame that activated was set true.
        if(arePowerOnCondsMet && !isPowered)
        {
            OnRecievingPower();
        }
        else if (!arePowerOnCondsMet && isPowered)
        {
            OnLosingPower();
        }
    }

    /// <summary>
    /// What reciever does when its activated conditions are met, after being inactive.
    /// </summary>
    protected virtual void OnRecievingPower()
    {
        isPowered = true;
        if (replacementShader.rendererIndex == 1) { gameObject.layer = 6; }
    }

    /// <summary>
    /// What reciever does when its activated conditions are no longer met, after being active.
    /// </summary>
    protected virtual void OnLosingPower() 
    {
        isPowered = false;
        if (replacementShader.rendererIndex == 1) { gameObject.layer = 0; }
    }
}
