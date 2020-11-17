using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// An area trigger to tell the enemy to use stairs
/// </summary>
public class StairsTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<WerewolfAI>().climbing = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<WerewolfAI>().climbing = false;
        }
    }

}
