using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Pathfinding;
/// <summary>
/// Author: Kevin Caton-Largent
/// Parent AI
/// v1
/// </summary>

abstract public class AIV1 : MonoBehaviour
{
    public Transform singleTarget;
    public List<Transform> targets;

    [SerializeField]
    [Space(10f)]
    [Header("Internal values exposed only for debugging")]
    protected List<Transform> patrolWaypoints, levelWaypoints;
    public enum EAIWaypointsEditing { AutoSearchLevel = 0, ManualAlotment = 1}
    public EAIWaypointsEditing CurrentWaypointEditingMode = EAIWaypointsEditing.AutoSearchLevel;

    public enum EAISelection {Werewolf = 0, Servant = 1} 
    public EAISelection AIType;

    public bool revalulatePathing = false;
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
    protected BoxCollider2D myCollider;

    protected Animator myAnimator;

    /// <summary>
    ///  The event delegate to subscribe to when to kill the AI
    /// </summary>
    protected delegate void DeathEvent();
    protected static event DeathEvent OnDeathEvent;

    /// <summary>
    ///  The event delegate to subscribe to when an AI moves vertically with a room
    /// </summary>
    /*public delegate void VerticalRoomMoveEvent();
    public static event VerticalRoomMoveEvent OnVerticalRoomMoveEvent;*/
    protected virtual void Awake()
    {
        MethodBase AwakeMethod = MethodBase.GetCurrentMethod();
        /*Debug.Log("<color=#4f7d00>Subscribing to UpdateToNearestTarget at function call: " + AwakeMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name + "</color>", this);
        GridManager.OnVerticalRoomMoveEvent += UpdateToNearestTarget;*/

        SetDefaultValues();

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdateNavigation", 0f, .5f);
    }

