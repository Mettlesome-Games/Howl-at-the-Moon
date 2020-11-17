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

    private Vector3[] targetTransforms;
    private bool moving = false;
    private float speed = 50;
    private bool left = false;
    private bool right = false;
    private bool up = false;
    private bool down = false;

    public GameObject[] grid;
    public Color[] currentRow;
    public Color[] currentCol;
    //The rows and cols of colors - to be rooms later
    public Color[] row0 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    public Color[] row1 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    public Color[] row2 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    public Color[] col0 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    public Color[] col1 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    public Color[] col2 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    //The grid panels by row and col ( index 1 - 3 is the main visible grid)
    public List<GameObject> gridPanelsRow0;
    public List<GameObject> gridPanelsRow1;
    public List<GameObject> gridPanelsRow2;
    public List<GameObject> gridPanelsCol0;
    public List<GameObject> gridPanelsCol1;
    public List<GameObject> gridPanelsCol2;
    //Where to store the original positions of the rows and cols
    public List<Vector3> originalPositionsRow0 = new List<Vector3>();
    public List<Vector3> originalPositionsRow1 = new List<Vector3>();
    public List<Vector3> originalPositionsRow2 = new List<Vector3>();
    public List<Vector3> originalPositionsCol0 = new List<Vector3>();
    public List<Vector3> originalPositionsCol1 = new List<Vector3>();
    public List<Vector3> originalPositionsCol2 = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        //DrawColors();
        targetTransforms = new Vector3[5];
 
        SetOriginalPos(gridPanelsRow0, originalPositionsRow0);
        SetOriginalPos(gridPanelsRow1, originalPositionsRow1);
        SetOriginalPos(gridPanelsRow2, originalPositionsRow2);
        SetOriginalPos(gridPanelsCol0, originalPositionsCol0);
        SetOriginalPos(gridPanelsCol1, originalPositionsCol1);
        SetOriginalPos(gridPanelsCol2, originalPositionsCol2);
    }

    // Update is called once per frame
    void Update()
    {
        DrawColors();
        /*if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int i = 0;
            if (currentRow == row0)
            {
                foreach (GameObject target in gridPanelsRow0)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x - 60, target.transform.position.y, 0);
                    i++;
                }
            }
            if (currentRow == row1)
            {
                foreach (GameObject target in gridPanelsRow1)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x - 60, target.transform.position.y, 0);
                    i++;
                }
            }
            if (currentRow == row2)
            {
                foreach (GameObject target in gridPanelsRow2)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x - 60, target.transform.position.y, 0);
                    i++;
                }
            }
            moving = true;
            left = true;
            right = false;
            up = false;
            down = false;
            //UpdateMainNine(false);
            //ThirdGroupMatching(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int i = 0;
            if (currentRow == row0)
            {
                foreach (GameObject target in gridPanelsRow0)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x + 60, target.transform.position.y, 0);
                    i++;

                }
            }
            if (currentRow == row1)
            {
                foreach (GameObject target in gridPanelsRow1)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x + 60, target.transform.position.y, 0);
                    i++;

                }
            }
            if (currentRow == row2)
            {
                foreach (GameObject target in gridPanelsRow2)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x + 60, target.transform.position.y, 0);
                    i++;

                }
            }
            moving = true;
            left = false;
            right = true;
            up = false;
            down = false;
            //UpdateMainNine(false);
            //ThirdGroupMatching(false);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int i = 0;
            if (currentCol == col0)
            {
                foreach (GameObject target in gridPanelsCol0)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x, target.transform.position.y + 60, 0);
                    i++;

                }
            }
            if (currentCol == col1)
            {
                foreach (GameObject target in gridPanelsCol1)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x, target.transform.position.y + 60, 0);
                    i++;

                }
            }
            if (currentCol == col2)
            {
                foreach (GameObject target in gridPanelsCol2)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x, target.transform.position.y + 60, 0);
                    i++;

                }
            }
            moving = true;
            left = false;
            right = false;
            up = true;
            down = false;
            //UpdateMainNine(true);
            //ThirdGroupMatching(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int i = 0;
            if (currentCol == col0)
            {
                foreach (GameObject target in gridPanelsCol2)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x, target.transform.position.y - 60, 0);
                    print(targetTransforms[i]);
                    i++;

                }
            }
            if (currentCol == col1)
            {
                foreach (GameObject target in gridPanelsCol2)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x, target.transform.position.y - 60, 0);
                    i++;

                }
            }
            if (currentCol == col2)
            {
                foreach (GameObject target in gridPanelsCol2)
                {
                    targetTransforms[i] = new Vector3(target.transform.position.x, target.transform.position.y - 60, 0);
                    i++;

                }
            }
            moving = true;
            left = false;
            right = false;
            up = false;
            down = true;
            //UpdateMainNine(true);
            //ThirdGroupMatching(true);
        }
        if (moving)
        {
            
            if (left)
            {
                if (currentRow == row0)
                {
                    if (!MovingRowCol(gridPanelsRow0))
                    {
                        row0 = ShiftUpLeft(row0);
                        currentRow = row0;
                        ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        UpdateMainNine(false);
                        ThirdGroupMatching(false);
                    }
                }
                else if (currentRow == row1)
                {
                    if (!MovingRowCol(gridPanelsRow1))
                    {
                        row1 = ShiftUpLeft(row1);
                        currentRow = row1;
                        ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        UpdateMainNine(false);
                        ThirdGroupMatching(false);
                    }
                }
                else if (currentRow == row2)
                {
                    if (!MovingRowCol(gridPanelsRow2))
                    {
                        row2 = ShiftUpLeft(row2);
                        currentRow = row2;
                        ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        UpdateMainNine(false);
                        ThirdGroupMatching(false);
                    }
                }
            }
            else if (right)
            {
                if (currentRow == row0)
                {
                    if (!MovingRowCol(gridPanelsRow0))
                    {
                        row0 = ShiftDownRight(row0);
                        currentRow = row0;
                        ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        UpdateMainNine(false);
                        ThirdGroupMatching(false);
                    }
                }
                else if (currentRow == row1)
                {
                    if (!MovingRowCol(gridPanelsRow1))
                    {
                        row1 = ShiftDownRight(row1);
                        currentRow = row1;
                        ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        UpdateMainNine(false);
                        ThirdGroupMatching(false);
                    }
                }
                else if (currentRow == row2)
                {
                    if (!MovingRowCol(gridPanelsRow2))
                    {
                        row2 = ShiftDownRight(row2);
                        currentRow = row2;
                        ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        UpdateMainNine(false);
                        ThirdGroupMatching(false);
                    }
                }
            }
            else if (up)
            {

            }
            else if (down)
            {
                if (currentCol == col0)
                {
                    if (!MovingRowCol(gridPanelsCol0))
                    {
                        col0 = ShiftDownRight(col0);
                        currentCol = col0;
                        ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        UpdateMainNine(true);
                        ThirdGroupMatching(true);
                    }
                }
                else if (currentCol == col1)
                {
                    if (!MovingRowCol(gridPanelsCol1))
                    {
                        col1 = ShiftDownRight(col1);
                        currentCol = col1;
                        ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        UpdateMainNine(true);
                        ThirdGroupMatching(true);
                    }
                }
                else if (currentCol == col2)
                {
                    if (!MovingRowCol(gridPanelsCol2))
                    {
                        col2 = ShiftDownRight(col2);
                        currentCol = col2;
                        ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        UpdateMainNine(true);
                        ThirdGroupMatching(true);
                    }
                }
            }

        }*/
    }

    public Color[] ShiftUpLeft(Color[] shifted)//shifts up or left and then calls UpdateMainNine
    {
        Color[] temp = new Color[shifted.Length];
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
        DrawRow(gridPanelsRow0, row0);
        DrawRow(gridPanelsRow1, row1);
        DrawRow(gridPanelsRow2, row2);
        DrawRow(gridPanelsCol0, col0);
        DrawRow(gridPanelsCol1, col1);
        DrawRow(gridPanelsCol2, col2);
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

    public void ResetPanels(List<GameObject> rowCol, List<Vector3> originalRowCol)
    {
        left = false;
        right = false;
        up = false;
        down = false;
        moving = false;
        int i = 0;
        foreach (GameObject pan in rowCol)
        {
            pan.transform.position = originalRowCol[i];
            i++;
        }
    }

    public bool MovingRowCol(List<GameObject> rowCol)
    {
        if (targetTransforms[0] != rowCol[0].transform.position)
        {
            int i = 0;
            var step = speed * Time.deltaTime;
            foreach (GameObject target in rowCol)
            {
                target.transform.position = Vector3.MoveTowards(target.transform.position, targetTransforms[i], step);
                i++;

            }
        }
        else
        {
            return false;
        }

        return true;
    }

    void SetOriginalPos(List<GameObject> rowCol, List<Vector3> originalRowCol)
    {
        foreach (GameObject pan in rowCol)
        {
            originalRowCol.Add(new Vector3(pan.transform.position.x, pan.transform.position.y));
        }
    }

    public void UpdateGrid(int upLeft, bool isRow, int rowColNumber, bool updown)
    {
        if (isRow)
        {
            if(upLeft == -1)
            {
                switch (rowColNumber)
                {
                    case 0:
                        row0 = ShiftUpLeft(row0);
                        currentRow = row0;
                        ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        break;
                    case 1:
                        row1 = ShiftUpLeft(row1);
                        currentRow = row1;
                        ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        break;
                    case 2:
                        row2 = ShiftUpLeft(row2);
                        currentRow = row2;
                        ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        break;
                }
            }
            else if(upLeft == 1)
            {
                switch (rowColNumber)
                {
                    case 0:
                        row0 = ShiftDownRight(row0);
                        currentRow = row0;
                        ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        break;
                    case 1:
                        row1 = ShiftDownRight(row1);
                        currentRow = row1;
                        ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        break;
                    case 2:
                        row2 = ShiftDownRight(row2);
                        currentRow = row2;
                        ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        break;
                }
            }
        }
        else
        {
            if (upLeft == -1)
            {
                switch (rowColNumber)
                {
                    case 0:
                        col0 = ShiftUpLeft(col0);
                        currentCol = col0;
                        ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        break;
                    case 1:
                        col1 = ShiftUpLeft(col1);
                        currentCol = col1;
                        ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        break;
                    case 2:
                        col2 = ShiftUpLeft(col2);
                        currentCol = col2;
                        ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        break;
                }
            }
            else if (upLeft == 1)
            {
                switch (rowColNumber)
                {
                    case 0:
                        col0 = ShiftDownRight(col0);
                        currentCol = col0;
                        ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        break;
                    case 1:
                        col1 = ShiftDownRight(col1);
                        currentCol = col1;
                        ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        break;
                    case 2:
                        col2 = ShiftDownRight(col2);
                        currentCol = col2;
                        ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        break;
                }
            }
        }
        UpdateMainNine(updown);
        ThirdGroupMatching(updown);
    }
}
