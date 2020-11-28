using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Kevin Caton-Largent
/// Controls the spawn behavior of the enemy waves and directs which wave to take place via the attached WaveTimer
/// V2
/// </summary>
public class WaveController : MonoBehaviour
{
    private enum EWaveSystemVersion { AfterDuration = 0, AfterLastEnemyInWave = 1, EnemyIntervalDelay = 2, AfterLastEnemyInWaveWithDelay = 4};
    [SerializeField]
    [Tooltip("4 different spawn variations explained by their names")]
    private EWaveSystemVersion SpawnSystemToggle;

    public uint EnemiesDead { get; private set; }
    public uint EnemiesDeadInWave { get; private set;}

    public uint TotalEnemiesToBeSpawned { get; private set; }
    public uint CurrentWaveID, CurrentSpawnedEnemies, TotalSpawnedEnemies;

    public GameObject EnemyPrefab;

    // Just to make things look pretty
    private GameObject WaveOrganizerContainer;
    private GameObject WaveOrganizer;

    // The clock that controls the frequency of the waves
    private WaveTimer WaveClock;


    // A list of Spawn Points to spawn at
    [Tooltip("Please put all spawn points in the level here they should be located in the Lvl#WaveSpawners game object")]
    public List<Transform> SpawnPoints;

    [System.Serializable]
    public class SingleEnemyData
    {
        public int SpawnId, TypeId;  
    }

    [System.Serializable]
    public class WaveImprovedData
    {
        public SingleEnemyData[] EnemyArrangement;
    }
    [Space(10f)]
    [Header("New Improved Wave Data")]
    public WaveImprovedData[] Waves;
      
    [SerializeField]
    [Space(10f)]
    [Header("Internal values exposed only for debugging")]
    private int CurrentSpawnQuota;
    [SerializeField]
    private int SpawnQuotaGoal;

    [SerializeField]
    [Tooltip("Needs to have the same key dimensions as the spawn boxes you place in the level, otherwise there will be issues\nMake sure to keep the z blank for 2D considering that controls depth placement")]
    private Vector3 SpawnRandomRange;
    private void Awake()
    {
        MethodBase AwakeMethod = MethodBase.GetCurrentMethod();
        Debug.Log("<color=#4f7d00>Subscribing to SpawnEnemies at function call: " + AwakeMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name + "</color>", this);
        WaveTimer.OnSpawnWaveEvent += SpawnEnemies;
        LoadDefaults();
       
        WaveOrganizerContainer = new GameObject();
        WaveOrganizerContainer.name = "Wave Organizer";
        WaveOrganizer = new GameObject();
        WaveOrganizer.name = "Wave ( " + CurrentWaveID + " )";
        WaveOrganizer.transform.parent = WaveOrganizerContainer.transform;

        SpawnEnemies();
    }

    void Update()
    {
        if (TotalSpawnedEnemies < TotalEnemiesToBeSpawned)
        {
            if (SpawnSystemToggle == EWaveSystemVersion.AfterDuration)
            {
                WaveClock.InvokeTimer(0);
            }
            else if (SpawnSystemToggle == EWaveSystemVersion.AfterLastEnemyInWave)
            {
                if (SpawnQuotaGoal == EnemiesDeadInWave)
                {
                    WaveClock.InvokeTimer(0);
                }
            }
            else if (SpawnSystemToggle == EWaveSystemVersion.EnemyIntervalDelay)
            {
                if (CurrentSpawnedEnemies != SpawnQuotaGoal)
                {
                    WaveClock.InvokeTimer(1);
                }
            }
            else if (SpawnSystemToggle == EWaveSystemVersion.AfterLastEnemyInWaveWithDelay)
            {
                if (SpawnQuotaGoal == EnemiesDeadInWave)
                {
                    CurrentSpawnedEnemies = 0;
                    EnemiesDeadInWave = 0;
                    WaveClock.InvokeTimer(0);
                }
                else if (CurrentSpawnedEnemies != SpawnQuotaGoal)
                {
                    WaveClock.InvokeTimer(1);
                }
            }
        }
    }

