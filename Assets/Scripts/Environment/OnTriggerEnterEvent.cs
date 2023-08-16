using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// OnTriggerEnter, invokes event for other objects use
/// </summary>
public class OnTriggerEnterEvent : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerEnter;

    void OnTriggerEnter(Collider col)
    {
        if (onTriggerEnter != null)
        {
            onTriggerEnter.Invoke(col);
        }
    }
}
