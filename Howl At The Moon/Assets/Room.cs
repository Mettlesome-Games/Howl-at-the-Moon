using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Sprite roomSprite;
    public bool canSlide;
    public GameObject[] traps;
    public string[] hindrances;
    public int[] moonlight;
    private ChandelierTrap chanChan;
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
