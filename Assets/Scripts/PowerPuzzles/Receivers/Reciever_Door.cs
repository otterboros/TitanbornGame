using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reciever_Door : ReceiverBase
{
    Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        if (invert_OnOffStates) { _animator.Play("Door.DoorOpen"); }
        else { _animator.Play("Door.Idle"); }
    }

    protected override void Update()
    {
        if (isPowered && isPoweredLastFrame != isPowered)
        {
            if (invert_OnOffStates) { OnLosingPower_Receiver(); }
            else { OnReceivingPower_Receiver(); }
        }
        else if (!isPowered && isPoweredLastFrame != isPowered)
        {
            if (invert_OnOffStates) { OnReceivingPower_Receiver(); }
            else {  OnLosingPower_Receiver(); }
        }

        // Must be at end of update
        isPoweredLastFrame = isPowered;
    }

    protected override void OnReceivingPower_Receiver()
    {
        _animator.Play("Door.DoorOpen");
    }

    protected override void OnLosingPower_Receiver()
    {
        _animator.Play("Door.DoorClose");
    }
}
