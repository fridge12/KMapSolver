using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//attach to parent object
public class KMapRenderer : MonoBehaviour
{

    static readonly bool toPrint = false;

    //cell prefab
    public GameObject cell;

    //label prefab
    public GameObject label;

    public static GameObject[] columnLabels;
    public static GameObject[] rowLabels;


    //array containing visible kmap
    public GameObject[,] KMapArray;

    //parent object
    public GameObject parent;

    public static float parentStartY = 0;
    public static float parentStartX = 0;

    public static float parentEndY = -252;
    public static float parentEndX = 450;

    public static readonly int maxColumns = 16;
    public static readonly int maxRows = 8;

    //number of columns and rows
    public static int columns;
    public static int rows;

    //size of cell
    Vector2 cellSize;

    //holds the velocity of the cells
    public static Vector2 cellVelocity = new Vector2(0,0);


    public static int arrayRowStart=0;
    public static int arrayColStart=0;

    //public static int updateArrayRowStart=0;
    //public static int updateArrayColStart=0;

    public static bool canMoveUp = true;
    public static bool canMoveDown = false;
    public static bool canMoveLeft = true;
    public static bool canMoveRight = false;



    public static Vector2 oldMousePosition;
    private void Start()
    {
        oldMousePosition = Input.mousePosition;
        
        //initialise();

       
    }

    // Update is called once per frame
    int ctr = 0;
    void Update()
    {
        //arrayColStart += updateArrayColStart;
        //arrayRowStart += updateArrayRowStart;

        canMoveDown = canMoveLeft = canMoveRight = canMoveUp = true;

        if (arrayColStart < 0) canMoveRight = false;
        if (arrayColStart + maxColumns >= columns) canMoveLeft = false;
        if (arrayRowStart < 0) canMoveDown = false;
        if (arrayRowStart + maxRows >= rows) canMoveUp = false;

        if (rows <= maxRows) canMoveDown = canMoveUp = false;
        if (columns <= maxColumns) canMoveRight = canMoveLeft = false;

        if (ctr % 1000 == 0)
        {
            Debug.Log(ctr);
            //Debug.Log(parent.GetComponent<RectTransform>().sizeDelta);
            //Debug.Log(ScreenConstants.oldScale / ScreenConstants.scale);
        }
        ctr++;

        if (Input.GetMouseButton(0))
        {
            cellVelocity.x = Input.mousePosition.x - oldMousePosition.x;
            cellVelocity.y = Input.mousePosition.y - oldMousePosition.y;
        }
        else
        {
            cellVelocity.x = 0;
            cellVelocity.y = 0;
        }
        oldMousePosition = Input.mousePosition;
    }

    public void initialise()
    {
        //Kmap.variables =10;
        clearAll();

        canMoveUp = true;
        canMoveDown = false;
        canMoveLeft = true;
        canMoveRight = false;

        arrayRowStart = 0;
        arrayColStart = 0;

        rows = (int)System.Math.Pow(2, (Kmap.variables / 2));
        columns = (int)System.Math.Pow(2, Kmap.variables - (Kmap.variables / 2));

        cellSize = new Vector2(System.Math.Abs(parentEndX-parentStartX) / (float)System.Math.Min(columns, maxColumns), System.Math.Abs(parentEndY - parentStartY)/ (float)System.Math.Min(rows, maxRows ));

        columnLabels = new GameObject[System.Math.Min(columns, maxColumns + 1)];
        rowLabels = new GameObject[System.Math.Min(rows, maxRows + 1)];

        KMapArray = new GameObject[System.Math.Min(rows, maxRows + 1), System.Math.Min(columns, maxColumns + 1)];

        

        spawnAllCells();
    }
    public void clearAll()
    {
        if (KMapArray == null) return; 

        for (int i = 0; i < KMapArray.GetLength(0); i++)
        {
            for (int j = 0; j < KMapArray.GetLength(1); j++)
            {

                //if(KMapArray[i, j]!= null)
                Destroy(KMapArray[i, j].GetComponent<cellResizer>());
                Destroy(KMapArray[i, j].GetComponent<cellMover>());
                clear(KMapArray[i, j].transform);
            }
        }
        
        for (int i = 0; i < rowLabels.Length; i++)
        {
            //if(rowLabels[i] != null)
            Destroy(rowLabels[i].GetComponent<rowLabelMover>());
            clear(rowLabels[i].transform);
        }
        
        for (int i = 0; i < columnLabels.Length; i++)
        {
            //if(columnLabels[i]!= null)
            Destroy(columnLabels[i].GetComponent<columnLabelMover>());
            clear(columnLabels[i].transform);
        }

        Debug.Log("clearing over");
    }

    public void clear(Transform t)
    {
        while (t.childCount > 0)
        {
            DestroyImmediate(t.GetChild(0).gameObject);
        }
        DestroyImmediate(t.gameObject);
    }

    public void spawnAllCells()
    {
       // for (int i = 0; i < 128; i++)
        {
            //cellSpawner(i % columns, i / columns);
           // Debug.Log(i % columns);
            //Debug.Log(i / columns);
        }
        //Debug.Log("spawning all cells");

        for (int i = 0; i < System.Math.Min(columns, maxColumns + 1); i++)
        {
            for (int j = 0; j < System.Math.Min(rows, maxRows + 1); j++)
            {
                cellSpawner(i, j);
            }
        }

        for (int i = 0; i < rowLabels.Length; i++)
        {
            rowLabelSpawner(i);
        }
        for (int i = 0; i < columnLabels.Length; i++)
        {
            columnLabelSpawner(i);
        }
    }

