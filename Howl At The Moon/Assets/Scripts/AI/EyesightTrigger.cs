using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// An area trigger to act as the enemy's eyesight
/// </summary>
public class EyesightTrigger : MonoBehaviour
{
    private GameObject AIParent;

    void Awake()
    {
        AIParent = this.transform.parent.gameObject; 
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        ServantAI servant = AIParent.GetComponent<ServantAI>();
        WerewolfAI werewolf = AIParent.GetComponent<WerewolfAI>();

        if (collision.CompareTag("Servant") && werewolf != null)
        { 
            werewolf.newState = WerewolfAI.EWerewolfStates.Chasing;
            werewolf.singleTarget = collision.gameObject.transform;
        }
        else if (collision.CompareTag("Enemy") && servant != null)
        {            
            if (servant.hasWolfsbane)
            {
                servant.newState = ServantAI.EServantStates.PresentingWolfsbane;
                servant.singleTarget = collision.gameObject.transform;
            }
            else 
            {
                servant.newState = ServantAI.EServantStates.Running;
            }
        }
        else if (collision.CompareTag("Food Bowl") && servant != null)
        {
            if (servant.canMakeWolfsbane)
            {
                servant.newState = ServantAI.EServantStates.CreatingWolfsbane;
                servant.singleTarget = collision.gameObject.transform;
            }
          
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        WerewolfAI werewolf = AIParent.GetComponent<WerewolfAI>();

        if (collision.CompareTag("Servant") && werewolf != null)
        {
            werewolf.newState = WerewolfAI.EWerewolfStates.Chasing;
            werewolf.singleTarget = collision.gameObject.transform;
        }
    }

}
