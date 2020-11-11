using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private float[] array = { 0, 1, 2, 3 };

    // Start is called before the first frame update
    void Start()
    {
        foreach (float c in array)
        {
            print(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            array = ShiftUpLeft(array);
            foreach (float c in array)
            {
                print(c);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            array = ShiftDownRight(array);
            foreach (float c in array)
            {
                //print(c);
            }
        }
    }

    public float[] ShiftUpLeft(float[] shifted)//shifts up or left and then calls UpdateMainNine
    {
        float[] temp = shifted;
        for (int i = 0; i < shifted.Length; i++)
        {
            if (i == shifted.Length - 1)
            {
                temp[i] = shifted[0];
            }
            else
            {
                temp[i] = shifted[i + 1];
            }
        }
        return temp;
    }

    public float[] ShiftDownRight(float[] shifted)//shifts down or right and then calls UpdateMainNine
    {
        float[] temp = new float[4];// = shifted;
        foreach (float c in array)
        {
            //print(c);
        }
        for (int i = 0; i < shifted.Length; i++)
        {
            if (i == 0)
            {
                temp[i] = shifted[shifted.Length - 1];
                print("i is zero " + temp[i]);
            }
            else
            {
                print("i is not zero " + shifted[i - 1]);
            }
        }
        return temp;
    }
}
