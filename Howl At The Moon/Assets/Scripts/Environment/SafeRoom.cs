using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeRoom : MonoBehaviour
{
    [SerializeField]bool bWasUsed = false;
    //if an enemy enters you lose.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !bWasUsed)
        {
            Gamemanager.instance.LoseGame();
            bWasUsed = true;
        }
    }
}
