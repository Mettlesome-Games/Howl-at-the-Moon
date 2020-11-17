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

    public float walkSpeed;
    public float nextWaypointDistance;

    public bool climbing = false;

    public Transform characterGFX;
    public Transform characterEyesight;

    protected float hp;

    public float HP
    {
        get 
        {
            return hp;
        }
        set
        {
            if (hp - value < 0.0f)
                hp = 0f;
            else
                hp -= value;
        }
    }

    protected Path path;

    protected float defaultWalkSpeed;

    protected int currentWaypoint = 0;
    protected int currentTarget = 0;
    protected bool reachedEndOfPath = false;

    protected Seeker seeker;
    protected Rigidbody2D rb;

    protected float reversedCharacterX;
    protected float reversedEyesightX;
    protected virtual void SetDefaultValues()
    {
        reversedCharacterX = -1 * characterGFX.localScale.x;
        reversedEyesightX = -1 * characterEyesight.localScale.x;

        if (walkSpeed <= 0f)
        {
            defaultWalkSpeed = 400f;
            walkSpeed = defaultWalkSpeed;
        }
        else
            defaultWalkSpeed = walkSpeed;
       
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            if (targets.Count > 0 && currentTarget < targets.Count - 1)
                currentTarget++;

            currentWaypoint = 0;
        }
    }
    void SwitchGFXDirection(Vector2 inForce)
    {
        if (rb.velocity.x >= 0.01f)
        {
            characterGFX.localScale = new Vector3(characterGFX.localScale.x, characterGFX.localScale.y, characterGFX.localScale.z);
            characterEyesight.localScale = new Vector3(characterEyesight.localScale.x, characterEyesight.localScale.y, characterEyesight.localScale.z);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            characterGFX.localScale = new Vector3(reversedCharacterX, characterGFX.localScale.y, characterGFX.localScale.z);
            characterEyesight.localScale = new Vector3(reversedEyesightX, characterEyesight.localScale.y, characterEyesight.localScale.z);
        }
    }
   
    protected abstract void Action();
    protected virtual void Movement()
    {
        if (path == null)
            return;

        if (climbing)
        {
            if (rb != null)
            {
                rb.gravityScale = 0;
            }
        }
        else
        {
            if (rb != null)
            {
                rb.gravityScale = 1;
            }
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * walkSpeed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        SwitchGFXDirection(force);
    }

    protected virtual void Awake()
    {
        SetDefaultValues();

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
 
        InvokeRepeating("UpdateNavigation", 0f, .5f); 
    }

    protected virtual void UpdateNavigation()
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

    protected virtual void FixedUpdate()
    {
        Movement();
    }
}
