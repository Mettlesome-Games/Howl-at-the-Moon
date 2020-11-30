using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// Controls the ticking down of wave timers and fires an event that can be used to trigger spawns
/// V2
/// </summary>
public class WaveTimer : Timer
{
    [Space(10)]
    [Header("Boolean Wave Checks")]
    [SerializeField]
    private bool waveCooldownActive;
    [SerializeField]
    private bool enemySpawnCooldownActive;
    [SerializeField]
    private bool spawnEnemies;

    [Space(10)]
    [Header("Timers")]
    [SerializeField]
    private float waveCountdownTimer;
    [SerializeField]
    private float enemySpawnCountdownTimer;

    [Space(10)]
    [Header("WaveController")]

    [Space(10)]
    [Header("Default Durations ")]
    [SerializeField]
    private float defaultWaveDuration;
    [SerializeField]
    private float defaultEnemySpawnInterval;


    /// <summary>
    ///  The event delegate to subscribe to when to spawn a wave
    /// </summary>
    public delegate void SpawnWaveEvent();
    public static event SpawnWaveEvent OnSpawnWaveEvent;

    /// <summary>
    /// This function starts the appropriate wave timer countdown
    /// <param name="TimerKey">The signature of the timer to invoke, 0 = WaveTimer, 1 = EnemySpawnDelay </param>
    public override void InvokeTimer(int TimerKey)
    {
        if (TimerKey == 0 && !waveCooldownActive)
        {
            waveCooldownActive = true;
            waveCountdownTimer = defaultWaveDuration;
        }
        else if (TimerKey == 1 && !enemySpawnCooldownActive)
        {
            enemySpawnCooldownActive = true;
            enemySpawnCountdownTimer = defaultEnemySpawnInterval;
        }
    }
    /// <summary>
    /// Advances the countdowns that are turned on
    /// </summary>
    public override void TickCountdowns()
    {
        if (waveCooldownActive)
        {
            waveCountdownTimer -= Time.deltaTime;
            if (waveCountdownTimer <= 0f)
            {
                waveCountdownTimer = defaultWaveDuration;
                waveCooldownActive = false;
                spawnEnemies = true;
            }
        }
        else if (enemySpawnCooldownActive)
        {
            enemySpawnCountdownTimer -= Time.deltaTime;
            if (enemySpawnCountdownTimer <= 0f)
            {
                enemySpawnCountdownTimer = defaultEnemySpawnInterval;
                enemySpawnCooldownActive = false;
                spawnEnemies = true;
            }
        }
        if (spawnEnemies)
        {
            spawnEnemies = false;
            OnSpawnWaveEvent?.Invoke();
        }
    }

    /// <summary>
    /// Until a ui is setup/designed this will serve as a way to see the countdowns in action
    /// </summary>
    public override void PrintCountdowns()
    {
        if (Printing == EPrintingAvailability.Yes)
        {
            string ToPrint = this.gameObject.name;
            if (waveCooldownActive)
            {
                ToPrint += " Next Wave in: " + waveCountdownTimer;
                Debug.LogFormat("Whats going on here");

                if (!ToPrint.Equals(this.gameObject.name))
                {
                    Debug.Log(ToPrint);
                }
            }
            if (waveCooldownActive && enemySpawnCooldownActive)
                ToPrint += "\n";
            if (enemySpawnCooldownActive)
            {
                ToPrint += " Next Enemy in: " + enemySpawnCountdownTimer;
                if (!ToPrint.Equals(this.gameObject.name))
                {
                    Debug.Log(ToPrint);
                }
            }

        }
    }

    /// <summary>
    /// Initialize the defaults for the cooldowns
    /// </summary>
    public override void InitializeDefaults()
    {
        MethodBase InitializeDefaultsMethod = MethodBase.GetCurrentMethod();
        // Initialize default booleans
        waveCooldownActive = false;
        enemySpawnCooldownActive = false;
        spawnEnemies = false;
        if (defaultWaveDuration <= 0f)
        {
            defaultWaveDuration = 20f;
        }
        if (defaultEnemySpawnInterval <= 0f)
        {
            defaultEnemySpawnInterval = 5f;
        }
        waveCountdownTimer = defaultWaveDuration;
        enemySpawnCountdownTimer = defaultEnemySpawnInterval;
    }

    protected override void Update()
    {
        if (waveCooldownActive || enemySpawnCooldownActive)
        {
            base.Update();
        }
    }
}
