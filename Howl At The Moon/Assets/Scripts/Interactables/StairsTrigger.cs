using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// An area trigger to tell the enemy and servant to teleport
/// </summary>
public class StairsTrigger : MonoBehaviour
{
    private Transform teleportPoint;
    private void Awake()
    {
        teleportPoint = this.gameObject.transform.parent.Find("TP-Point");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ServantAI servant = collision.gameObject.GetComponent<ServantAI>();
        WerewolfAI werewolf = collision.gameObject.GetComponent<WerewolfAI>();
        if (collision.CompareTag("Enemy"))
        {
            werewolf.transform.position = teleportPoint.position;
            werewolf.MoveToNextNavigation();
        }
        else if (collision.CompareTag("Servant"))
        {
            servant.transform.position = teleportPoint.position;
            servant.MoveToNextNavigation();
        }
    }
    
}
