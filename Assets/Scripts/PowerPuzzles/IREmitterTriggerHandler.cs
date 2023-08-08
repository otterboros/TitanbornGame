using IREmitter;
using System.Diagnostics;
/// <summary>
/// Takes trigger messages from IRTriggerHandler and sets which group of IREmitter is viewable.
/// </summary>
public class IREmitterTriggerHandler
{
    // TODO, HACK: Should be set to last saved room that player will spawn into, but for now since we don't have saving.
    //             Just set to room I'm testing.
    /// <summary>
    /// Which room's IREmitterBase are triggered by player proximity to be active.
    /// </summary>
    public static Room activeRoom = Room.Room1_1;

    /// <summary>
    /// Set the activeRoom for IREmitterBase
    /// </summary>
    /// <param name="room"></param>
    public static void SetActiveRoom(Room room)
    {
        activeRoom = room;
    }
}
