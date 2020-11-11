using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private const float GRID_HEIGHT = 3f;
    private const float GRID_WIDTH = 3f;

    public GameObject[] grid;
    public Color[] currentRow;
    public Color[] currentCol;

    public Color[] row0 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta};
    public Color[] row1 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    public Color[] row2 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    public Color[] col0 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    public Color[] col1 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    public Color[] col2 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };


    // Start is called before the first frame update
    void Start()
    {
        //DrawColors();
    }

    // Update is called once per frame
    void Update()
    {
        DrawColors();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(currentRow == row0)
            {
                row0 = ShiftUpLeft(row0);
                currentRow = row0;
            }
            if (currentRow == row1)
            {
                row1 = ShiftUpLeft(row1);
                currentRow = row1;
            }
            if (currentRow == row2)
            {
                row2 = ShiftUpLeft(row2);
                currentRow = row2;
            }
            UpdateMainNine(false);
            ThirdGroupMatching(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentRow == row0)
            {
                row0 = ShiftDownRight(row0);
                currentRow = row0;
            }
            if (currentRow == row1)
            {
                row1 = ShiftDownRight(row1);
                currentRow = row1;
            }
            if (currentRow == row2)
            {
                row2 = ShiftDownRight(row2);
                currentRow = row2;
            }
            UpdateMainNine(false);
            ThirdGroupMatching(false);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentCol == col0)
            {
                col0 = ShiftUpLeft(col0);
                currentCol = col0;
            }
            if (currentCol == col1)
            {
                col1 = ShiftUpLeft(col1);
                currentCol = col1;
            }
            if (currentCol == col2)
            {
                col2 = ShiftUpLeft(col2);
                currentCol = col2;
            }
            UpdateMainNine(true);
            ThirdGroupMatching(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentCol == col0)
            {
                col0 = ShiftDownRight(col0);
                currentCol = col0;
            }
            if (currentCol == col1)
            {
                col1 = ShiftDownRight(col1);
                currentCol = col1;
            }
            if (currentCol == col2)
            {
                col2 = ShiftDownRight(col2);
                currentCol = col2;
            }
            UpdateMainNine(true);
            ThirdGroupMatching(true);
        }
    }

    public Color[] ShiftUpLeft(Color[] shifted)//shifts up or left and then calls UpdateMainNine
    {
        Color[] temp = new Color[shifted.Length];
        for(int i = 0; i < shifted.Length; i++)
        {
            if(i == shifted.Length - 1)
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

    public Color[] ShiftDownRight(Color[] shifted)//shifts down or right and then calls UpdateMainNine
    {
        Color[] temp = new Color[shifted.Length];
        for (int i = 0; i < shifted.Length; i++)
        {
            if (i == 0)
            {
                temp[i] = shifted[shifted.Length - 1];
            }
            else
            {
                temp[i] = shifted[i - 1];
            }
        }
        return temp;
    }

    public void UpdateMainNine(bool movedVertical)//makes sure that the elements in the first 3 spots of each array match
    {
        if (movedVertical)
        {
            row0[0] = col0[0];
            row0[1] = col1[0];
            row0[2] = col2[0];
            row1[0] = col0[1];
            row1[1] = col1[1];
            row1[2] = col2[1];
            row2[0] = col0[2];
            row2[1] = col1[2];
            row2[2] = col2[2];
        }
        else
        {
            col0[0] = row0[0];
            col1[0] = row0[1];
            col2[0] = row0[2];
            col0[1] = row1[0];
            col1[1] = row1[1];
            col2[1] = row1[2];
            col0[2] = row2[0];
            col1[2] = row2[1];
            col2[2] = row2[2];
        }
    }

    public void ThirdGroupMatching(bool movedVertical)// this insures that the 3rd grouping of 9 in the rows and columns are the same ([6,7,8] of each)
    {
        if (movedVertical)
        {
            row0[6] = col0[6];
            row0[7] = col1[6];
            row0[8] = col2[6];
            row1[6] = col0[7];
            row1[7] = col1[7];
            row1[8] = col2[7];
            row2[6] = col0[8];
            row2[7] = col1[8];
            row2[8] = col2[8];
        }
        else
        {
            col0[6] = row0[6];
            col1[6] = row0[7];
            col2[6] = row0[8];
            col0[7] = row1[6];
            col1[7] = row1[7];
            col2[7] = row1[8];
            col0[8] = row2[6];
            col1[8] = row2[7];
            col2[8] = row2[8];
        }
    }

    public void DrawColors()
    {
        int i = 0;
        foreach(GameObject tile in grid)
        {
            if(i < 3)
            {
                tile.GetComponent<Image>().color = row0[i];
            }
            else if(i < 6)
            {
                tile.GetComponent<Image>().color = row1[i - 3];
            }
            else
            {
                tile.GetComponent<Image>().color = row2[i - 6];
            }
            

            i++;
        }
    }
}
