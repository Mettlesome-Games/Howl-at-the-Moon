using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// Status: (WIP)
/// This program serves as the base timer classe to be inherited by future timers
/// </summary>
abstract public class Timer : MonoBehaviour
{
    // Do you want the timers to print to the console log yes or no?
    public enum EPrintingAvailability {Yes = 1, No = 0};
    [Space(10)]
    [Header("Printing Enabler")]
    [Tooltip("Enable to allow this object to print its cooldowns to the console")]
    public EPrintingAvailability Printing;
             
    /// <summary>
    /// This function starts the timer with the appropriate countdown
    /// </summary>
    /// <param name="TimerKey"> The signature of the timer to invoke </param>
    public abstract void InvokeTimer(int TimerKey);

    /// <summary>
    /// Advances the countdowns that are turned on
    /// </summary>
    public abstract void TickCountdowns();


    /// <summary>
    /// Until a ui is setup/designed this will serve as a way to see the countdowns in action
    /// </summary>
    public abstract void PrintCountdowns();
    


    /// <summary>
    /// This function populates the default values
    /// </summary>
    public abstract void InitializeDefaults();
    

    protected virtual void Awake()
    {
      InitializeDefaults();
    }

    
    protected virtual void Update()
    {
        TickCountdowns();
        PrintCountdowns();
    }
}
