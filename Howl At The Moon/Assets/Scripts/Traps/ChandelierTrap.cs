using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Logic for the Chandelier trap
public class ChandelierTrap : MonoBehaviour
{
    public Rigidbody2D Chandelier;
    [SerializeField] private bool bWasUsed = false;

    private void Start()
    {
        Chandelier.simulated = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: check if the werewolf is on fire and the trap has not been used.
        if (collision.CompareTag("Enemy") && !bWasUsed) {
            print(collision.name + "");
            Chandelier.simulated = true;
            Chandelier.WakeUp();
            WerewolfAI ai = collision.gameObject.GetComponent<WerewolfAI>();
            if (ai != null) 
            { 
                //collision.gameObject.GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.;
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            }
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
