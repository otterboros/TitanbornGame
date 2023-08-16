using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Repeats a lot of what's in CDoorHandler. Can we combine?
/// <summary>
/// Triggers animation for FactoryArm when assigned onTriggerEnterEvent is triggered
/// </summary>
public class Receiver_FactoryArm : ReceiverBase
{
    protected Animator animator;
    [SerializeField] Light _light;
    [SerializeField] protected OnTriggerEnterEvent onTriggerEnterEvent;

    protected override void Awake()
    {
        base.Awake();

        _light = GetComponentInChildren<Light>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        _light.enabled = false;
    }

    void OnEnable()
    {
        onTriggerEnterEvent.onTriggerEnter.AddListener(OnTheOtherTriggerEnterMethod);
    }

    void OnDisable()
    {
        onTriggerEnterEvent.onTriggerEnter.RemoveListener(OnTheOtherTriggerEnterMethod);
    }

    protected override void OnReceivingPower_Receiver()
    {
        _light.enabled = true;
    }

    protected override void OnLosingPower_Receiver()
    {
        _light.enabled = false;
    }

    void OnTheOtherTriggerEnterMethod(Collider col)
    {
        if (col.gameObject.GetComponent<PickupableObject>() != null && isPowered)
        {
            animator.SetTrigger("StartCycle");
        }
    }
}
