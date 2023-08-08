using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reciever_Light : ReceiverBase
{
    Light _light;

    protected override void Awake()
    {
        base.Awake();

        _light = GetComponentInChildren<Light>();
    }

    protected virtual void Start()
    {
        if (default_on) { _light.enabled = true; }
        else { _light.enabled = false; }
    }

    protected override void OnReceivingPower_Receiver()
    {
        _light.enabled= true;
    }

    protected override void OnLosingPower_Receiver()
    {
        _light.enabled = false;
    }
}
