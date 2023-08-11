using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A switch activated automatically by motion, light, or both.
/// </summary>
public class Switch_Occupancy : SwitchBase
{
    protected enum OccType { Motion, Light, Both}
    [Header("Occupancy Switch Type")]
    [SerializeField] protected OccType occType;

    protected enum CastType { None, Box, Sphere, Ray}
    [Header("Motion Detection Parameters")]
    [Tooltip("Which type of cast the switch uses")]
    [SerializeField] protected CastType castType;
    [Tooltip("The max distance that cast travels from switch")]
    [SerializeField] protected float castDistance = 3f;

    [Header("Light Detection Parameters")]
    [Tooltip("What light casting object needs to be on for this occ switch to 'detect'")]
    [SerializeField] protected Reciever_Light lightReciever;
    protected float blockingDist = 1f;

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
            case OccType.Both:
                if(DetectMotion() || DetectLight()) { isActivated = true; }
                else { isActivated = false; }
                break;
        }
    }

    protected bool DetectMotion()
    {
        switch (castType)
        {
            case CastType.Box:
                didCastHit = Physics.BoxCast(transform.position, Vector3.one * castDistance, transform.forward, out hit, transform.rotation, 0);
                if (didCastHit)
                {
                    Debug.Log("Did Hit - Box");

                    Transform objectHit = hit.transform;

                    // If object is player
                    if (objectHit.TryGetComponent(out PlayerController playerController))
                    {
                        return true;
                    }
                }
                return false;
            case CastType.Sphere:
                didCastHit = Physics.SphereCast(transform.position, castDistance, transform.TransformDirection(Vector3.forward), out hit, 0);
                if (didCastHit)
                {
                    Debug.Log("Did Hit - Sphere");

                    Transform objectHit = hit.transform;

                    // If object is player
                    if (objectHit.TryGetComponent(out PlayerController playerController))
                    {
                        return true;
                    }
                }
                return false;
            case CastType.Ray:
                didCastHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, castDistance);
                if (didCastHit)
                {
                    Debug.Log("Did Hit - Ray");

                    Transform objectHit = hit.transform;

                    // If object is player
                    if (objectHit.TryGetComponent(out PlayerController playerController))
                    {
                        return true;
                    }
                }
                return false;
            case CastType.None:
            default:
                Debug.Log("Error! CastType must be assigned if OccType.Light or .Both are used.");
                return false;
        }
    }

    protected bool DetectLight()
    {
        if (lightReciever._light.isActiveAndEnabled && !IsBoxBlockingDetector()) { return true; }
        else { return false; }
    }

    /// <summary>
    /// If a pickable object is within blockingDist of the detector, return true;
    /// </summary>
    /// <returns></returns>
    protected bool IsBoxBlockingDetector()
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

    void OnDrawGizmos()
    {
        if(occType == OccType.Motion || occType == OccType.Both)
        {
            Gizmos.color = Color.red;

            //Check if there has been a hit yet
            if (didCastHit)
            {
                switch (castType)
                {
                    case CastType.Box:
                        Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                        Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, Vector3.one * castDistance / 2);
                        break;
                    case CastType.Sphere:
                        Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                        Gizmos.DrawWireSphere(transform.position, castDistance);
                        break;
                    case CastType.Ray:
                        Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                        break;
                }

            }
            //If there hasn't been a hit yet
            else
            {
                switch (castType)
                {
                    case CastType.Box:
                        Gizmos.DrawWireCube(transform.position, Vector3.one * castDistance / 2);
                        break;
                    case CastType.Sphere:
                        Gizmos.DrawWireSphere(transform.position, castDistance);
                        break;
                    case CastType.Ray:
                        Gizmos.DrawRay(transform.position, transform.forward * castDistance);
                        break;
                }

            }
        }     
    }
}
