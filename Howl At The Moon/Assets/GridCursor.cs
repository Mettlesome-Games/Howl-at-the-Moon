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

    //keep the selection within what the player can see
    private const int MAX_X = 2;
    private const int MAX_Y = 2;
    private const int MIN_X = 0;
    private const int MIN_Y = 0;

    public GameObject cursor;
    Vector3 cursorInitialPos;
    Color[] currentRow;
    Color[] currentCol;

    int currentX = 0;//use the x to determine the index
    int currentY = 0;//use the y to determine which array (row0, row1...)

    //for swiping
    Vector3 firstPressPos;
    Vector3 lastPressPos;
    Vector2 currentSwipe;
    bool dragging = false;
    bool autoRotating = false;
    private float sensitivity = 0.4f;
    bool leftRight = false;
    bool upDown = false;
    Vector3[] localTargets = new Vector3[5];

    GridManager gridManager;
    Transform tform;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        tform = GetComponent<RectTransform>();
        cursorInitialPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        //rays
        gr = FindObjectOfType<GraphicRaycaster>();
        ev = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            firstPressPos = Input.mousePosition;
            lastPressPos = firstPressPos;
            dragging = true;
            ped = new PointerEventData(ev);
            ped.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            foreach(RaycastResult result in results)
            {
                //Debug.Log("Hit" + result.gameObject.name);
                SetCurrentRowAndCol(result.gameObject.name);
                cursor.GetComponent<Image>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
                break;
            }
        }
        if (dragging)
        {
            if (leftRight)
            {
                if(gridManager.currentRow == gridManager.row0)
                {
                    DragLeftRight(gridManager.gridPanelsRow0);
                }
                else if (gridManager.currentRow == gridManager.row1)
                {
                    DragLeftRight(gridManager.gridPanelsRow1);
                }
                else if (gridManager.currentRow == gridManager.row2)
                {
                    DragLeftRight(gridManager.gridPanelsRow2);
                }
            }
            else if (upDown)
            {
                if (gridManager.currentCol == gridManager.col0)
                {
                    DragUpDown(gridManager.gridPanelsCol0);
                }
                else if (gridManager.currentCol == gridManager.col1)
                {
                    DragUpDown(gridManager.gridPanelsCol1);
                }
                else if (gridManager.currentCol == gridManager.col2)
                {
                    DragUpDown(gridManager.gridPanelsCol2);
                }
            }
            else if((-1 * Input.mousePosition.x + lastPressPos.x) > 0.5f || (-1 * Input.mousePosition.x + lastPressPos.x) < -0.5f)//-0.5f > xmove||ment > 0.5
            {
                leftRight = true;
            }
            else if((-1 * Input.mousePosition.y + lastPressPos.y) > 0.5f || (-1 * Input.mousePosition.y + lastPressPos.y) < -0.5f)//-0.5f > ymove||ment > 0.5)
            {
                upDown = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (leftRight)
                {
                    if(gridManager.currentRow == gridManager.row0)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow0, gridManager.originalPositionsRow0);
                        MoveToCorrectPos(gridManager.gridPanelsRow0);
                        gridManager.UpdateGrid(i[0], true, 0, false);//(-1,0,1), true, 0, false
                    }
                    else if (gridManager.currentRow == gridManager.row1)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow1, gridManager.originalPositionsRow1);
                        MoveToCorrectPos(gridManager.gridPanelsRow1);
                        gridManager.UpdateGrid(i[0], true, 1, false);//(-1,0,1), true, 1, false
                    }
                    else if (gridManager.currentRow == gridManager.row2)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow2, gridManager.originalPositionsRow2);
                        MoveToCorrectPos(gridManager.gridPanelsRow2);
                        gridManager.UpdateGrid(i[0], true, 2, false);//(-1,0,1), true, 2, false
                    }
                }
                if (upDown)
                {
                    if (gridManager.currentCol == gridManager.col0)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol0, gridManager.originalPositionsCol0);
                        MoveToCorrectPos(gridManager.gridPanelsCol0);
                        gridManager.UpdateGrid(i[1], false, 0, true);//(-1,0,1), false, 0, true
                    }
                    else if (gridManager.currentCol == gridManager.col1)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol1, gridManager.originalPositionsCol1);
                        MoveToCorrectPos(gridManager.gridPanelsCol1);
                        gridManager.UpdateGrid(i[1], false, 1, true);//(-1,0,1), false, 1, true
                    }
                    else if (gridManager.currentCol == gridManager.col2)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol2, gridManager.originalPositionsCol2);
                        MoveToCorrectPos(gridManager.gridPanelsCol2);
                        gridManager.UpdateGrid(i[1], false, 2, true);//(-1,0,1), false, 2, true
                    }
                }
                dragging = false;
                leftRight = false;
                upDown = false;
                //autoRotating = true;
                //firstPressPos = Input.mousePosition;
            }
        }
        /*if (autoRotating)
        {
            AutoRotate();
            row0 = ShiftUpLeft(row0);
            currentRow = row0;
            ResetPanels(gridPanelsRow0, originalPositionsRow0);
            UpdateMainNine(false);
            ThirdGroupMatching(false);
        }
        if (false)
        {
            secondPressPos = Input.mousePosition;
            currentSwipe = firstPressPos - secondPressPos;
            currentSwipe.Normalize();
            if (LeftSwipe(currentSwipe))
            {
                print("left");
            }
            else if (RightSwipe(currentSwipe))
            {
                print("right");
            }
            else if (UpSwipe(currentSwipe))
            {
                print("up");
            }
            else if (DownSwipe(currentSwipe))
            {
                print("down");
            }
        }*/
    }

    private void DragLeftRight(List<GameObject> rowCol)
    {
        Vector3 rotation = Vector3.zero;

        Vector3 mouseOffset = (-1 * Input.mousePosition + lastPressPos);
        //print(mouseOffset);
        rotation.x = ((mouseOffset.x) * sensitivity * -1f);//works but get it to work better
        int i = 0;
        foreach(GameObject tile in rowCol)
        {
            if(tile.transform.position.x + rotation.x > gridManager.originalPositionsRow0[i].x +60)
            {
                tile.transform.Translate(Vector3.zero, Space.Self);
            }
            else if (tile.transform.position.x + rotation.x < gridManager.originalPositionsRow0[i].x - 60)
            {

            }
            else
            {
                tile.transform.Translate(rotation, Space.Self);
            }
            //tile.transform.Translate(rotation, Space.Self);
            i++;
        }

        lastPressPos = Input.mousePosition;
    }
    private void DragUpDown(List<GameObject> rowCol)
    {
        Vector3 rotation = Vector3.zero;

        Vector3 mouseOffset = (-1 * Input.mousePosition + lastPressPos);
        //print(mouseOffset);
        rotation.y = ((mouseOffset.y) * sensitivity * -1f);//works but get it to work better
        int i = 0;
        foreach (GameObject tile in rowCol)
        {
            if (tile.transform.position.y + rotation.y > gridManager.originalPositionsCol0[i].y + 60)
            {
                tile.transform.Translate(Vector3.zero, Space.Self);
            }
            else if (tile.transform.position.y + rotation.y < gridManager.originalPositionsCol0[i].y - 60)
            {

            }
            else
            {
                tile.transform.Translate(rotation, Space.Self);
            }
            //tile.transform.Translate(rotation, Space.Self);
            i++;
        }

        lastPressPos = Input.mousePosition;
    }
    public void MoveToCorrectPos(List<GameObject> rowCol)
    {
        int[] i = new int[2];
        foreach(GameObject tile in rowCol)
        {
            Vector3 vec = tile.transform.localPosition;
            vec.x = Mathf.Round(vec.x / 60) * 60;
            vec.y = Mathf.Round(vec.y / 60) * 60;
            vec.z = Mathf.Round(vec.z / 60) * 60;
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

    void SetPos()
    {
        transform.localPosition = new Vector3(cursorInitialPos.x + currentX * 60, cursorInitialPos.y - currentY * 60, cursorInitialPos.z);
    }

    void SetCurrentRowAndCol(string tile)
    {
        switch (tile)
        {
            case "0":
                gridManager.currentRow = gridManager.row0;
                gridManager.currentCol = gridManager.col0;
                break;
            case "1":
                gridManager.currentRow = gridManager.row0;
                gridManager.currentCol = gridManager.col1;
                break;
            case "2":
                gridManager.currentRow = gridManager.row0;
                gridManager.currentCol = gridManager.col2;
                break;
            case "3":
                gridManager.currentRow = gridManager.row1;
                gridManager.currentCol = gridManager.col0;
                break;
            case "4":
                gridManager.currentRow = gridManager.row1;
                gridManager.currentCol = gridManager.col1;
                break;
            case "5":
                gridManager.currentRow = gridManager.row1;
                gridManager.currentCol = gridManager.col2;
                break;
            case "6":
                gridManager.currentRow = gridManager.row2;
                gridManager.currentCol = gridManager.col0;
                break;
            case "7":
                gridManager.currentRow = gridManager.row2;
                gridManager.currentCol = gridManager.col1;
                break;
            case "8":
                gridManager.currentRow = gridManager.row2;
                gridManager.currentCol = gridManager.col2;
                break;
        }
    }

    int[] GetDirection(List<GameObject> panels, List<Vector3> originalPos)
    {
        int[] results = { 0, 0 };
        if(panels[2].transform.position.x < originalPos[2].x - 30)
        {
            results[0] = -1;
        }
        else if(panels[2].transform.position.x > originalPos[2].x + 30)
        {
            results[0] = 1;
        }
        if (panels[2].transform.position.y < originalPos[2].y - 30)
        {
            results[1] = 1;
        }
        else if (panels[2].transform.position.y > originalPos[2].y + 30)
        {
            results[1] = -1;
        }
        print(results[0]);
        print(results[1]);
        return results;
    }
}
