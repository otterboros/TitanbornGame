using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objects that, when activated, affect wiresConnected or signals.
/// </summary>
public class SwitchBase : MonoBehaviour, IPowerProvider
{
    /// <summary>
    /// The object(s) that provides power to this wire when it's isOn is true. Typically a PowerSource or Switch.
    /// </summary>
    [SerializeField] protected GameObject[] inputs;

    protected int poweredOnThreshold = 10;
    public int wattsProvided { get;} = 10;

    int cooldownTimer = 1;
    Coroutine cooldownCoroutine;
    bool isCooldownCoroutineActive { get { return cooldownCoroutine != null; } }

    /// <summary>
    /// If the power supplied by all connected power sources clears this objects's poweredOnThreshold, return true.
    /// </summary>
    protected bool isPowered
    {
        get
        {
            var wattsSuppled = 0;

            if (inputs != null)
            {
                foreach (GameObject obj in inputs)
                {
                    if (obj.TryGetComponent(out IPowerProvider powerProvider))
                    {
                        if (powerProvider.isOn) { wattsSuppled += powerProvider.wattsProvided; }
                    }
                }
            }

            if (wattsSuppled >= poweredOnThreshold) { return true; }
            else { return false; }
        }
    }

    //protected bool _isActivated = false;
    [SerializeField] protected bool _isActivated = false;
    // TODO: replace cooldown timer with either a switching animation (that calls cooldown during) or a check for canceling click b4 going again
    /// <summary>
    /// If switch has been activated (i.e. flipped, motion sensing active, etc.), return true.
    /// Includes a cooldown timer -> CooldownCoroutine. Will not Set while CooldownCoroutine is active.s
    /// </summary>
    public bool isActivated
    {
        get { return _isActivated; }
        set 
        {
            if (isCooldownCoroutineActive) { return; }
            else { _isActivated = value; StartCooldownCoroutine(); }           
        }
    }

    /// <summary>
    /// If both isActivated and isPowered are true, return true. notInput also influences this.
    /// </summary>
    public bool isOn 
    { get 
        {
            return isPowered && isActivated;
        } 
    }

    // TODO: Genericify this. I use it a lot.
    void StartCooldownCoroutine()
    {
        cooldownCoroutine = StartCoroutine(CooldownCoroutine());
    }

    /// <summary>
    /// Wait for cooldownTimer number of seconds, then deactivate.
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldownTimer);

        cooldownCoroutine = null;
    }
}
