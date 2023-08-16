using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch that once toggled on is on for timeActive, then automatically shuts off.
/// </summary>
public class Switch_Timer : SwitchBase
{
    [Header("Timer Features")]
    [Tooltip("Number of Seconds Switch is Active")]
    [SerializeField] protected int timeActive = 10;

    protected Coroutine timedSwitchCoroutine;
    protected bool isTimedSwitchOnCoroutOn { get { return timedSwitchCoroutine != null; } }

    // Note: Key difference between this and toggle is that clicking again after cooldown is over will restart the timer rather than toggle it.
    public override bool isActivated
    {
        get { return _isActivated; }
        set
        {
            if (isCooldownCoroutineActive) { return; }
            else { _isActivated = true; StartCooldownCoroutine(); StartTimerCoroutine(); }
        }
    }

    // TODO: Genericify this. I use it a lot.
    protected void StartTimerCoroutine()
    {
        if(isTimedSwitchOnCoroutOn)
        {
            Debug.Log("reset timer");
            StopCoroutine(timedSwitchCoroutine);
        }
        timedSwitchCoroutine = StartCoroutine(SwitchTimer());
    }

    /// <summary>
    /// Wait for timeActive number of seconds, then deactivate.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator SwitchTimer()
    {
        yield return new WaitForSeconds(timeActive);

        _isActivated = false;
        timedSwitchCoroutine = null;
    }
}
