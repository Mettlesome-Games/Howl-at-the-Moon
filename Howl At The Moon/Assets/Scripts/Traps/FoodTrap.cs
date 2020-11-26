using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrap : MonoBehaviour
{
    [SerializeField]
    private bool isfoodPoisoned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Servant") && !isfoodPoisoned) {
            isfoodPoisoned = true;
        }

        if (collision.CompareTag("Enemy") && isfoodPoisoned) {
            print("Werewolf knockout!!!");
            GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Trapped;
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
