using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// Base class handling objects that react to signals from wiresConnected.
/// </summary>
public class RecieverBase : MonoBehaviour
{
    //[SerializeField] protected IPowerProvider[] inputs;
    [SerializeField] protected GameObject[] inputs;
    ReplacementShader replacementShader;

    protected int poweredOnThreshold = 10;

    [SerializeField] protected bool default_on = false;

    /// <summary>
    /// If the power supplied by all connected power sources clears this objects's poweredOnThreshold, return true.
    /// </summary>
    protected bool isPowered
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

    bool isPoweredLastFrame;
    //protected bool isPowered
    //{
    //    get
    //    {
    //        var wattsSuppled = 0;

    //        if (inputs != null)
    //        {
    //            foreach (IPowerProvider powerProviders in inputs)
    //            {
    //                if (powerProviders.isOn) { wattsSuppled += powerProviders.wattsProvided; }
    //            }
    //        }

    //        if (wattsSuppled >= poweredOnThreshold) { return true; }
    //        else { return false; }
    //    }
    //}

    protected virtual void Start()
    {
        replacementShader = Camera.main.GetComponent<ReplacementShader>(); 
    }

    protected virtual void Update()
    {
        if(isPowered && isPoweredLastFrame != isPowered)
        {
            OnRecievingPower();
        }
        else if (!isPowered && isPoweredLastFrame != isPowered)
        {
            OnLosingPower();
        }

        // Must be at end of update
        isPoweredLastFrame = isPowered;
    }

    /// <summary>
    /// What reciever does when its activated conditions are met, after being inactive.
    /// </summary>
    protected virtual void OnRecievingPower()
    {
        if (replacementShader.rendererIndex == 1 && gameObject.layer != 6) { gameObject.layer = 6; }
    }

    /// <summary>
    /// What reciever does when its activated conditions are no longer met, after being active.
    /// </summary>
    protected virtual void OnLosingPower() 
    {
        if (replacementShader.rendererIndex == 1 && gameObject.layer != 0) { gameObject.layer = 0; }
    }
}
