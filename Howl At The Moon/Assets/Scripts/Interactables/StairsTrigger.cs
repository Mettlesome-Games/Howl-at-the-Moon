using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// An area trigger to tell the enemy and servant to use stairs
/// </summary>
public class StairsTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<WerewolfAI>().climbing = true;
        }
        else if (collision.CompareTag("Servant"))
        {
            collision.gameObject.GetComponent<ServantAI>().climbing = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {    
            collision.gameObject.GetComponent<WerewolfAI>().climbing = false;
        }
        else if (collision.CompareTag("Servant"))
        {
            collision.gameObject.GetComponent<ServantAI>().climbing = false;
        }

    }

}
