using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Sprite roomSprite;
    public bool canSlide;
    public int[] trapIDs;
    public GameObject[] traps;
    public Vector3[] localP;
    //public string[] hindrances;
    //spublic int[] moonlight;
    public BoxCollider2D floor;
    public BoxCollider2D floor2;
    public GameObject moon;
    public Vector2 moonOffset;
    public Vector2 moonSize;
    public List<GameObject> NPCs;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = roomSprite;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(collision.gameObject.GetComponent<WerewolfAI>().CurrentState != WerewolfAI.EWerewolfStates.Trapped)
            {
                collision.gameObject.transform.parent = this.transform;
                NPCs.Add(collision.gameObject);
            }
            else if (collision.CompareTag("Servant"))
            {
                collision.gameObject.transform.parent = this.transform;
                NPCs.Add(collision.gameObject);
            }
            //collision.gameObject.GetComponent<WerewolfAI>().newState = WerewolfAI.EWerewolfStates.Trapped;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Servant"))
        {
            if (NPCs.Contains(collision.gameObject))
            {
                NPCs.Remove(collision.gameObject);
                print(collision.gameObject.name + "has left");
            }
        }
    }
}
