using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Toggle : SwitchBase
{
    public override bool isActivated
    {
        get { return _isActivated; }
        set
        {
            if (isCooldownCoroutineActive) { return; }
            else { _isActivated = value; StartCooldownCoroutine(); }
        }
    }
}
