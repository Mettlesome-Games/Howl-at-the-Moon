using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlong : MonoBehaviour
{
    public Transform nextLevelTarget;
    private void Awake()
    {
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ServantAI servant = collision.gameObject.GetComponent<ServantAI>();
        WerewolfAI werewolf = collision.gameObject.GetComponent<WerewolfAI>();
        if (collision.CompareTag("Enemy"))
        {
            werewolf.singleTarget = nextLevelTarget;
            werewolf.levelTarget = nextLevelTarget;
        }
        else if (collision.CompareTag("Servant"))
        {
            servant.singleTarget = nextLevelTarget;
            servant.levelTarget = nextLevelTarget;
        }
    }
}
