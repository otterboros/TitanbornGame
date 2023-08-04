using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reciever_Door : RecieverBase
{
    Animator _animator;

    protected override void Start()
    {
        base.Start();

        _animator = GetComponentInChildren<Animator>();
        if (default_on) { _animator.Play("Door.DoorOpen"); }
        else { _animator.Play("Door.Idle"); }
    }

    protected override void OnRecievingPower()
    {
        //base.OnRecievingPower();
        _animator.Play("Door.DoorOpen");
    }

    protected override void OnLosingPower()
    {
        //base.OnLosingPower();
        _animator.Play("Door.DoorClose");
    }
}
