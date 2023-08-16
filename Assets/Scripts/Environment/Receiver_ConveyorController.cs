using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver_ConveyorController : ReceiverBase
{
    [SerializeField] RadialConveyor[] radialConveyors;
    [SerializeField] LinearConveyor[] linearConveyors;

    protected override void Update()
    {
        if (isPowered && isPoweredLastFrame != isPowered)
        {
            OnReceivingPower_Receiver();
        }
        else if (!isPowered && isPoweredLastFrame != isPowered)
        {
            OnLosingPower_Receiver();
        }

        // Must be at end of update
        isPoweredLastFrame = isPowered;
    }

    protected override void OnReceivingPower_Receiver()
    {
        SetSpeedForConveyors(0.5f);
    }

    protected override void OnLosingPower_Receiver()
    {
        SetSpeedForConveyors(0f);
    }

    public void SetSpeedForConveyors(float newSpeed)
    {
        foreach(RadialConveyor radialConveyor in radialConveyors)
        {
            radialConveyor.ChangeSpeed(newSpeed);
        }

        foreach(LinearConveyor linearConveyor in linearConveyors)
        {
            linearConveyor.ChangeSpeed(newSpeed);
        }
    }


}
