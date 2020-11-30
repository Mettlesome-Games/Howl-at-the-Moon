using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Kevin Caton-Largent
/// An area trigger to tell the enemy and servant to teleport
/// V2
/// </summary>
public class StairsTrigger : MonoBehaviour
{
    private Transform teleportPoint;
    public Transform nextLevelTarget;
    public Transform nextWaypointTargets;
    private void Awake()
    {
        teleportPoint = this.gameObject.transform.parent.Find("TP-Point");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ServantAI servant = collision.gameObject.GetComponent<ServantAI>();
        WerewolfAI werewolf = collision.gameObject.GetComponent<WerewolfAI>();
        if (collision.CompareTag("Enemy"))
        {
            werewolf.transform.position = teleportPoint.position;
            werewolf.singleTarget = nextLevelTarget;
            werewolf.levelTarget = nextLevelTarget;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x * -1, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            collision.gameObject.transform.parent.GetComponent<Room>().NPCs.Remove(collision.gameObject);//sometimes object no exist error
        }
        else if (collision.CompareTag("Servant"))
        {
            servant.transform.position = teleportPoint.position;
            servant.singleTarget = nextLevelTarget;
            servant.levelTarget = nextLevelTarget;
            servant.AnalyzePatrolArray(nextWaypointTargets);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x * -1, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            collision.gameObject.transform.parent.GetComponent<Room>().NPCs.Remove(collision.gameObject);//sometimes object no exist error
        }
    }
    
}
