using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Sprite roomSprite;
    public bool canSlide;
    public GameObject Chandler;
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
        if (Chandler != null)
        {
            chanChan = Chandler.GetComponent<ChandelierTrap>();
            chanChan.SetParent(this);
            chanChan.SetLocalPos(0, -1);
        }
        Instantiate(Chandler);
    }
}
