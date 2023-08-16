using DoorInteractionKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// A switch activated automatically by motion, light, or both.
/// </summary>
public class Switch_Occupancy : SwitchBase
{
    protected enum OccType { Motion, Light, Motion_OR_Light, MotionOnlyWhenLightOff}
    [Header("Occupancy Switch Type")]
    [SerializeField] protected OccType occType;

    protected enum CastType { None, Box, Sphere, Ray}
    [Header("Motion Detection Parameters")]
    [Tooltip("Which type of cast the switch uses")]
    [SerializeField] protected CastType castType;
    [Tooltip("Box Type - Dimensions of boxcast * 2")]
    [SerializeField] protected Vector3 boxDimensions = Vector3.one;
    [Tooltip("Sphere Type - radius of spherecast")]
    [SerializeField] protected float sphereRadius = 1f;
    [Tooltip("Box or Sphere Type - The max distance the raycast travels from switch")]
    [SerializeField] protected float castDistance = 5f;
    [Tooltip("Ray Type - The max distance the raycast travels from switch")]
    [SerializeField] protected float rayLength = 3f;

    //TODO: Learn from DoorInteractableEditor and make only relevant items appear based on type. Low Priority

    [Header("Light Detection Parameters")]
    [Tooltip("What light casting object needs to be on for this occ switch to 'detect'")]
    [SerializeField] protected Reciever_Light lightReciever;
    [SerializeField] protected DoorInteractable barrierToLight;
    protected float blockingDist = 1f;

    Collider[] hitColliders;
    protected RaycastHit hit;
    protected bool didCastHit;

    public override bool isActivated
    {
        get { return _isActivated; }
        set
        {
            if (isCooldownCoroutineActive) { return; }
            else { _isActivated = value; StartCooldownCoroutine(); }
        }
    }

    protected void Update()
    {
        switch (occType)
        {
            case OccType.Motion:
                if (DetectMotion()) { isActivated = true; }
                else { isActivated = false;}
                break;
            case OccType.Light:
                if(DetectLight()) { isActivated = true; }
                else { isActivated = false; }
                break;
            case OccType.Motion_OR_Light:
                if(DetectMotion() || DetectLight()) { isActivated = true; }
                else { isActivated = false; }
                break;
            case OccType.MotionOnlyWhenLightOff:
                if (DetectLight()) { isActivated = false; }
                else
                {
                    if (DetectMotion()) { isActivated = true; }
                    else { isActivated = false; }
                }
                break;
        }
    }

    protected bool DetectMotion()
    {
        switch (castType)
        {
            case CastType.Box:
                hitColliders = Physics.OverlapBox(transform.position + transform.TransformDirection(Vector3.forward) * castDistance, boxDimensions / 2);
                foreach(Collider collider in hitColliders)
                {
                    if(collider.gameObject.transform.tag == "Player")
                    {
                        return true;
                    }
                }
                return false;
            case CastType.Sphere:
                hitColliders = Physics.OverlapSphere(transform.position + transform.TransformDirection(Vector3.forward) * castDistance, sphereRadius);
                foreach (Collider collider in hitColliders)
                {
                    if (collider.gameObject.transform.tag == "Player")
                    {
                        return true;
                    }
                }
                return false;
            case CastType.Ray:
                didCastHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayLength);
                if (didCastHit)
                {
                    Transform objectHit = hit.transform;

                    // If object is player
                    if (objectHit.transform.tag == "Player")
                    {
                        return true;
                    }
                }
                return false;
            case CastType.None:
            default:
                Debug.Log("Error! CastType must be assigned if OccType.Motion or .Both are used.");
                return false;
        }
    }

    protected bool DetectLight()
    {
        if (lightReciever._light.isActiveAndEnabled && !IsPickableObjBlocking() && !IsStaticObjectBlocking()) {return true; }
        else { return false; }
    }

    /// <summary>
    /// If a pickable object is within blockingDist of the detector, return true;
    /// </summary>
    /// <returns></returns>
    protected bool IsPickableObjBlocking()
    {
        bool didCastHit2 = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, blockingDist);
        if (didCastHit2)
        {
            Debug.Log(hit.transform.name + " is blocking detector");

            Transform objectHit = hit.transform;

            // If object is player
            if (objectHit.TryGetComponent(out PickupableObject pickupableObject))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// If a static object barrierToLight is blocking the detector or light source, return true. Only works with DoorInteractable component currently.
    /// </summary>
    /// <returns></returns>
    protected bool IsStaticObjectBlocking()
    {
        //TODO: coded for one use case, DoorInteractable opening, for now. Make more generic if needed
        if (barrierToLight != null)
        {
            if (!barrierToLight.DoorOpen && !barrierToLight.IsOpening)
            {
                return true;
            }      
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if(occType == OccType.Motion || occType == OccType.Motion_OR_Light)
        {
            Gizmos.color = Color.red;
            switch (castType)
            {
                case CastType.Box:
                    Gizmos.DrawWireCube(transform.position + transform.TransformDirection(Vector3.forward) * castDistance, boxDimensions);
                    break;
                case CastType.Sphere:
                    Gizmos.DrawWireSphere(transform.position + transform.TransformDirection(Vector3.forward) * castDistance, sphereRadius);
                    break;
                case CastType.Ray:
                    Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * rayLength);
                    break;
            }
        }     
    }
}
