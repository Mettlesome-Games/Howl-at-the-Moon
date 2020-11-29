using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Nathan Hales
//Once Use Trapdoor the works of a hingejoint once an enemy enters the trigger box the connected platform's gravity is changed to 1. 
// after 3 seconds the hingejoint rests using limtis.
public class TrapDoor : MonoBehaviour
{
    [SerializeField] Rigidbody2D connectedPlatform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Open if not already open.
        if (collision.CompareTag("Enemy"))
        {
            connectedPlatform.gravityScale = 1;
            GetComponent<Collider2D>().enabled = false;
            connectedPlatform.GetComponent<HingeJoint2D>().useLimits = false; 
            WerewolfAI ai = collision.gameObject.GetComponent<WerewolfAI>();
            if (ai != null)
            {
                collision.gameObject.GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Trapped;
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            }
            StartCoroutine("HangTimer");
        }
    }
   
    private IEnumerator HangTimer() { 
        yield return new WaitForSeconds(3);
        // After three seconds reset to original position.
        connectedPlatform.GetComponent<HingeJoint2D>().useLimits = true;
    }
}
