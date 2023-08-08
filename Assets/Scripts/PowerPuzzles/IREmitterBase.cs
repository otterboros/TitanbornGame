using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IREmitter;

/// <summary>
/// Base class for objects that can change color in "IR" mode.
/// </summary>
public abstract class IREmitterBase : MonoBehaviour
{
    protected ReplacementShader replacementShader;
    protected abstract bool isPowered { get; }

    /// <summary>
    /// Enum label of which puzzle room this emitter is assigned to.
    /// </summary>
    [SerializeField] protected Room room;

    protected virtual void Awake()
    {
        replacementShader = Camera.main.GetComponent<ReplacementShader>();
    }

    // TODO, HACK: There's probs a way to do this without calling each frame.
    protected virtual void Update()
    {
        if (isPowered && IREmitterTriggerHandler.activeRoom == room)
        {
            OnReceivingPower_Emission();
        }
        else
        {
            OnLosingPower_Emission();
        }
    }

    /// <summary>
    /// What obj does when its activated conditions are met, after being inactive.
    /// </summary>
    protected virtual void OnReceivingPower_Emission()
    {
        if (IREmitterTriggerHandler.activeRoom == room) 
        { gameObject.layer = 6; }
    }

    /// <summary>
    /// What obj does when its activated conditions are no longer met.
    /// </summary>
    protected virtual void OnLosingPower_Emission()
    {
        gameObject.layer = 0;
    }
}