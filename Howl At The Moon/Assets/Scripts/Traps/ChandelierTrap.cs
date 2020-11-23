﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Logic for the Chandelier trap
public class ChandelierTrap : MonoBehaviour
{
    public Rigidbody2D Chandelier;
    [SerializeField] private bool bWasUsed = false;

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: check if the werewolf is on fire and the trap has not been used.
        if (collision.CompareTag("Enemy") && !bWasUsed) {
            print("Smash");
            Chandelier.WakeUp();
            GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Trapped;
            bWasUsed = true;
        }
    }

    public void SetParent(Room room)
    {
        transform.parent = room.transform;
    }

    public Vector3 GetLocalPos()
    {
        return transform.localPosition;
    }

    public void SetLocalPos(float x, float y)
    {
        transform.localPosition = new Vector3(x, y, -10);
    }

}
