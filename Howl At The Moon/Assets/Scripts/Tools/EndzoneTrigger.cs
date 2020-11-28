using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// A trigger that clears an enemy or servant
/// </summary>
public class EndzoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ServantAI servant = collision.gameObject.GetComponent<ServantAI>();
        WerewolfAI werewolf = collision.gameObject.GetComponent<WerewolfAI>();

        if (collision.CompareTag("Servant"))
        {
            if (servant.CurrentState == ServantAI.EServantStates.Running)
                servant.TakeDamage(servant.HP);
        }
        else if (collision.CompareTag("Enemy"))
        {
            if(werewolf.CurrentState != WerewolfAI.EWerewolfStates.Chasing)
            {
                werewolf.TakeDamage(werewolf.HP);
            }
        }    
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        WerewolfAI werewolf = collision.gameObject.GetComponent<WerewolfAI>();
        if (collision.CompareTag("Enemy"))
        {
            if (werewolf.CurrentState != WerewolfAI.EWerewolfStates.Chasing)
            {
                werewolf.TakeDamage(werewolf.HP);
            }
        }
    }
}

