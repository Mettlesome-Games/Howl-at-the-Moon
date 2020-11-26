using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// Tells the manor lord to freak out
/// </summary>
public class ManorLordEyesight : MonoBehaviour
{
    private Animator myAnimator;
    private void Awake()
    {
        myAnimator = this.transform.parent.Find("LordGFX").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            myAnimator.SetBool("Panic", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            myAnimator.SetBool("Panic", false);
        }
    }
}
