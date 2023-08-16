using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IREmitter;
/// <summary>
/// Determines which objects will be visible in IR vision, based on player collision with IR triggers.
/// </summary>
public class IRTriggerHandler : MonoBehaviour
{
    // TODO, HACK: Should be Enter Exit but for now since we spawn right in a room it's Stay Exit.
    /// <summary>
    /// Set IR active room based on player triggering room triggers.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IRTriggerLabel>(out IRTriggerLabel iRTriggerLabel) && IREmitterTriggerHandler.activeRoom != iRTriggerLabel.thisTriggerRoom)
        {
            IREmitterTriggerHandler.SetActiveRoom(iRTriggerLabel.thisTriggerRoom);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IRTriggerLabel>(out IRTriggerLabel iRTriggerLabel) && IREmitterTriggerHandler.activeRoom == iRTriggerLabel.thisTriggerRoom)
        {
            IREmitterTriggerHandler.SetActiveRoom(Room.None);
        }
    }
}