    private void CalculateTotals()
    {
        MethodBase CalculateTotals = MethodBase.GetCurrentMethod();
        #region Check if there is data inside Waves array
        if (Waves == null)
        {
            Debug.LogError("Enemy wave data is not initialized at function call: " + CalculateTotals.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
        foreach (WaveImprovedData CurrentWave in Waves)
            foreach (SingleEnemyData Enemy in CurrentWave.EnemyArrangement)
                TotalEnemiesToBeSpawned++;     
    }

    /// <summary>
    /// Pulls the current wave quota data from built in wave array
    /// </summary>
    private void InitializeCurrentWaveQuota()
    {

        if (SpawnSystemToggle == EWaveSystemVersion.AfterDuration || SpawnSystemToggle == EWaveSystemVersion.AfterLastEnemyInWave)
        {
            CurrentSpawnQuota = Waves[CurrentWaveID-1].EnemyArrangement.Length;
            SpawnQuotaGoal = CurrentSpawnQuota;
        }
        else if (SpawnSystemToggle == EWaveSystemVersion.EnemyIntervalDelay || SpawnSystemToggle == EWaveSystemVersion.AfterLastEnemyInWaveWithDelay)
        {
            CurrentSpawnQuota = 1;
            SpawnQuotaGoal = Waves[CurrentWaveID - 1].EnemyArrangement.Length;
        }
    }
    /// <summary>
    /// Get the spawnquota, the current spawn id's and enemy arrangement and spawn the appropriate number of enemies
    /// </summary>
    public void SpawnEnemies()
    {
        MethodBase SpawnEnemiesMethod = MethodBase.GetCurrentMethod();
        
        if (TotalSpawnedEnemies < TotalEnemiesToBeSpawned)
        {
            InitializeCurrentWaveQuota();
            
            for (uint CurrentEnemyInQuota = 0; CurrentEnemyInQuota < CurrentSpawnQuota; CurrentEnemyInQuota++)
            {
                // This is used for a random spawn range within a 3d box where the ground is
                //  Vector3 RandomOffset = new Vector3(Random.Range(SpawnRandomRange.x / -2f, SpawnRandomRange.x / 2f), 0f, Random.Range(SpawnRandomRange.z / -2f, SpawnRandomRange.z / 2f));
                Vector3 RandomOffset = new Vector3(Random.Range(SpawnRandomRange.x / -2f, SpawnRandomRange.x / 2f), 0f, 0f);

                // Instantiate enemy at the appropriate spawner, select its type
                int CurrentSpawnIndex = Waves[CurrentWaveID-1].EnemyArrangement[CurrentEnemyInQuota].SpawnId;
                Transform SpawnInThisBox = SpawnPoints[CurrentSpawnIndex];

                GameObject SpawnedEnemy = Instantiate(EnemyPrefab, SpawnInThisBox.position + RandomOffset, Quaternion.identity);
                WerewolfAI EnemyBehaviorControls = SpawnedEnemy.GetComponent<WerewolfAI>();
                SpawnedEnemy.transform.parent = WaveOrganizer.transform;

                EnemyBehaviorControls.masterCommander = this.gameObject.GetComponent<WaveController>();

                EnemyBehaviorControls.MonsterID = TotalSpawnedEnemies + 1;
                EnemyBehaviorControls.WaveID = CurrentWaveID;
                EnemyBehaviorControls.name = EnemyBehaviorControls.AIType.ToString() + " " + EnemyBehaviorControls.MonsterID + " Wave " + EnemyBehaviorControls.WaveID;
                
                EnemyBehaviorControls.movementEnabled = true;
                EnemyBehaviorControls.attackEnabled = true;

                CurrentSpawnedEnemies++;
                TotalSpawnedEnemies++;
            }
            if (CurrentSpawnedEnemies == SpawnQuotaGoal)
            {
                CurrentWaveID++;
                WaveOrganizer = new GameObject();
                WaveOrganizer.name = "Wave ( " + CurrentWaveID + " )";
                WaveOrganizer.transform.parent = WaveOrganizerContainer.transform;
                EnemiesDeadInWave = 0;

                if (SpawnSystemToggle != EWaveSystemVersion.AfterLastEnemyInWaveWithDelay)
                {
                    CurrentSpawnedEnemies = 0;
                }
            }
        }
    }
    public void TickKilledEnemies()
    {
        #region Total Enemies
        if ((EnemiesDead + 1) > TotalEnemiesToBeSpawned)
        {
            EnemiesDead = TotalEnemiesToBeSpawned;
        }
        else
        {
            EnemiesDead++;
        }
        #endregion

        #region Enemies Dead in Current Wave
        EnemiesDeadInWave++;
        #endregion
    }

    /// <summary>
    /// Clears active enemies from the scene this is useful for scene reloads
    /// </summary>
    private void ClearEnemies()
    {
        MethodBase ClearEnemiesMethod = MethodBase.GetCurrentMethod();

        GameObject[] FieldedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (FieldedEnemies.Length <= 0)
        {
            Debug.Log("No enemies to clear at function call: " + ClearEnemiesMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name);
            return;
        }
        foreach(GameObject DestroyThis in FieldedEnemies)
        {
            Destroy(DestroyThis.gameObject);
        }
    }

    /// <summary>
    /// Finds the spawn points in the level and sets up the the spawn point variable
    /// </summary>
    private void FindSpawnPoints()
    {
        MethodBase FindSpawnPointsMethod = MethodBase.GetCurrentMethod();
        GameObject SpawnObj = null;
        SpawnObj = GameObject.FindGameObjectWithTag("Spawnpoints");

        if (SpawnObj == null)
        {
            Debug.LogError("There are no Spawn points in the scene at function call: " + FindSpawnPointsMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name);
        }
        else
        {
            foreach (Transform Spawns in SpawnObj.transform)
                SpawnPoints.Add(Spawns);                        
        }
    }

    /// <summary>
    /// Resets the defaults and is particularly useful for scene reloads
    /// </summary>
    public void ResetDefaults()
    {
        
        ClearEnemies();
        LoadDefaults();
    }

    /// <summary>
    /// Loads the defaults
    /// </summary>
    public void LoadDefaults()
    {
        MethodBase LoadDefaultsMethod = MethodBase.GetCurrentMethod();

        TotalEnemiesToBeSpawned = 0;
        EnemiesDead = 0;
        EnemiesDeadInWave = 0;

        CurrentSpawnedEnemies = 0;
        CurrentWaveID = 1;
        
        SpawnPoints = new List<Transform>();
        FindSpawnPoints();
        CalculateTotals();

        WaveClock = this.gameObject.GetComponent<WaveTimer>();

        #region Check if WaveClock exists
        if (WaveClock == null)
        {
            Debug.LogError("Can't load the WaveClock at function call: " + LoadDefaultsMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
    }

    /// <summary>
    /// Whenever this object is destroyed unsubscribe the spawn enemies function from the Wave Timer Event which should only occur on scene reloads/loads
    /// </summary>
    void OnDestroy()
    {
        MethodBase OnDestroyMethod = MethodBase.GetCurrentMethod();
        Debug.Log("<color=#910a00>Unsubscribing to SpawnEnemies at function call: " + OnDestroyMethod.Name + " at script " + this.GetType().Name + " on the GameObject " + this.gameObject.name + "</color>", this);
        WaveTimer.OnSpawnWaveEvent -= SpawnEnemies;
    }
}
