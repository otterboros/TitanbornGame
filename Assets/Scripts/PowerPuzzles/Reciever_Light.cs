using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reciever_Light : RecieverBase
{
    Light _light;

    protected override void Start()
    {
        base.Start();

        _light = GetComponentInChildren<Light>();
        if (default_on) { _light.enabled = true; }
        else { _light.enabled = false; }
    }

    protected override void OnRecievingPower()
    {
        base.OnRecievingPower();
        _light.enabled= true;
    }

    protected override void OnLosingPower()
    {
        base.OnLosingPower();
        _light.enabled = false;
    }
}
