using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// An area trigger to trigger the cursed state in the enemy
/// </summary>
public class MoonlightTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Cursed;
        }
    }
}
