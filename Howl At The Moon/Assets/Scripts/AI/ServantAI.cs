using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Author: Kevin Caton-Largent
/// Controls Servant AI Behavior
/// </summary>
public class ServantAI : AI
{

    public bool canMakeWolfsbane = false;
    
    public float wolfsbaneSpeedDefault;
    public float wolfsbaneMakeTimer;
    public  bool hasWolfsbane = false;
    public bool wolfsbaneTimerActive = false;

    public enum EServantStates { Normal = 0, Running = 1, CreatingWolfsbane = 2, PresentingWolfsbane = 3, Dead = 4 };
    private EServantStates currentState = EServantStates.Normal;
    public EServantStates newState;

    protected override void SetDefaultValues()
    {
        base.SetDefaultValues();

        newState = currentState;

        hpMax = 2f;
        hp = hpMax;

        if (wolfsbaneSpeedDefault <= 0f)            
            wolfsbaneSpeedDefault = 1.5f;

        targets = patrolWaypoints;
    }
    void UpdateState()
    {
        currentState = newState;
        newState = currentState;

        if (currentState == EServantStates.Normal)
        {
            walkSpeed = defaultWalkSpeed;
            targets = patrolWaypoints;
            currentTarget = 0;
            currentWaypoint = 0;
            UpdateNavigation();
        }
        else if (currentState == EServantStates.Running)
        {
            targets = levelWaypoints;
            currentTarget = 0;
            currentWaypoint = 0;
            UpdateNavigation();
        }
        else if (currentState == EServantStates.CreatingWolfsbane)
        {
            walkSpeed = 0f;
        }
        else if (currentState == EServantStates.PresentingWolfsbane)
        {
            walkSpeed = defaultWalkSpeed;
        }
        else if (currentState == EServantStates.Dead)
        {
            Destroy(this.gameObject);
        }

    }
    protected void InvokeWolfsbaneCountdown()
    {
        if (!wolfsbaneTimerActive && canMakeWolfsbane)
        {
            wolfsbaneTimerActive = true;
            wolfsbaneMakeTimer = wolfsbaneSpeedDefault;
        }
    }
    private void TickCountdowns()
    {
        if (wolfsbaneTimerActive)
        {
            wolfsbaneMakeTimer -= Time.deltaTime;
            if (wolfsbaneMakeTimer <= 0f)
            {
                wolfsbaneTimerActive = false;
                hasWolfsbane = true;
                canMakeWolfsbane = false;
                newState = EServantStates.Normal;
            }
        }
    }
    protected override void Action()
    {
        if (singleTarget != null)
        {
            if (hasWolfsbane)
            {
                hasWolfsbane = false;
                if (singleTarget.CompareTag("Enemy"))
                {
                    singleTarget.gameObject.GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Trapped;
                    newState = EServantStates.Running;
                }
            }
        }
        
    }
    private void Update()
    {
        if (HP <= 0.0f)
            newState = EServantStates.Dead;

        if (newState != currentState)
            UpdateState();

        if (reachedEndOfPath)
        {
            if (currentState == EServantStates.PresentingWolfsbane)
                Action();
            else if (currentState == EServantStates.Normal)
            {
                if (currentTarget == targets.Count - 1)
                {
                   
                }
            }
                
        }
            
        if (wolfsbaneTimerActive)
            TickCountdowns(); 
    }

}
