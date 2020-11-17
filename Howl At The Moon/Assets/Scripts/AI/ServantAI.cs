using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// Controls Servant AI Behavior
/// </summary>
public class ServantAI : AI
{

    public bool canMakeWolfsbane = false;
    
    public float wolfsbaneSpeedDefault;
    public float wolfsbaneMakeTimer;
    [SerializeField]
    private bool hasWolfsbane = false;
    private bool wolfsbaneTimerActive = false;

    public enum EServantStates { Normal = 0, Running = 1, Wolfsbane = 2, Dead = 3 };
    private EServantStates currentState = EServantStates.Normal;
    public EServantStates newState;

    protected override void SetDefaultValues()
    {
        base.SetDefaultValues();

        newState = currentState;
        
        hp = 2f;

        if (wolfsbaneSpeedDefault <= 0f)            
            wolfsbaneSpeedDefault = 1.5f;
        
    }
    void UpdateState()
    {
        currentState = newState;
        newState = currentState;
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
            }
        }
    }
    protected override void Action()
    {
        hasWolfsbane = false;
        if (singleTarget.CompareTag("Enemy"))
        {
            singleTarget.gameObject.GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Trapped;
            newState = EServantStates.Running;
        }
        
    }
    protected override void UpdateNavigation()
    {
        if (singleTarget != null)
        {
            if (hasWolfsbane)
                Action();
        }
        base.UpdateNavigation();
    }
    protected override void FixedUpdate()
    {
        print(HP);
        if (HP <= 0.0f)
        {
            newState = EServantStates.Dead;
        }
        if (newState != currentState)
            UpdateState();

        base.FixedUpdate();
    }

    void Update()
    {

        if (wolfsbaneTimerActive)
        {
            TickCountdowns();
        }
    }

}
