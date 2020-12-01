using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridCursor : MonoBehaviour
{
    //raycaster
    GraphicRaycaster gr;
    PointerEventData ped;
    EventSystem ev;
    
    //unused but maybe should use?
    Room[] currentRow;
    Room[] currentCol;

    //for swiping
    Vector3 firstPressPos;
    bool dragging = false;
    public float sensitivity = 0.01f;//adjust this for sliding speed
    bool leftRight = false;
    bool upDown = false;

    GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        //for raycasting the click
        gr = FindObjectOfType<GraphicRaycaster>();
        ev = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Gamemanager.instance.PauseGame();
            firstPressPos = Input.mousePosition;//get the location of the initial click
            dragging = true;
            ped = new PointerEventData(ev);
            ped.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            foreach(RaycastResult result in results)//save the name of the first object (should be a number between 0-8)
            {
                SetCurrentRowAndCol(result.gameObject.name);
                break;
            }
        }
        if (dragging)
        {
            if (leftRight)//if dragging a row left or right
            {
                if(gridManager.currentRow == gridManager.testRow1)
                {
                    DragLeftRight(gridManager.gridPanelsRow0);
                }
                else if (gridManager.currentRow == gridManager.testRow2)
                {
                    DragLeftRight(gridManager.gridPanelsRow1);
                }
                else if (gridManager.currentRow == gridManager.testRow3)
                {
                    DragLeftRight(gridManager.gridPanelsRow2);
                }
            }
            else if (upDown)//if dragging a col up or down
            {
                if (gridManager.currentCol == gridManager.testCol1)
                {
                    DragUpDown(gridManager.gridPanelsCol0);
                }
                else if (gridManager.currentCol == gridManager.testCol2)
                {
                    DragUpDown(gridManager.gridPanelsCol1);
                }
                else if (gridManager.currentCol == gridManager.testCol3)
                {
                    DragUpDown(gridManager.gridPanelsCol2);
                }
            }
            else if((-1 * Input.mousePosition.x + firstPressPos.x) > 0.5f || (-1 * Input.mousePosition.x + firstPressPos.x) < -0.5f)//-0.5f > xmove||ment > 0.5(if lots of x movement)
            {
                leftRight = true;
            }
            else if((-1 * Input.mousePosition.y + firstPressPos.y) > 0.5f || (-1 * Input.mousePosition.y + firstPressPos.y) < -0.5f)//-0.5f > ymove||ment > 0.5)(if lots of y movement)
            {
                upDown = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                gridManager.SaveNPCLocalPos(gridManager.gridPanelsRow0, gridManager.testRow1);
                gridManager.SaveNPCLocalPos(gridManager.gridPanelsRow1, gridManager.testRow2);
                gridManager.SaveNPCLocalPos(gridManager.gridPanelsRow2, gridManager.testRow3);
                gridManager.SaveNPCLocalPos(gridManager.gridPanelsCol0, gridManager.testCol1);
                gridManager.SaveNPCLocalPos(gridManager.gridPanelsCol1, gridManager.testCol2);
                gridManager.SaveNPCLocalPos(gridManager.gridPanelsCol2, gridManager.testCol3);
                if (leftRight)
                {
                    if(gridManager.currentRow == gridManager.testRow1)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow0, gridManager.originalPositionsRow0);
                        //MoveToCorrectPos(gridManager.gridPanelsRow0);
                        gridManager.UpdateGrid(i[0], true, 0, false);//(-1,0,1), true, 0, false
                    }
                    else if (gridManager.currentRow == gridManager.testRow2)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow1, gridManager.originalPositionsRow1);
                        //MoveToCorrectPos(gridManager.gridPanelsRow1);
                        gridManager.UpdateGrid(i[0], true, 1, false);//(-1,0,1), true, 1, false
                    }
                    else if (gridManager.currentRow == gridManager.testRow3)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow2, gridManager.originalPositionsRow2);
                        //MoveToCorrectPos(gridManager.gridPanelsRow2);
                        gridManager.UpdateGrid(i[0], true, 2, false);//(-1,0,1), true, 2, false
                    }
                    Gamemanager.instance.ResumeGame();
                }
                if (upDown)
                {
                    if (gridManager.currentCol == gridManager.testCol1)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol0, gridManager.originalPositionsCol0);
                        //MoveToCorrectPos(gridManager.gridPanelsCol0);
                        gridManager.UpdateGrid(i[1], false, 0, true);//(-1,0,1), false, 0, true
                    }
                    else if (gridManager.currentCol == gridManager.testCol2)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol1, gridManager.originalPositionsCol1);
                        //MoveToCorrectPos(gridManager.gridPanelsCol1);
                        gridManager.UpdateGrid(i[1], false, 1, true);//(-1,0,1), false, 1, true
                    }
                    else if (gridManager.currentCol == gridManager.testCol3)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol2, gridManager.originalPositionsCol2);
                        //MoveToCorrectPos(gridManager.gridPanelsCol2);
                        gridManager.UpdateGrid(i[1], false, 2, true);//(-1,0,1), false, 2, true
                    }
                    Gamemanager.instance.ResumeGame();

                }
                dragging = false;
                leftRight = false;
                upDown = false;
                gridManager.ResetNPCs(gridManager.gridPanelsRow0, gridManager.testRow1);
                gridManager.ResetNPCs(gridManager.gridPanelsRow1, gridManager.testRow2);
                gridManager.ResetNPCs(gridManager.gridPanelsRow2, gridManager.testRow3);
                gridManager.ResetNPCs(gridManager.gridPanelsCol0, gridManager.testCol1);
                gridManager.ResetNPCs(gridManager.gridPanelsCol1, gridManager.testCol2);
                gridManager.ResetNPCs(gridManager.gridPanelsCol2, gridManager.testCol3);
            }
        }
    }

    private void DragLeftRight(List<GameObject> rowCol)
    {
        Vector3 rotation = Vector3.zero;

        Vector3 mouseOffset = (-1 * Input.mousePosition + firstPressPos);
        rotation.x = ((mouseOffset.x) * sensitivity * -1f);//works but get it to work better
        int i = 0;
        foreach(GameObject tile in rowCol)
        {
            if(tile.transform.position.x + rotation.x > gridManager.originalPositionsRow1[i].x +2)//do not go past one tile to the right
            {
                tile.transform.Translate(Vector3.zero, Space.Self);
            }
            else if (tile.transform.position.x + rotation.x < gridManager.originalPositionsRow1[i].x - 2)//do not go past one tile to the left
            {

            }
            else
            {
                tile.transform.Translate(rotation, Space.Self);
            }
            i++;
        }

        firstPressPos = Input.mousePosition;
    }
    private void DragUpDown(List<GameObject> rowCol)
    {
        Vector3 rotation = Vector3.zero;

        Vector3 mouseOffset = (-1 * Input.mousePosition + firstPressPos);
        rotation.y = ((mouseOffset.y) * sensitivity * -1f);//works but get it to work better
        int i = 0;
        foreach (GameObject tile in rowCol)
        {
            if (tile.transform.position.y + rotation.y > gridManager.originalPositionsCol0[i].y + 2)//do not go past one tile up
            {
                tile.transform.Translate(Vector3.zero, Space.Self);
            }
            else if (tile.transform.position.y + rotation.y < gridManager.originalPositionsCol0[i].y - 2)//do not go past one tile down
            {

            }
            else
            {
                tile.transform.Translate(rotation, Space.Self);
                //if (tile.GetComponent<Room>().traps.Length > 0) { tile.GetComponent<Room>().traps[0].transform.Translate(rotation, Space.World); }
            }
            //tile.transform.Translate(rotation, Space.Self);
            i++;
        }

        firstPressPos = Input.mousePosition;
    }
    public void MoveToCorrectPos(List<GameObject> rowCol)//snap the grid in place
    {
        int[] i = new int[2];
        foreach (GameObject tile in rowCol)
        {
            float scale = 2f;
            Vector3 vec = tile.transform.localPosition;
            vec.x = Mathf.Round(vec.x / scale) * scale;
            vec.y = Mathf.Round(vec.y / scale) * scale;
            vec.z = Mathf.Round(vec.z / scale) * scale;
            tile.transform.localPosition = vec;
        }
    }

    bool RightSwipe(Vector2 swipe)
    {
        return swipe.x < 0 && swipe.y > -0.5 && swipe.y < 0.5;
    }
    bool LeftSwipe(Vector2 swipe)
    {
        return swipe.x > 0 && swipe.y > -0.5 && swipe.y < 0.5;
    }
    bool DownSwipe(Vector2 swipe)
    {
        return swipe.y > 0 && swipe.x > -0.5 && swipe.x < 0.5;
    }
    bool UpSwipe(Vector2 swipe)
    {
        return swipe.y < 0 && swipe.x > -0.5 && swipe.x < 0.5;
    }

    void SetCurrentRowAndCol(string tile)//based on click sets currentRow/Col in the grid
    {
        switch (tile)
        {
            case "0":
                gridManager.currentRow = gridManager.testRow1;
                gridManager.currentCol = gridManager.testCol1;
                break;
            case "1":
                gridManager.currentRow = gridManager.testRow1;
                gridManager.currentCol = gridManager.testCol2;
                break;
            case "2":
                gridManager.currentRow = gridManager.testRow1;
                gridManager.currentCol = gridManager.testCol3;
                break;
            case "3":
                gridManager.currentRow = gridManager.testRow2;
                gridManager.currentCol = gridManager.testCol1;
                break;
            case "4":
                gridManager.currentRow = gridManager.testRow2;
                gridManager.currentCol = gridManager.testCol2;
                break;
            case "5":
                gridManager.currentRow = gridManager.testRow2;
                gridManager.currentCol = gridManager.testCol3;
                break;
            case "6":
                gridManager.currentRow = gridManager.testRow3;
                gridManager.currentCol = gridManager.testCol1;
                break;
            case "7":
                gridManager.currentRow = gridManager.testRow3;
                gridManager.currentCol = gridManager.testCol2;
                break;
            case "8":
                gridManager.currentRow = gridManager.testRow3;
                gridManager.currentCol = gridManager.testCol3;
                break;
        }
    }

    //based on the position of the center panel when click released, decide if movement is left, right, up, down or none
    int[] GetDirection(List<GameObject> panels, List<Vector3> originalPos)
    {
        int[] results = { 0, 0 };//x, y
        if(panels[2].transform.position.x < originalPos[2].x - 1.2f)
        {
            results[0] = -1;//left
        }
        else if(panels[2].transform.position.x > originalPos[2].x + 1.2f)
        {
            results[0] = 1;//right
        }
        if (panels[2].transform.position.y < originalPos[2].y - 1.2f)
        {
            results[1] = 1;//down
        }
        else if (panels[2].transform.position.y > originalPos[2].y + 1.2f)
        {
            results[1] = -1;//up
        }
        return results;
    }
}
