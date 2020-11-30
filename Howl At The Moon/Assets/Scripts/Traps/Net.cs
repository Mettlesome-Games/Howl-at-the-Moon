using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] float cooldownTime = 2f;
    [SerializeField] GameObject bounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) { 
            //collision.gameObject.transform.parent = this.transform;
            WerewolfAI ai = collision.gameObject.GetComponent<WerewolfAI>();
            if (ai != null)
            {
                //this.transform.parent.GetComponent<Room>().NPCs.Remove(collision.gameObject);
                collision.gameObject.transform.parent.GetComponent<Room>().NPCs.Remove(collision.gameObject);
                ai.TakeDamage(ai.HP);
                StartCoroutine(TrapCooldown());
            }
        }
    }

    IEnumerator TrapCooldown() {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        bounds.SetActive(false);

        yield return new WaitForSeconds(cooldownTime);

        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        bounds.SetActive(true);

    }
}
