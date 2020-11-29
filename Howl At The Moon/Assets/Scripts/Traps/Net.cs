using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) { 
            //collision.gameObject.transform.parent = this.transform;
            WerewolfAI ai = collision.gameObject.GetComponent<WerewolfAI>();
            if (ai != null)
            {
                ai.TakeDamage(ai.HP);
            }
            this.transform.parent.GetComponent<Room>().NPCs.Remove(collision.gameObject);
            collision.gameObject.transform.parent.GetComponent<Room>().NPCs.Remove(collision.gameObject);
        }
    }
}
