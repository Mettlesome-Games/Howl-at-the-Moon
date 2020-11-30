using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Author: Kevin Caton-Largent
/// Controls Servant AI Behavior
/// </summary>
public class ServantAI : AI
{
    public Vector2 localPos;

    public bool canMakeWolfsbane = true;

    public float wolfsbaneDamage;
    public float wolfsbaneSpeedDefault;
    public float wolfsbaneMakeTimer;
    public bool hasWolfsbane = false;
    private float wolfsbaneDamageDefault;

    public enum EServantStates { Normal = 0, Running = 1, FoundFoodbowl = 2, CreatingWolfsbane = 3, PresentingWolfsbane = 4 };
    private EServantStates currentState = EServantStates.Normal;
    public EServantStates previousState;
    public EServantStates newState;

    public EServantStates CurrentState 
    {
        get
        {
            return currentState;
        }
        private set 
        { 
        }
    }
    public Transform foodbowlPlacementSpot;

    protected override void SetDefaultValues()
    {

        base.SetDefaultValues();

        AIType = EAISelection.Servant;
        
        previousState = currentState;
        newState = currentState;

        currentWaypointMode = EAIWaypointMode.Patrol;

        movementEnabled = true;
        attackEnabled = true;
        canMakeWolfsbane = true;

        if (wolfsbaneDamage <= 0f)
        {
            wolfsbaneDamageDefault = 1f;
            wolfsbaneDamage = wolfsbaneDamageDefault;
        }
        else
            wolfsbaneDamageDefault = wolfsbaneDamage;

        if (wolfsbaneSpeedDefault <= 0f)            
            wolfsbaneSpeedDefault = 0f;

    }

    void UpdateState()
    {
        previousState = currentState;
        currentState = newState;
        newState = currentState;

        if (currentState == EServantStates.Normal)
        {
            walkSpeed = defaultWalkSpeed;
            
            currentWaypointMode = EAIWaypointMode.Patrol;
            
            myAnimator.SetBool("Running", false);
        }
        else if (currentState == EServantStates.Running)
        {
            currentWaypointMode = EAIWaypointMode.OneWay;
            singleTarget = levelTarget;
           
            myAnimator.SetBool("Running", true);
        }
        else if (currentState == EServantStates.FoundFoodbowl)
        {
            currentWaypointMode = EAIWaypointMode.OneWay;
            currentTarget = 0;
            
            myAnimator.SetBool("Running", false);

        }
        else if (currentState == EServantStates.CreatingWolfsbane)
        {
            walkSpeed = 0f;
        }
        else if (currentState == EServantStates.PresentingWolfsbane)
        {
            walkSpeed = defaultWalkSpeed;
            currentWaypointMode = EAIWaypointMode.OneWay;
        }


    }
      protected override void PerformAction()
    {
        if (singleTarget != null)
        {
            if (hasWolfsbane)
            {
                if (singleTarget.CompareTag("Enemy") && currentState == EServantStates.PresentingWolfsbane)
                {
                    hasWolfsbane = false;
                    
                    singleTarget.gameObject.GetComponent<WerewolfAI>().TakeDamage(wolfsbaneDamage);
                    singleTarget = levelTarget;
                    newState = EServantStates.Running;
                    myAnimator.SetBool("Presenting Wolfsbane", false);
                    Destroy(foodbowlPlacementSpot.transform.GetChild(0).gameObject);
                }
            }
        }
        
    }
    private void Update()
    {
        if (hasWolfsbane)
        {
            myAnimator.SetBool("Running", false);
            myAnimator.SetBool("Presenting Wolfsbane", true);
           
        }

        if (newState != currentState)
            UpdateState();

        if (singleTarget != null)
        {
            if (singleTarget.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, singleTarget.position);
                if (currentState == EServantStates.PresentingWolfsbane && distance <= attackDistance)
                    CheckAction();
            }
        }
            
     }
}
