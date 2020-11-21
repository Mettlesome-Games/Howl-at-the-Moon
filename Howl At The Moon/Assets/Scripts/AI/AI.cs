using System.Collections;
using System.Collections.Generic;
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
    protected List<Transform> patrolWaypoints, levelWaypoints;
    public enum EAIWaypointsEditing { AutoSearchLevel = 0, ManualAlotment = 1}
    public EAIWaypointsEditing CurrentWaypointEditingMode = EAIWaypointsEditing.AutoSearchLevel;

    public enum EAISelection {Werewolf = 0, Servant = 1} 
    public EAISelection AIType;


    public enum EAIWaypointMode {OneWay = 0, Patrol = 1}
    public EAIWaypointMode currentWaypointMode = EAIWaypointMode.OneWay;
    public float walkSpeed;
    public float nextWaypointDistance;

    public bool climbing = false;
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

    protected Path path;

    protected float defaultWalkSpeed;
    [SerializeField]
    protected int currentWaypoint = 0;
    [SerializeField]
    protected int currentTarget = 0;

    [SerializeField]
    protected bool reachedEndOfPath = false;
    protected bool reachedEndOfPatrol = false;

    protected Seeker seeker;
    protected Rigidbody2D rb;

    protected float defaultCharacterLocalscaleX, reversedCharacterLocalscaleX;
    protected float defaultEyesightLocalpositionX, reversedEyesightLocalpositionX;
    protected CircleCollider2D myCollider;

    public void TakeDamage(float value)
    {
        if (hp - value <= 0.0f)
            hp = 0f;
        else
            hp = hp - value;
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
        if (CurrentWaypointEditingMode == EAIWaypointsEditing.AutoSearchLevel)
        {
            levelWaypoints = new List<Transform>();
            Transform levelWaypointsParent = GameObject.FindGameObjectWithTag("Level Waypoints").transform;
            foreach (Transform lChild in levelWaypointsParent)
                levelWaypoints.Add(lChild);

            patrolWaypoints = new List<Transform>();
            Transform patrolWaypointsParent = GameObject.FindGameObjectWithTag("Patrol Waypoints").transform;

            foreach (Transform pChild in patrolWaypointsParent)
                patrolWaypoints.Add(pChild);
        }

        defaultCharacterLocalscaleX = characterGFX.localScale.x;
        reversedCharacterLocalscaleX = -1f * characterGFX.localScale.x;
        defaultEyesightLocalpositionX = characterEyesight.localPosition.x;
        reversedEyesightLocalpositionX = -1f * characterEyesight.localPosition.x;

        myCollider = this.GetComponent<CircleCollider2D>();

        if (walkSpeed <= 0f)
        {
            defaultWalkSpeed = 400f;
            walkSpeed = defaultWalkSpeed;
        }
        else
            defaultWalkSpeed = walkSpeed;


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
   
    protected void Action()
    {
        if (attackEnabled)
            PerformAction();
    }
    protected abstract void PerformAction();

    protected virtual bool ReachedEndOfPath()
    {
        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (singleTarget == null)
            {
                if (currentWaypointMode == EAIWaypointMode.OneWay)
                {
                    if (currentTarget < targets.Count - 1)
                    {
                        currentTarget++;
                    }
                }
                else if (currentWaypointMode == EAIWaypointMode.Patrol)
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
            }
            reachedEndOfPath = true;
            return reachedEndOfPath;
        }
        else
        {
            reachedEndOfPath = false;
            return reachedEndOfPath;
        }
    }


    private void Movement()
    {
        if (movementEnabled)
        {
            if (path == null)
            {
                return;
            }


            if (climbing)
            {
                if (rb != null)
                {
                    rb.gravityScale = 0f;
                }
            }
            else
            {
                if (rb != null)
                {
                    rb.gravityScale = 1f;
                }
            }

            if (ReachedEndOfPath())
                return;

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * walkSpeed * Time.deltaTime;
            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            Debug.LogFormat("<color=#01751a> Force: {0} </color>", force.x);
            SwitchGFXDirection(force);
        }
    }

    private void Awake()
    {
        SetDefaultValues();

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
 
        InvokeRepeating("UpdateNavigation", 0f, .5f); 
    }

    protected void UpdateNavigation()
    {
        if (seeker.IsDone())
        {
            if (singleTarget != null)
            {
                seeker.StartPath(rb.position, singleTarget.position, OnPathComplete);
            }
            else if (targets.Count != 0)
            {
                seeker.StartPath(rb.position, targets[currentTarget].position, OnPathComplete);
            }
        }

    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }
}
