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

    public bool canMakeWolfsbane = false;

    public float wolfsbaneDamage;
    public float wolfsbaneSpeedDefault;
    public float wolfsbaneMakeTimer;
    public bool hasWolfsbane = false;
    public bool wolfsbaneTimerActive = false;
    private float defaultNextWaypointDistance;
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


    protected override void Awake()
    {
        MethodBase AwakeMethod = MethodBase.GetCurrentMethod();
        /*Debug.Log("<color=#4f7d00>Subscribing to OnDeath at function call: " + AwakeMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name + "</color>", this);
        ServantAI.OnDeathEvent += OnDeath;*/
        base.Awake();
    }


    protected override void SetDefaultValues()
    {
        base.SetDefaultValues();
        AIType = EAISelection.Servant;
        
        previousState = currentState;
        newState = currentState;

        currentWaypointMode = EAIWaypointMode.Patrol;

        movementEnabled = true;
        attackEnabled = true;

        if (wolfsbaneDamage <= 0f)
        {
            wolfsbaneDamageDefault = 1f;
            wolfsbaneDamage = wolfsbaneDamageDefault;
        }
        else
            wolfsbaneDamageDefault = wolfsbaneDamage;

        if (wolfsbaneSpeedDefault <= 0f)            
            wolfsbaneSpeedDefault = 0f;

        targets = patrolWaypoints;
        if (nextWaypointDistance <= 0f)
        {
            defaultNextWaypointDistance = 3f;
            nextWaypointDistance = defaultNextWaypointDistance;
        }
        else
        {
            defaultNextWaypointDistance = nextWaypointDistance;
        }
    }

    void UpdateState()
    {
        previousState = currentState;
        currentState = newState;
        newState = currentState;

        if (currentState == EServantStates.Normal)
        {
            walkSpeed = defaultWalkSpeed;
            targets = patrolWaypoints;

            currentWaypointMode = EAIWaypointMode.Patrol;
            
            currentTarget = 0;
            nextWaypointDistance = defaultNextWaypointDistance;
            revalulatePathing = true;

            myAnimator.SetBool("Running", false);
        }
        else if (currentState == EServantStates.Running)
        {
            currentWaypointMode = EAIWaypointMode.OneWay;
            targets = levelWaypoints;
            nextWaypointDistance = defaultNextWaypointDistance;
            revalulatePathing = true;

            myAnimator.SetBool("Running", true);
        }
        else if (currentState == EServantStates.FoundFoodbowl)
        {
            currentWaypointMode = EAIWaypointMode.OneWay;
            currentTarget = 0;
            nextWaypointDistance = .1f;
            
            revalulatePathing = true;

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
    protected void InvokeWolfsbaneCountdown()
    {
        if (!wolfsbaneTimerActive && canMakeWolfsbane)
        {
            wolfsbaneTimerActive = true;
            wolfsbaneMakeTimer = wolfsbaneSpeedDefault;
            newState = EServantStates.CreatingWolfsbane;
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
                if (singleTarget.CompareTag("Food Bowl"))
                {
                    if (!canMakeWolfsbane)
                    {
                        singleTarget.parent = foodbowlPlacementSpot;
                        foodbowlPlacementSpot.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
                        singleTarget = null;
                    }
                }
            }
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
                    singleTarget = null;
                    newState = EServantStates.Running;
                    myAnimator.SetBool("Presenting Wolfsbane", false);
                    Destroy(foodbowlPlacementSpot.transform.gameObject);
                }
            }
        }
        
    }
    private void Update()
    {
        if (currentState == EServantStates.FoundFoodbowl && canMakeWolfsbane)
        {
            InvokeWolfsbaneCountdown();
        }
        if (hasWolfsbane)
        {
            myAnimator.SetBool("Running", false);
            myAnimator.SetBool("Presenting Wolfsbane", true);
           
        }
        if (newState != currentState)
            UpdateState();
     
        if (reachedEndOfPath)
        {
            if (singleTarget != null)
            {
                float distance = Vector2.Distance(transform.position, singleTarget.position);
                if (currentState == EServantStates.PresentingWolfsbane && distance <= 1.5f)
                    CheckAction();
            }
        }
            
        if (wolfsbaneTimerActive)
            TickCountdowns(); 
    }
    protected override void OnDestroy()
    {
        MethodBase OnDestroyMethod = MethodBase.GetCurrentMethod();
        /*Debug.Log("<color=#910a00>Unsubscribing to OnDeath at function call: " + OnDestroyMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name + "</color>", this);
        ServantAI.OnDeathEvent -= OnDeath;*/
        base.OnDestroy();
    }

}
