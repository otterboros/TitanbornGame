using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : IREmitterBase, IPowerProvider
{
    /// <summary>
    /// True if PowerSource is on. On by default for now, until we get to more complex puzzles.
    /// </summary>
    public bool isOn { get; } = true;
    protected override bool isEmittingHeat { get { return isOn; } }
    public int wattsProvided {get;} = 10;
}