    public virtual void TakeDamage(float value)
    {

        if (hp - value <= 0.0f)
            hp = 0f;
        else
            hp = hp - value;

        if (hp <= 0f)
        {
            //OnDeathEvent?.Invoke();
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
        if (CurrentWaypointEditingMode == EAIWaypointsEditing.AutoSearchLevel)
        {
            levelWaypoints = new List<Transform>();
            Transform levelWaypointsParent = GameObject.FindGameObjectWithTag("Level Waypoints").transform;
            foreach (Transform lChild in levelWaypointsParent)
                levelWaypoints.Add(lChild);

            /*
            patrolWaypoints = new List<Transform>();
            Transform patrolWaypointsParent = GameObject.FindGameObjectWithTag("Patrol Waypoints").transform;

            foreach (Transform pChild in patrolWaypointsParent)
                patrolWaypoints.Add(pChild);*/
        }

        hpMax = 1f;
        hp = hpMax;

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

    protected void UpdateCurrentTarget()
    {
        if (singleTarget == null)
        {
            if (currentWaypointMode == EAIWaypointMode.OneWay)
            {
                if (currentTarget < targets.Count - 1)
                {
                    currentTarget++;
                    revalulatePathing = true;
                }
            }
            else if (currentWaypointMode == EAIWaypointMode.Patrol)
            { 
                if (currentTarget < targets.Count - 1 && !reachedEndOfPatrol)
                {
                    currentTarget++;
                    revalulatePathing = true;
                }
                else
                {
                    reachedEndOfPatrol = true;
                    currentTarget--;

                    revalulatePathing = true;

                    if (currentTarget == 0)
                        reachedEndOfPatrol = false;
                }
               
            }
        }
    }

    protected bool ReachedEndOfPath()
    {
        //Debug.LogFormat("CurrentWaypoint {0}, path.vectorPath.Count {1} what is this object {2}", currentWaypoint, path.vectorPath.Count, this.gameObject.name);
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
        }
        else
        {
            reachedEndOfPath = false;
        }
        return reachedEndOfPath;
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
            {
                UpdateCurrentTarget();
                return;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * walkSpeed * Time.deltaTime;
            force.y = 0f;
            rb.AddForce(force);

            myAnimator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            //Debug.LogFormat("<color=#01751a> Force: {0} </color>", force.x);
            SwitchGFXDirection(force);
        }
    }


    public void UpdateToNearestTarget()
    {
        int i = 0, targetIndex = -1;
        float shortestDistance = Mathf.Infinity, currentDistance = 0f;
        Transform candidate = null; 

        if (currentWaypointMode == EAIWaypointMode.OneWay)
        {
         
            foreach (Transform target in targets)
            {
                currentDistance = Vector2.Distance(rb.position, target.position);
                
                if (currentDistance < shortestDistance)
                {
                    targetIndex = i;
                    shortestDistance = currentDistance;
                }
                i++;

            }
            currentTarget = targetIndex;
        }
        else if (currentWaypointMode == EAIWaypointMode.Patrol)
        {
            GameObject[] possiblePatrolWaypoints = GameObject.FindGameObjectsWithTag("Patrol Waypoints");
            foreach (GameObject patrolWaypointsArr in possiblePatrolWaypoints)
            {
                Transform patrolWaypointsParent = patrolWaypointsArr.transform;
                i = 0;
                foreach (Transform pChild in patrolWaypointsParent)
                {
                    currentDistance = Vector2.Distance(rb.position, pChild.position);
                   
                    if (currentDistance < shortestDistance)
                    {
                        targetIndex = i;
                        shortestDistance = currentDistance;
                        candidate = patrolWaypointsParent;
                    }
                    i++;
                }
            }
            if (targetIndex != -1)
            {
                patrolWaypoints.Clear();
                foreach (Transform cChild in candidate)
                {
                    patrolWaypoints.Add(cChild);
                }
                currentTarget = targetIndex;
            }

        }
        revalulatePathing = true;

    }

    private void EvaluateDistanceOfCurrentWaypoint ()
    {
        int waypointIndex = 0, targetIndex = -1;
        float shortestDistance = Mathf.Infinity, currentDistance = 0f; 
        
        foreach (Vector3 waypoint in path.vectorPath)
        {
            currentDistance = Vector2.Distance(rb.position, waypoint);

            if (currentDistance < shortestDistance)
            {
                targetIndex = waypointIndex;
                shortestDistance = currentDistance;
            }
            waypointIndex++;

        }
        currentWaypoint = targetIndex;
    }
    protected virtual void UpdateNavigation()
    {
        MethodBase UpdateNavigationMethod = MethodBase.GetCurrentMethod();

        if (seeker.IsDone())
        {
            if (singleTarget != null)
            {
                seeker.StartPath(rb.position, singleTarget.position, OnPathComplete);
            }
            else if (targets.Count != 0)
            {
                if (revalulatePathing)
                {
                    //Debug.LogFormat("<color=#7e00a1> Before:EvaluateDistanceOfCurrentWaypoint() CurrentWaypoint: {0}, VectorPath.Count: {1} </color>", currentWaypoint, path.vectorPath.Count);
                    EvaluateDistanceOfCurrentWaypoint();
                    //Debug.LogFormat("<color=#f5aa42> After:EvaluateDistanceOfCurrentWaypoint() CurrentWaypoint: {0}, VectorPath.Count: {1} </color>", currentWaypoint, path.vectorPath.Count);
                    revalulatePathing = false;
                }    
                seeker.StartPath(rb.position, targets[currentTarget].position, OnPathComplete);
            }
        }

    }

    public void MoveToNextNavigation()
    {
        UpdateCurrentTarget();
        revalulatePathing = true;
    }
    protected virtual void OnDeath()
    {
        Destroy(this.gameObject);
    }
    protected void OnPathComplete(Path p)
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

    /// <summary>
    /// Whenever this object is destroyed unsubscribe the death event function which should only occur once per game object
    /// </summary>
    protected virtual void OnDestroy()
    {
        MethodBase OnDestroyMethod = MethodBase.GetCurrentMethod();
        /*Debug.Log("<color=#910a00>Unsubscribing to UpdateToNearestTarget at function call: " + OnDestroyMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name + "</color>", this);
        GridManager.OnVerticalRoomMoveEvent -= UpdateToNearestTarget;*/
    }
}

