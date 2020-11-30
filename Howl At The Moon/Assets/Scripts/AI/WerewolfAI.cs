using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Pathfinding;
/// <summary>
/// Author: Kevin Caton-Largent
/// Controls Werewolf AI Behavior
/// </summary>
public class WerewolfAI : AI
{
    public Vector2 localPos;

    public WaveController masterCommander;
    public uint MonsterID, WaveID;
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
    [SerializeField]
    private bool canSwingAttack = true;

    // Distracted was a planned feature to have additional environmental distractions for werewolves but we ran out of time
    public enum EWerewolfStates { Normal = 0, Chasing = 1, Distracted = 2, Trapped = 3, Cursed = 4 };
    private EWerewolfStates currentState = EWerewolfStates.Normal;
    public EWerewolfStates previousStates;

    public EWerewolfStates CurrentState
    {
        get
        {
            return currentState;
        }
        private set
        {
        }
    }
    public EWerewolfStates newState;

    public AnimationClip redEyesWalking, redEyesIdle, redEyesAttack;

    protected AnimatorOverrideController myAnimatorOverrideController;

    protected override void SetDefaultValues()
    {
        base.SetDefaultValues();
        AIType = EAISelection.Werewolf;

        previousStates = currentState;
        newState = currentState;

        myAnimatorOverrideController = new AnimatorOverrideController(myAnimator.runtimeAnimatorController);
        myAnimator.runtimeAnimatorController = myAnimatorOverrideController;

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
    }

    protected override void SwitchGFXDirection(Vector2 force)
    {
        if (rb.velocity.x >= 0.01f)
        {
            characterGFX.localScale = new Vector3(defaultCharacterLocalscaleX, characterGFX.localScale.y, characterGFX.localScale.z);
            characterEyesight.localPosition = new Vector3(defaultEyesightLocalpositionX, characterEyesight.localPosition.y, characterEyesight.localPosition.z);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            characterGFX.localScale = new Vector3(reversedCharacterLocalscaleX, characterGFX.localScale.y, characterGFX.localScale.z);
            characterEyesight.localPosition = new Vector3(reversedEyesightLocalpositionX, characterEyesight.localPosition.y, characterEyesight.localPosition.z);
        }
    }

    void UpdateState()
    {
        previousStates = currentState;
        currentState = newState;
        newState = currentState;
        if (currentState == EWerewolfStates.Normal)
        {
            if (previousStates != EWerewolfStates.Cursed)
            {
                walkSpeed = defaultWalkSpeed;
                attackSpeed = defaultAttackSpeed;
            }
            singleTarget = null;
        }
        else if (currentState == EWerewolfStates.Chasing)
        {
            if (previousStates != EWerewolfStates.Cursed)
                walkSpeed = chaseWalkSpeed;
        }
        else if (currentState == EWerewolfStates.Distracted)
        {
            walkSpeed = 0f;
            attackSpeed = 0f;
            singleTarget = null;

            movementEnabled = false;
            attackEnabled = false;
            rb.gravityScale = 0f;
            myCollider.enabled = false;

        }
        else if (currentState == EWerewolfStates.Trapped)
        {
            walkSpeed = 0f;
            attackSpeed = 0f;
            singleTarget = null;

            movementEnabled = false;
            attackEnabled = false;
            myAnimator.SetFloat("Speed", 0);
            rb.gravityScale = 0f;
            myCollider.enabled = false;
        }
        else if (currentState == EWerewolfStates.Cursed)
        {
            walkSpeed = cursedWalkSpeed;
            attackSpeed = cursedAttackSpeed;
            myAnimatorOverrideController["werewolf_idle"] = redEyesIdle;
            myAnimatorOverrideController["werewolf_walking"] = redEyesWalking;
            myAnimatorOverrideController["werewolf_attack"] = redEyesAttack;
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
    protected override void PerformAction()
    {
        if (singleTarget != null)
        {
            if (canSwingAttack)
            {
                if (singleTarget.CompareTag("Servant"))
                {
                    canSwingAttack = false;
                    singleTarget.GetComponent<ServantAI>().TakeDamage(attackDmg);
                    myAnimator.SetBool("Attack", true);

                    InvokeAttackCountdown();
                }
                else if (singleTarget.CompareTag("ManorLord"))
                {
                    canSwingAttack = false;
                    singleTarget.GetComponent<ManorLordAI>().TakeDamage(attackDmg);
                    myAnimator.SetBool("Attack", true);

                    InvokeAttackCountdown();
                }
            }
        }
    }
    protected override void OnDeath()
    {
        masterCommander.TickKilledEnemies();
        base.OnDeath();
    }
    private void Update()
    {
        if (singleTarget == null && currentState == EWerewolfStates.Chasing)
        {
            canSwingAttack = true;
            attackTimerActive = false;
            attackTimer = attackSpeed;
            newState = previousStates;
        }

        if (newState != currentState && currentState != EWerewolfStates.Trapped)
            UpdateState();

        if (singleTarget.CompareTag("Servant") || singleTarget.CompareTag("ManorLord"))
        {
            float distance = Vector2.Distance(transform.position, singleTarget.position);
            if (currentState == EWerewolfStates.Chasing || currentState == EWerewolfStates.Cursed)
                if (distance <= attackDistance)
                    CheckAction();
        }

        if (attackTimerActive)
            TickCountdowns();
    }

}