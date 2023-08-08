using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle pickup of rigidbody objects
/// </summary>
public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    GameObject heldObject;
    Rigidbody heldObjectRB;

    [Header("Physics Parameters")]
    [SerializeField] float pickupRange = 5.0f;
    [SerializeField] float pickupForce = 150.0f;

    private void Update()
    {
        //TODO: Change this to a new input click
        //TODO: Make sure this is compatible with button pressing
        if(Input.GetMouseButtonDown(0))
        {
            if(heldObject == null)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }

        }

        if(heldObject != null)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if(Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObject.transform.position);
            heldObjectRB.AddForce(moveDirection * pickupForce);
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if(pickObj.GetComponent<PickupableObject>())
        {
            heldObjectRB = pickObj.GetComponent<Rigidbody>();
            heldObjectRB.useGravity = false;
            heldObjectRB.drag = 10;
            heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRB.transform.parent = holdArea;
            heldObject = pickObj;
        }
    }

    void DropObject()
    {
        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 1;
        heldObjectRB.constraints = RigidbodyConstraints.None;

        heldObjectRB.transform.parent = null;
        heldObject = null;
    }
}
