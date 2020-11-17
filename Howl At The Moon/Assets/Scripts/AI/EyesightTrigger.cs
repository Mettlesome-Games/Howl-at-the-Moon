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

        if (collision.CompareTag("Servant"))
        {
            AIParent.GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Chasing;
            AIParent.GetComponent<WerewolfAI>().singleTarget = collision.gameObject.transform; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Servant"))
        {
            Debug.Log("what's going on");
            AIParent.GetComponent<WerewolfAI>().newState = AIParent.GetComponent<WerewolfAI>().PreviousStates;
            AIParent.GetComponent<WerewolfAI>().singleTarget = collision.gameObject.transform;
        }
    }

}
