using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
/// <summary>
/// Author: Kevin Caton-Largent
/// Controls Werewolf AI Behavior
/// </summary>
public class WerewolfAI : AI
{
    private float defaultAttackSpeed;
    private float defaultAttackDmg;
    private float defaultChaseWalkSpeed;
    private float defaultCursedWalkSpeed;
    private float defaultCursedAttackSpeed;

    public float attackTimer;
    public float attackSpeed;
    public float attackDmg;
    public float chaseWalkSpeed;
    public float cursedWalkSpeed;
    public float cursedAttackSpeed;

    [SerializeField]
    private bool attackTimerActive = false;
    private bool canSwingAttack = true;

    public enum EWerewolfStates { Normal = 0, Chasing = 1, Distracted = 2, Trapped = 3, Cursed = 4};
    private EWerewolfStates currentState = EWerewolfStates.Normal;
    private EWerewolfStates previousStates;

    public EWerewolfStates PreviousStates
    {
        get
        {
            return previousStates;
        }
        set
        {
            if (value == EWerewolfStates.Normal)
                previousStates = value;
            else if (value == EWerewolfStates.Cursed)
                previousStates = value;
        }
    }
    public EWerewolfStates newState; 

    protected override void SetDefaultValues()
    {
        base.SetDefaultValues();
        newState = currentState;
        if (attackSpeed <= 0f)
        {
            defaultAttackSpeed = 5f;
            attackSpeed = defaultAttackSpeed;
        }
        else
            defaultAttackSpeed = attackSpeed;

        if (attackDmg <= 0f)
        {
            defaultAttackDmg = 1f;
            attackDmg = defaultAttackDmg;
        }
        else
            defaultAttackDmg = attackDmg;

        if (chaseWalkSpeed <= 0f)
        {
            defaultChaseWalkSpeed = 500f;
            chaseWalkSpeed = defaultChaseWalkSpeed;
        }
        else
            defaultChaseWalkSpeed = chaseWalkSpeed;

        if (cursedWalkSpeed <= 0f)
        {
            defaultCursedWalkSpeed = 600f;
            cursedWalkSpeed = defaultCursedWalkSpeed;
        }
        else
            defaultCursedWalkSpeed = cursedWalkSpeed;

        if (cursedAttackSpeed <= 0f)
        {
            defaultCursedAttackSpeed = 3f;
            cursedAttackSpeed = defaultCursedAttackSpeed;
        }
        else
            defaultCursedAttackSpeed = cursedAttackSpeed;

        targets = levelWaypoints; 

    }
    void UpdateState()
    {
        PreviousStates = currentState;
        currentState = newState;
        newState = currentState;
        if (currentState == EWerewolfStates.Normal)
        {
            if (PreviousStates != EWerewolfStates.Cursed)
            {
                walkSpeed = defaultWalkSpeed;
                attackSpeed = defaultAttackSpeed;
            }
            singleTarget = null;
        }
        else if (currentState == EWerewolfStates.Chasing)
        {
            if (PreviousStates != EWerewolfStates.Cursed)
                walkSpeed = chaseWalkSpeed;
        }
        else if (currentState == EWerewolfStates.Distracted)
        {
            walkSpeed = 0f;
            attackSpeed = 0f;
        }
        else if (currentState == EWerewolfStates.Trapped)
        {
            walkSpeed = 0f;
            attackSpeed = 0f;
            singleTarget = null;
        }
        else if (currentState == EWerewolfStates.Cursed)
        {
            walkSpeed = cursedWalkSpeed;
            attackSpeed = cursedAttackSpeed;
        }
    }
    protected void InvokeAttackCountdown()
    {
        if (!attackTimerActive)
        {
            attackTimerActive = true;
            attackTimer = attackSpeed;
        }
    }
    private void TickCountdowns()
    {
        if (attackTimerActive)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                attackTimer = attackSpeed;
                attackTimerActive = false;
                canSwingAttack = true;
            }
        }
    }
    protected override void Action()
    {
        if (singleTarget != null)
        {   if (canSwingAttack)
            {
                canSwingAttack = false;
                if (singleTarget.CompareTag("Servant"))
                    singleTarget.GetComponent<ServantAI>().TakeDamage(attackDmg);

                InvokeAttackCountdown();
            }
        }
    }

    private void Update()
    {
        if (newState != currentState)
            UpdateState(); 

        if (reachedEndOfPath)
            Action();

        if (attackTimerActive)
            TickCountdowns();
    }

}