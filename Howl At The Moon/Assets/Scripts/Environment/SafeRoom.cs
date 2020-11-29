using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeRoom : MonoBehaviour
{
    [SerializeField]bool bWasUsed = false;
    [SerializeField] float LoseDelay = 1f;

    //if an enemy enters you lose.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !bWasUsed)
        {
            StartCoroutine(TimerLose());
            bWasUsed = true;
        }
    }

    IEnumerator TimerLose() {
        yield return new WaitForSeconds(LoseDelay);

        Gamemanager.instance.LoseGame();

    }
}
