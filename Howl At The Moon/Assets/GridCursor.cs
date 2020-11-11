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
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(currentY != MIN_Y)
            {
                currentY -= 1;
                SetPos();
            }
            else
            {
                currentY = MIN_Y;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(currentX != MIN_X)
            {
                currentX -= 1;
                SetPos();
            }
            else
            {
                currentX = MIN_X;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(currentY != MAX_Y)
            {
                currentY += 1;
                SetPos();
            }
            else
            {
                currentY = MAX_Y;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(currentX != MAX_X)
            {
                currentX += 1;
                SetPos();
            }
            else
            {
                currentX = MAX_X;
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            ped = new PointerEventData(ev);
            ped.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            foreach(RaycastResult result in results)
            {
                Debug.Log("Hit" + result.gameObject.name);
                SetCurrentRowAndCol(result.gameObject.name);
                break;
            }
        }
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
}
