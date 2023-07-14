using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objects that, when activated, affect wiresConnected or signals.
/// </summary>
public class SwitchBase : MonoBehaviour, IPowerProvider
{
    //[SerializeField] protected IPowerProvider[] inputs;
    [SerializeField] protected GameObject[] inputs;
    protected int poweredOnThreshold = 10;
    public int wattsProvided { get;} = 10;

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
    //protected bool isPowered 
    //{ get 
    //    {
    //        var wattsSuppled = 0;

    //        if(inputs != null)
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

    /// <summary>
    /// If switch has been activated (i.e. flipped, motion sensing active, etc.), return true.
    /// </summary>
    protected bool _isActivated = false;
    public bool isActivated
    {
        get { return _isActivated; }
        set { _isActivated = value; }
    }

    /// <summary>
    /// If both isActivated and isPowered are true, return true.
    /// </summary>
    public bool isOn { get { return isPowered && isActivated; } }
}
