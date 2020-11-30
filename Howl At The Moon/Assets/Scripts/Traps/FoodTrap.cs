using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodTrap : MonoBehaviour
{
    [SerializeField]
    private bool isfoodPoisoned = false;
    private Animator wolfbaneAnimator;
    private void Awake()
    {
        wolfbaneAnimator = this.transform.Find("WolfsbaneSprite").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Servant") && !isfoodPoisoned)
        {
            isfoodPoisoned = true;
            collision.gameObject.GetComponent<ServantAI>().canMakeWolfsbane = true;
            wolfbaneAnimator.SetBool("Poisoned", true);
            this.GetComponent<Collider2D>().enabled = true;
            Array.Clear(this.transform.parent.GetComponent<Room>().traps, 0, 1);
            this.transform.parent = collision.gameObject.transform.Find("FoodBowlPlacement");
            this.transform.position = collision.gameObject.transform.position;
        }

        
        /*if (collision.CompareTag("Enemy") && isfoodPoisoned) { //this works and keeps the wolf trapped and follows room
            print("Werewolf knockout!!!");
            collision.gameObject.transform.parent = this.transform;
            collision.gameObject.GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Trapped;
            collision.gameObject.transform.Rotate(0, 0, 90);
            this.transform.parent.GetComponent<Room>().NPCs.Remove(collision.gameObject);
            //collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<Collider2D>().enabled = false;
        }*/
    }
}
