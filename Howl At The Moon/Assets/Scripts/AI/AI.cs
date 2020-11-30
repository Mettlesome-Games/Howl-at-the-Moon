using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Pathfinding;
/// <summary>
/// Author: Kevin Caton-Largent
/// Parent AI
/// </summary>

abstract public class AI : MonoBehaviour
{
    public Transform singleTarget;
    public List<Transform> targets;
    public Transform levelTarget;
    [SerializeField]
    [Space(10f)]
    [Tooltip("Internal value exposed for debugging")]
    protected float defaultWalkSpeed;
    public float walkSpeed;

    [SerializeField]
    [Space(10f)]
    [Tooltip("Internal value exposed for debugging")]
    protected float defaultMaxTargetDistance;
    public float maxTargetDistance;

    [SerializeField]
    [Space(10f)]
    [Tooltip("Internal value exposed for debugging")]
    protected float defaultAttackDistance;
    public float attackDistance;
        

    public enum EAISelection {Werewolf = 0, Servant = 1} 
    public EAISelection AIType;

    public enum EAIWaypointMode {OneWay = 0, Patrol = 1}
    public EAIWaypointMode currentWaypointMode = EAIWaypointMode.OneWay;
 
    public bool movementEnabled = true;
    public bool attackEnabled = true;

    public Transform characterGFX;
    public Transform characterEyesight;

    protected float hp;
    protected float hpMax;
    public float HP
    {
        get
        {
            return hp;
        }
    }


    [SerializeField]
    protected int currentTarget = 0;
    protected Vector2 currentPath;
    [SerializeField]
    protected bool reachedEndOfPatrol = false;

    protected Rigidbody2D rb;

    protected float defaultCharacterLocalscaleX, reversedCharacterLocalscaleX;
    protected float defaultEyesightLocalpositionX, reversedEyesightLocalpositionX;
    protected BoxCollider2D myCollider;

    protected Animator myAnimator;

    private void Awake()
    {
        MethodBase AwakeMethod = MethodBase.GetCurrentMethod();
     
        SetDefaultValues();

        rb = GetComponent<Rigidbody2D>();
    }
    public void AnalyzePatrolArray(Transform NewPatrol)
    {
        targets.Clear();
        foreach (Transform patrolPoint in NewPatrol)
        {
            targets.Add(patrolPoint);
        }
    }

    public void TakeDamage(float value)
    {

        if (hp - value <= 0.0f)
            hp = 0f;
        else
            hp = hp - value;

        if (hp <= 0f)
        {
            OnDeath();
        }
    }

    public void HealDamage(float value)
    {
        if (hp + value >= hpMax)
            hp = hpMax;
        else
            hp = hp + value;
    }


    protected virtual void SetDefaultValues()
    {
        hpMax = 1f;
        hp = hpMax;

        Transform levelWaypointsParent = GameObject.FindGameObjectWithTag("Level Waypoints").transform;
        
        singleTarget = levelWaypointsParent.GetChild(0);
        levelTarget = levelWaypointsParent.GetChild(0);

        defaultCharacterLocalscaleX = characterGFX.localScale.x;
        reversedCharacterLocalscaleX = -1f * characterGFX.localScale.x;
        defaultEyesightLocalpositionX = characterEyesight.localPosition.x;
        reversedEyesightLocalpositionX = -1f * characterEyesight.localPosition.x;

        myCollider = this.GetComponent<BoxCollider2D>();
        myAnimator = characterGFX.GetComponent<Animator>();
        if (walkSpeed <= 0f)
        {
            defaultWalkSpeed = 400f;
            walkSpeed = defaultWalkSpeed;
        }
        else
            defaultWalkSpeed = walkSpeed;
        
        if (maxTargetDistance <= 0f)
        {
            defaultMaxTargetDistance = 3f;
            maxTargetDistance = defaultMaxTargetDistance;
        }
        else
            defaultMaxTargetDistance = maxTargetDistance;

        if (attackDistance <= 0f)
        {
            defaultAttackDistance = 1.5f;
            attackDistance = defaultAttackDistance;
        }
        else
            defaultAttackDistance = attackDistance;



    }
   
    protected virtual void SwitchGFXDirection(Vector2 force)
    {
        if (rb.velocity.x >= 0.01f)
        {
            characterGFX.localScale = new Vector3(reversedCharacterLocalscaleX, characterGFX.localScale.y, characterGFX.localScale.z);
            characterEyesight.localPosition = new Vector3(reversedEyesightLocalpositionX, characterEyesight.localPosition.y, characterEyesight.localPosition.z);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            characterGFX.localScale = new Vector3(defaultCharacterLocalscaleX, characterGFX.localScale.y, characterGFX.localScale.z);
            characterEyesight.localPosition = new Vector3(defaultEyesightLocalpositionX, characterEyesight.localPosition.y, characterEyesight.localPosition.z);
        }
    }
   
    protected void CheckAction()
    {
        if (attackEnabled)
            PerformAction();
    }
    protected abstract void PerformAction();

    private void Movement()
    {
        if (movementEnabled)
        {

            if (currentWaypointMode == EAIWaypointMode.OneWay)
            {
                currentPath = (Vector2)singleTarget.position;
            }
            else if (currentWaypointMode == EAIWaypointMode.Patrol)
            {
                currentPath = (Vector2)targets[currentTarget].position;
            }
            Vector2 direction = (currentPath - rb.position).normalized;
            Vector2 force = direction * walkSpeed * Time.deltaTime;
            force.y = 0f;
            rb.AddForce(force);

            myAnimator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            
            float distance = Vector2.Distance(rb.position, currentPath);

            if (distance < maxTargetDistance && currentWaypointMode == EAIWaypointMode.Patrol)
            {
                if (currentTarget < targets.Count - 1 && !reachedEndOfPatrol)
                {
                    currentTarget++;
                }
                else
                {
                    reachedEndOfPatrol = true;
                    currentTarget--;
                    if (currentTarget == 0)
                        reachedEndOfPatrol = false;
                }
            }
            //Debug.LogFormat("<color=#01751a> Force: {0} </color>", force.x);
            SwitchGFXDirection(force);
        }
    }

   
    protected virtual void OnDeath()
    {
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        Movement();
    }

}

