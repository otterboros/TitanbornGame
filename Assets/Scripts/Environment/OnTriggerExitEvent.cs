using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// OnTriggerExit, invokes event for other objects use
/// </summary>
public class OnTriggerExitEvent : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerExit;

    void OnTriggerExit(Collider col)
    {
        if (onTriggerExit != null)
        {
            onTriggerExit.Invoke(col);
        }
    }
}
