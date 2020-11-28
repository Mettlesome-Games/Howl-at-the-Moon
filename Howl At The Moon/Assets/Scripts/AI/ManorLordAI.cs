using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ManorLordAI : MonoBehaviour
{
    /// <summary>
    ///  The event delegate to subscribe to when to kill the AI
    /// </summary>
    private delegate void DeathEvent();
    private static event DeathEvent OnDeathEvent;

    private float hp;
    public float hpMax;
    public float HP
    {
        get
        {
            return hp;
        }
    }
    public void TakeDamage(float value)
    {
        if (hp - value <= 0.0f)
            hp = 0f;
        else
            hp = hp - value;

        if (hp <= 0f)
            OnDeathEvent?.Invoke();
    }
    public void HealDamage(float value)
    {
        if (hp + value >= hpMax)
            hp = hpMax;
        else
            hp = hp + value;
    }
    private void SetDefaultValues()
    {
        if (hpMax <= 0f)
        {
            hpMax = 1f;
            hp = hpMax;
        }
        else
        {
            hp = hpMax;
        }
    }
    private void OnDeath()
    {
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        MethodBase AwakeMethod = MethodBase.GetCurrentMethod();
        Debug.Log("<color=#4f7d00>Subscribing to OnDeath at function call: " + AwakeMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name + "</color>", this);
        ManorLordAI.OnDeathEvent += OnDeath;

        SetDefaultValues();   
    }
    /// <summary>
    /// Whenever this object is destroyed unsubscribe the death event function which should only occur once per game object
    /// </summary>
    void OnDestroy()
    {
        MethodBase OnDestroyMethod = MethodBase.GetCurrentMethod();
        Debug.Log("<color=#910a00>Unsubscribing to OnDeath at function call: " + OnDestroyMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name + "</color>", this);
        ManorLordAI.OnDeathEvent -= OnDeath;
    }
}
