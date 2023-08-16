using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Repeats a lot of what's in FactoryArmHandler. Can we combine?

/// <summary>
/// Triggers animation for CDoor when assigned onTriggerEnterEvent or onTriggerExitEvent is triggered
/// </summary>
public class Receiver_CDoorHandler : ReceiverBase
{
    protected Animator animator;
    [SerializeField] protected OnTriggerEnterEvent onTriggerEnterEvent;
    [SerializeField] protected OnTriggerExitEvent onTriggerExitEvent;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();

    }

    protected virtual void Start()
    {
        animator.Play("Door.Idle");
    }

    protected override void OnReceivingPower_Receiver()
    {

    }

    protected override void OnLosingPower_Receiver()
    {
        animator.Play("Door.DoorClose");
    }

    void OnEnable()
    {
        onTriggerEnterEvent.onTriggerEnter.AddListener(OnTheOtherTriggerEnterMethod);
        onTriggerExitEvent.onTriggerExit.AddListener(OnTheOtherTriggerExitMethod);
    }

    void OnDisable()
    {
        onTriggerEnterEvent.onTriggerEnter.RemoveListener(OnTheOtherTriggerEnterMethod);
        onTriggerExitEvent.onTriggerExit.RemoveListener(OnTheOtherTriggerExitMethod);
    }

    void OnTheOtherTriggerEnterMethod(Collider col)
    {
        if (col.gameObject.GetComponent<PickupableObject>() != null && isPowered)
        {
            animator.Play("Door.DoorOpen");
        }
    }

    void OnTheOtherTriggerExitMethod(Collider col)
    {
        if (col.gameObject.GetComponent<PickupableObject>() != null && isPowered)
        {
            animator.Play("Door.DoorClose");
        }
    }
}
