using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrap : MonoBehaviour
{
    [SerializeField]
    private bool foodPoisoned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       /* if (collision.CompareTag("Servant")) {
            foodPoisoned = true;
        }*/

        if (collision.CompareTag("Enemy") && foodPoisoned) {
            print("Werewolf knockout!!!");
            GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Trapped;
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
