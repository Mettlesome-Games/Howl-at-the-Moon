using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorTrap : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) {
            Attack();
            GetComponent<Collider2D>().enabled = false;
        }
    }

    void Attack() {
        print("Smack!");
    }

}
