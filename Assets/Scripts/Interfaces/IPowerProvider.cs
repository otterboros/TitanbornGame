using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerProvider
{
    public bool isOn { get;}
    public int wattsProvided { get;}
}
