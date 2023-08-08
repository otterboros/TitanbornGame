using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Class that PickupController checks for to see if an object can be pickedup.
/// Ensures that object has a rigidbody.
/// </summary>
public class PickupableObject : MonoBehaviour
{
    void Start()
    {
        // Ensure that this pickupableObject has a rigidbody
        if(transform.GetComponent<Rigidbody>() == null)
        {
            transform.AddComponent<Rigidbody>();
        }        
    }
}