    Group g = new Group(0, 0);
    GroupComparer gc = new GroupComparer();
    public void cellSpawner(int x, int y)
    {

        GameObject ob = Instantiate(cell, new Vector3(0, 0, 0), Quaternion.identity);
        ob.transform.SetParent(parent.transform);
        ob.GetComponent<RectTransform>().sizeDelta = cellSize * (ScreenConstants.scale/ ScreenConstants.oldScale); 


        KMapArray[y% System.Math.Min(rows, maxRows + 1), x % System.Math.Min(columns, maxColumns + 1)] = ob;

        ob.GetComponent<RectTransform>().localPosition = new Vector3(x * cellSize.x, (-y * cellSize.y)-(cellSize.y *0.66f), 0);
        uint coordinate = calculateCoordinate(x,y);


        g.coordinate = coordinate;
        int pos = Kmap.groupsList.BinarySearch(g, gc);
        if (pos >= 0)
        {
            ob.GetComponentsInChildren<Text>()[0].GetComponent<Outline>().effectColor = Kmap.groupsList[pos].borderColour;
            Debug.Log("colour");
            Debug.Log(g.borderColour);

            if (Kmap.SOP_POS == "SOP") ob.GetComponentsInChildren<Text>()[0].text = "1";
            else ob.GetComponentsInChildren<Text>()[0].text = "0";
        }
        else
        {
            ob.GetComponentsInChildren<Text>()[0].text = "";

        }

        ob.GetComponentsInChildren<Text>()[1].text = coordinate+"";
    }

    public void rowLabelSpawner(int y)
    {
        GameObject ob = Instantiate(label, new Vector3(0, 0, 0), Quaternion.identity);
        ob.transform.SetParent(parent.transform.parent.transform);
        ob.GetComponent<RectTransform>().sizeDelta = cellSize * (ScreenConstants.scale / ScreenConstants.oldScale);
        rowLabels[y % System.Math.Min(rows, maxRows + 1)] = ob;
        ob.AddComponent<rowLabelMover>();
        ob.GetComponent<RectTransform>().localPosition = new Vector3(50-cellSize.x, (-y * cellSize.y)-28, 0);
        ob.GetComponent<Text>().text = ConstantVariables( calculateCoordinate(0, y) & 0xAAAAAAA);
        ob.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
    }

    public void columnLabelSpawner(int x)
    {
        GameObject ob = Instantiate(label, new Vector3(0, 0, 0), Quaternion.identity);
        ob.transform.SetParent(parent.transform.parent.transform);
        ob.GetComponent<RectTransform>().sizeDelta = cellSize * (ScreenConstants.scale / ScreenConstants.oldScale);
        columnLabels[x % System.Math.Min(columns, maxColumns + 1)] = ob;
        ob.AddComponent<columnLabelMover>();
        ob.GetComponent<RectTransform>().localPosition = new Vector3((x * cellSize.x)+50,  cellSize.y-28, 0);
        ob.GetComponent<Text>().text = ConstantVariables(calculateCoordinate(x,0) & 0x55555555);
        ob.GetComponent<Text>().alignment = TextAnchor.LowerCenter;
    }

    //TODO: fix the labels

    public static string ConstantVariables(uint coordinate)
    {
        System.Text.StringBuilder exp = new System.Text.StringBuilder("");
        
        //should be kmap.variables - 1 not 7
        for (int i = Kmap.variables ; i >= 0; i--)
        {
            if (((coordinate >> i) & 1) == 1)
            {
                exp = exp.Append((char)(64 + Kmap.variables  - i));

            }
        }

        //returning constant variables
        return exp.ToString();
    }

    public static uint calculateCoordinate(int x, int y)
    {
        if(Kmap.variables == 4 || Kmap.variables == 3)
        {
            if (x == 2) x = 3;
            else if (x == 3) x = 2;

            if (y == 2) y = 3;
            else if (y == 3) y = 2;

            return (uint)((y * 4) + x);
        }


        uint coordinate = 0;
        //n should be kmap.variables -1 but for testing its 4
        for (int n = Kmap.variables ; n >= 0; n--)
        {
            if (n % 2 == 1)
            {
                coordinate = (uint)((((y >> (n / 2)) & 1) << n) + coordinate);
            }
            else
            {
                coordinate = (uint)((((x >> (n / 2)) & 1) << n) + coordinate);

            }
        }

        return coordinate;
    }
    
    public static int[] calculateXY(int coordinate)
    {
            int x = 0;
            int y = 0;
            for (int n = Kmap.variables; n >= 0; n--)
            {

                //if(((m >> n) & 1) == 0) continue;

                if (n % 2 == 1)
                {
                    y = (y << 1) + ((coordinate >> n) & 1);
                }
                else
                {
                    x = (x << 1) + ((coordinate >> n) & 1);
                }

            }
        if (Kmap.variables == 4 || Kmap.variables == 3)
        {
            y = coordinate / 4;
            x = coordinate % 4;

            if (x == 2) x = 3;
            else if (x == 3) x = 2;

            if (y == 2) y = 3;
            else if (y == 3) y = 2;

        }

        int[] a = { x, y };
        return a;
    }

}