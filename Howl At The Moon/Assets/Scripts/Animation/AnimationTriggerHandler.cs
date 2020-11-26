using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// Acts upon animation triggers to assist in animation
/// </summary>
public class AnimationTriggerHandler : MonoBehaviour
{
    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = this.GetComponent<Animator>();
    }

    public void TurnAnimationBoolOff(string toTurnOff)
    {
        myAnimator.SetBool(toTurnOff, false);
    }
}
