using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for wires. These carry "power" through their isPowered bool.
/// </summary>
public class WireBase : MonoBehaviour
{
    [HideInInspector] public bool isPowered;
    // Also shader code for wire emitting heat which i guess wont be here.
}
