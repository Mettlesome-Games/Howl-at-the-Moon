using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Sprite roomSprite;
    public bool canSlide;
    public GameObject[] traps;
    public Vector3 localP;
    //public string[] hindrances;
    //spublic int[] moonlight;
    public BoxCollider2D floor;
    public GameObject moon;
    public Vector2 moonOffset;
    public Vector2 moonSize;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = roomSprite;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpTraps()
    {
        
    }
}
