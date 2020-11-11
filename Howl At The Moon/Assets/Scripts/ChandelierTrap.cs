using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Logic for the Chandelier trap
public class ChandelierTrap : MonoBehaviour
{
    public Rigidbody2D Chandelier;
    [SerializeField] private bool bWasUsed = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: check if the werewolf is on fire and the trap has not been used.
        if (collision.CompareTag("Enemy")) {
            print("Smash");
            Chandelier.WakeUp();
        }
    }
}
