using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDisplayCounter : MonoBehaviour
{
    [SerializeField] WaveController GetWaveController;
    
    TMPro.TMP_Text CounterText;
    int EnemyTotal; // sum of all enemys in each wave.

    // Start is called before the first frame update
    void Start()
    {
        CounterText = GetComponent<TMPro.TMP_Text>();
        CounterText.text = "00 / 00";

        GetWaveController = FindObjectOfType<WaveController>(); // find the wave controller
        
        
    }

    // Update is called once per frame
    void Update() {
        if (GetWaveController != null)
        {
            
            CounterText.text = GetWaveController.EnemiesDead + "/" + GetWaveController.TotalEnemiesToBeSpawned;
            if (NoMoreWave()) Gamemanager.instance.WinGame();
        }
    }

    bool NoMoreWave() {
        if ((GetWaveController.CurrentWaveID-1) == GetWaveController.Waves.Length && GetWaveController.TotalEnemiesToBeSpawned == GetWaveController.EnemiesDead) { return true; }
        else  return false;
    } 
}
