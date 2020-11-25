﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    //unused consts
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private const float GRID_HEIGHT = 3f;
    private const float GRID_WIDTH = 3f;
    

    //keep track of the current row and col so we know which ones to slide
    [HideInInspector] public Room[] currentRow;
    [HideInInspector] public Room[] currentCol;

    //testing adding in rooms to replace colors(map will need a new way to represent things)
    public Room[] testRow1 = new Room[6];
    public Room[] testRow2 = new Room[6];
    public Room[] testRow3 = new Room[6];
    public Room[] testCol1 = new Room[6];
    public Room[] testCol2 = new Room[6];
    public Room[] testCol3 = new Room[6];
    //The rows and cols of colors - to be rooms later
    Color[] row0 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    Color[] row1 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    Color[] row2 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    Color[] col0 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    Color[] col1 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    Color[] col2 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    //The grid panels by row and col ( index 1 - 3 is the main visible grid; 0 and 5 are previews to other rooms necessary for clean sliding)
    public List<GameObject> gridPanelsRow0;
    public List<GameObject> gridPanelsRow1;
    public List<GameObject> gridPanelsRow2;
    public List<GameObject> gridPanelsCol0;
    public List<GameObject> gridPanelsCol1;
    public List<GameObject> gridPanelsCol2;
    //Where to store the original positions of the rows and cols of panels
    [HideInInspector] public List<Vector3> originalPositionsRow0 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsRow1 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsRow2 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsCol0 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsCol1 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsCol2 = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {

 
        //save the original positions of all the grid panels so we can reset them when necessary
        SetOriginalPos(gridPanelsRow0, originalPositionsRow0);
        SetOriginalPos(gridPanelsRow1, originalPositionsRow1);
        SetOriginalPos(gridPanelsRow2, originalPositionsRow2);
        SetOriginalPos(gridPanelsCol0, originalPositionsCol0);
        SetOriginalPos(gridPanelsCol1, originalPositionsCol1);
        SetOriginalPos(gridPanelsCol2, originalPositionsCol2);

        UpdateMainNine(false);
        SecondGroupMatching(false);
        //set up traps in rows not columns. doing both will duplicate objects (hardcoded but can't waste time on efficiency now)
        SetUpTraps(testRow1, gridPanelsRow0);
        SetUpTraps(testRow2, gridPanelsRow1);
        SetUpTraps(testRow3, gridPanelsRow2);
        //SetUpTraps(testCol1, gridPanelsCol0);
        //SetUpTraps(testCol2, gridPanelsCol1);
        //SetUpTraps(testCol3, gridPanelsCol2);
        //gridPanelsRow0[1].GetComponent<Room>().traps = new GameObject[testRow1[0].GetComponent<Room>().traps.Length];
        //gridPanelsRow0[1].GetComponent<Room>().traps[0] = Instantiate<GameObject>(testRow1[0].GetComponent<Room>().traps[0]);
        //testRow1[0].GetComponent<Room>().traps = gridPanelsRow0[1].GetComponent<Room>().traps;
        //gridPanelsRow0[1].GetComponent<Room>().traps[0].transform.parent = gridPanelsRow0[1].transform;
        //gridPanelsRow0[1].GetComponent<Room>().traps[0].transform.position = gridPanelsRow0[1].GetComponent<Room>().traps[0].transform.parent.transform.position + new Vector3(0,.35f,0);
        
        //set up this one separate, because when you setup the whole column it creates duplicate of any trap already created in the rows. 
        //There is actually still one square that hasn't had traps set up (below 7 or BM) but that's fine. Just don't put traps there 
        int i = 0;
        gridPanelsCol1[0].GetComponent<Room>().traps = new GameObject[testCol2[testCol2.Length-1].GetComponent<Room>().traps.Length];
        foreach(GameObject trap in testCol2[testCol2.Length - 1].GetComponent<Room>().traps)
        {
            gridPanelsCol1[0].GetComponent<Room>().traps[i] = Instantiate<GameObject>(trap);
            gridPanelsCol1[0].GetComponent<Room>().traps[i].transform.parent = gridPanelsCol1[0].transform;
            gridPanelsCol1[0].GetComponent<Room>().traps[i].transform.position = gridPanelsCol1[0].transform.position;// GetComponent<Room>().traps[i].transform.parent.transform.position;
            gridPanelsCol1[0].GetComponent<Room>().traps[i].transform.localScale = trap.transform.localScale;
            i++;
        }
        testCol2[testCol2.Length - 1].GetComponent<Room>().traps = gridPanelsCol1[0].GetComponent<Room>().traps;
    }

    // Update is called once per frame
    void Update()
    {
        //DrawColors();
        DrawRooms(); //draw the rooms continually(maybe should only call once something changes...) --will test
    }

    //shifts up for cols or left for rows
    //just adjusts the data in one row
    //needs to be adjusted in overlapping rows after
    public Room[] ShiftUpLeft(Room[] shifted)
    {
        Room[] temp = new Room[shifted.Length];
        /*for (int i = 0; i < shifted.Length; i++)
        {
            if (i == shifted.Length - 1)
            {
                temp[i] = shifted[0];
            }
            else
            {
                temp[i] = shifted[i + 1];
            }
        }*/
        //with unmovable tiles
        int saved_index = -10;
        for(int i = 0; i < shifted.Length; i++)
        {
            if (!shifted[i].canSlide)
            {
                saved_index = i == 0 ? shifted.Length : i;
                //saved_index = i;
                temp[i] = shifted[i];
            }
            else if(i+1 == saved_index)
            {
                temp[i] = i + 1 >= shifted.Length ? shifted[1] : shifted[i + 2];
            }
            else if(i == shifted.Length - 1)
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

    //shifts down for cols or right for rows
    //just adjusts the data in one row
    //needs to be adjusted in overlapping rows after
    public Room[] ShiftDownRight(Room[] shifted)
    {
        Room[] temp = new Room[shifted.Length];
        /*for (int i = 0; i < shifted.Length; i++)
        {
            if (i == 0)
            {
                temp[i] = shifted[shifted.Length - 1];
            }
            else
            {
                temp[i] = shifted[i - 1];
            }
        }*/
        int saved_index = -10;
        for (int i = 0; i < shifted.Length; i++)
        {
            if (!shifted[i].canSlide)
            {
                saved_index = i == 0 ? 0 : i;
                //saved_index = i;
                temp[i] = shifted[i];
            }
            else if (i - 1 == saved_index)
            {
                temp[i] = i - 1 == 0 ? shifted[shifted.Length-1] : shifted[i - 2];
            }
            else if (i == 0)
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

    //makes sure that the elements in the first 3 spots of each array match
    public void UpdateMainNine(bool movedVertical)
    {
        if (movedVertical)//if you moved a col set the rows
        {
            testRow1[0] = testCol1[0];  
            testRow1[1] = testCol2[0];  //            |             |
            testRow1[2] = testCol3[0];  //r1[0],c1[0] | r1[1],c2[0] | r1[2],c3[0]
            testRow2[0] = testCol1[1];  //_______________________________________
            testRow2[1] = testCol2[1];  //
            testRow2[2] = testCol3[1];  //r2[0],c1[1] | r2[1],c2[1] | r2[2],c3[1]  
            testRow3[0] = testCol1[2];  //_______________________________________
            testRow3[1] = testCol2[2];  //r3[0],c1[2] | r3[1],c2[2] | r3[2],c3[2]
            testRow3[2] = testCol3[2];  //            |             |
        }
        else// if you moved a row set the cols
        {
            testCol1[0] = testRow1[0];
            testCol2[0] = testRow1[1];
            testCol3[0] = testRow1[2];
            testCol1[1] = testRow2[0];
            testCol2[1] = testRow2[1];
            testCol3[1] = testRow2[2];
            testCol1[2] = testRow3[0];
            testCol2[2] = testRow3[1];
            testCol3[2] = testRow3[2];
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
    //works the same as the main nine but for different parts of the arrays
    public void SecondGroupMatching(bool movedVertical)// this insures that the 3rd grouping of 9 in the rows and columns are the same ([6,7,8] of each)
    {
        if (movedVertical)//if you moved a col set the rows
        {
            testRow1[3] = testCol1[3];
            testRow1[4] = testCol2[3];
            testRow1[5] = testCol3[3];
            testRow2[3] = testCol1[4];
            testRow2[4] = testCol2[4];
            testRow2[5] = testCol3[4];
            testRow3[3] = testCol1[5];
            testRow3[4] = testCol2[5];
            testRow3[5] = testCol3[5];
        }
        else// if you moved a row set the cols
        {
            testCol1[3] = testRow1[3];
            testCol2[3] = testRow1[4];
            testCol3[3] = testRow1[5];
            testCol1[4] = testRow2[3];
            testCol2[4] = testRow2[4];
            testCol3[4] = testRow2[5];
            testCol1[5] = testRow3[3];
            testCol2[5] = testRow3[4];
            testCol3[5] = testRow3[5];
        }
    }
    public void DrawColors()
    {
        DrawRow(gridPanelsRow0, row0);
        DrawRow(gridPanelsRow1, row1);
        DrawRow(gridPanelsRow2, row2);
        DrawRow(gridPanelsCol0, col0);
        DrawRow(gridPanelsCol1, col1);
        DrawRow(gridPanelsCol2, col2);
    }
    public void DrawRooms()
    {
        //draw each row of the grid
        DrawRoomRow(gridPanelsRow0, testRow1);
        DrawRoomRow(gridPanelsRow1, testRow2);
        DrawRoomRow(gridPanelsRow2, testRow3);
        //need to draw the cols too because we need to draw the preview panels
        DrawRoomRow(gridPanelsCol0, testCol1);
        DrawRoomRow(gridPanelsCol1, testCol2);
        DrawRoomRow(gridPanelsCol2, testCol3);
    }
    //color each of the tiles in each row
    //index 0 will be the last value in the Color/Room array
    //index 1-4 should be the same as its counterpart in the Color/Room array -1
    public void DrawRow(List<GameObject> panelRow, Color[] rowSource)
    {
        int i = 0;
        foreach (GameObject tile in panelRow)
        {
            if (i == 0)
            {
                tile.GetComponent<Image>().color = rowSource[rowSource.Length - 1];
            }
            else
            {
                tile.GetComponent<Image>().color = rowSource[i - 1];
            }
            i++;
        }
    }
    //draw each of the tiles in each row
    //index 0 will be the last value in the Room array
    //index 1-4 should be the same as its counterpart in the Room array -1
    public void DrawRoomRow(List<GameObject> panelRow, Room[] rowSource)
    {
        int i = 0;
        foreach (GameObject tile in panelRow)
        {
            if (i == 0)
            {
                tile.GetComponent<SpriteRenderer>().sprite = rowSource[rowSource.Length - 1].roomSprite;
                tile.GetComponent<Room>().traps = rowSource[rowSource.Length - 1].traps;
                //tile.GetComponent<Room>().traps[0].transform.parent = tile.transform;
                int j = 0;
                foreach(GameObject trap in tile.GetComponent<Room>().traps)
                {
                    tile.GetComponent<Room>().traps[j].transform.parent = tile.transform;
                    tile.GetComponent<Room>().traps[j].transform.position = tile.GetComponent<Room>().traps[j].transform.parent.transform.position;
                    j++;
                }
            }
            else
            {
                tile.GetComponent<SpriteRenderer>().sprite = rowSource[i - 1].roomSprite;
                tile.GetComponent<Room>().traps = rowSource[i - 1].traps;
                int j = 0;
                foreach (GameObject trap in tile.GetComponent<Room>().traps)
                {
                    tile.GetComponent<Room>().traps[j].transform.parent = tile.transform;
                    tile.GetComponent<Room>().traps[j].transform.position = tile.GetComponent<Room>().traps[j].transform.parent.transform.position;
                    j++;
                }
                //if (tile.GetComponent<Room>().traps.Length > 0) { Instantiate(tile.GetComponent<Room>().traps[0]); }
            }
            //tile.GetComponent<Room>().traps = rowSource[i - 1].traps;
            i++;
        }
    }

    //move the grid panels back to their original position
    public void ResetPanels(List<GameObject> rowCol, List<Vector3> originalRowCol)
    {
        int i = 0;
        foreach (GameObject pan in rowCol)
        {
            pan.transform.position = originalRowCol[i];
            i++;
        }
    }

    //set the original pos
    void SetOriginalPos(List<GameObject> rowCol, List<Vector3> originalRowCol)
    {
        foreach (GameObject pan in rowCol)
        {
            originalRowCol.Add(new Vector3(pan.transform.position.x, pan.transform.position.y));
        }
    }

    void SetUpTraps(Room[] rowSource, List<GameObject> gridRowCol)
    {
        int i = 0;
        foreach(GameObject tile in gridRowCol)
        {
            if (i == 0)
            {
                tile.GetComponent<Room>().traps = new GameObject[rowSource[rowSource.Length - 1].GetComponent<Room>().traps.Length];
                int j = 0;
                foreach (GameObject trap in rowSource[rowSource.Length - 1].GetComponent<Room>().traps)
                {
                    tile.GetComponent<Room>().traps[j] = Instantiate(trap);
                    tile.GetComponent<Room>().traps[j].transform.parent = tile.transform;
                    tile.GetComponent<Room>().traps[j].transform.position = tile.GetComponent<Room>().traps[j].transform.parent.transform.position;
                    tile.GetComponent<Room>().traps[j].transform.localScale = trap.transform.localScale;
                    j++;
                }
                rowSource[rowSource.Length - 1].GetComponent<Room>().traps = tile.GetComponent<Room>().traps;
            }
            else
            {
                tile.GetComponent<Room>().traps = new GameObject[rowSource[i-1].GetComponent<Room>().traps.Length];
                int j = 0;
                foreach(GameObject trap in rowSource[i-1].GetComponent<Room>().traps)
                {
                    tile.GetComponent<Room>().traps[j] = Instantiate(trap);
                    tile.GetComponent<Room>().traps[j].transform.parent = tile.transform;
                    tile.GetComponent<Room>().traps[j].transform.position = tile.GetComponent<Room>().traps[j].transform.parent.transform.position;
                    tile.GetComponent<Room>().traps[j].transform.localScale = trap.transform.localScale;
                    j++;
                }
                rowSource[i-1].GetComponent<Room>().traps = tile.GetComponent<Room>().traps;
            }
            i++;
        }
    }

    //main update of the grid
    //shift rows, update arrays
    public void UpdateGrid(int upLeft, bool isRow, int rowColNumber, bool updown)
    {
        if (isRow)
        {
            if(upLeft == -1)//moving left
            {
                switch (rowColNumber)
                {
                    case 0:
                        testRow1 = ShiftUpLeft(testRow1);
                        currentRow = testRow1;
                        ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        break;
                    case 1:
                        testRow2 = ShiftUpLeft(testRow2);
                        currentRow = testRow2;
                        ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        break;
                    case 2:
                        testRow3 = ShiftUpLeft(testRow3);
                        currentRow = testRow3;
                        ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        break;
                }
            }
            else if(upLeft == 1)//moving right
            {
                switch (rowColNumber)
                {
                    case 0:
                        testRow1 = ShiftDownRight(testRow1);
                        currentRow = testRow1;
                        ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        break;
                    case 1:
                        testRow2 = ShiftDownRight(testRow2);
                        currentRow = testRow2;
                        ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        break;
                    case 2:
                        testRow3 = ShiftDownRight(testRow3);
                        currentRow = testRow3;
                        ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        break;
                }
            }
            else if(upLeft == 0)
            {
                switch (rowColNumber)
                {
                    case 0:
                        ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        break;
                    case 1:
                        ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        break;
                    case 2:
                        ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        break;
                }
            }
        }
        else
        {
            if (upLeft == -1)//moving up
            {
                switch (rowColNumber)
                {
                    case 0:
                        testCol1 = ShiftUpLeft(testCol1);
                        currentCol = testCol1;
                        ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        break;
                    case 1:
                        testCol2 = ShiftUpLeft(testCol2);
                        currentCol = testCol2;
                        ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        break;
                    case 2:
                        testCol3 = ShiftUpLeft(testCol3);
                        currentCol = testCol3;
                        ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        break;
                }
            }
            else if (upLeft == 1)//moving down
            {
                switch (rowColNumber)
                {
                    case 0:
                        testCol1 = ShiftDownRight(testCol1);
                        currentCol = testCol1;
                        ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        break;
                    case 1:
                        testCol2 = ShiftDownRight(testCol2);
                        currentCol = testCol2;
                        ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        break;
                    case 2:
                        testCol3 = ShiftDownRight(testCol3);
                        currentCol = testCol3;
                        ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        break;
                }
            }
            else if (upLeft == 0)
            {
                switch (rowColNumber)
                {
                    case 0:
                        ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        break;
                    case 1:
                        ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        break;
                    case 2:
                        ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        break;
                }
            }
        }
        UpdateMainNine(updown);
        SecondGroupMatching(updown);
    }
}
