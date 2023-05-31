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

    //array containing visible kmap
    public GameObject[,] KMapArray;

    //parent object
    public GameObject parent;

    public static float parentStartY = 0;
    public static float parentStartX = 0;

    public static float parentEndY = -252;
    public static float parentEndX = 450;

    //number of columns and rows
    public int columns;
    public int rows;

    //size of cell
    Vector2 cellSize;

    //holds the velocity of the cells
    public static Vector2 cellVelocity = new Vector2(-0,-0);


    public static int arrayRowStart=0;
    public static int arrayColStart=0;

    public static int updateArrayRowStart=0;
    public static int updateArrayColStart=0;


    public static Vector2 oldMousePosition;
    private void Start()
    {
        oldMousePosition = Input.mousePosition;
        initialise();

        GameObject ob;
        //cell.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        for (int i = 0; i < 128; i++)
        {
            cellSpawner(i % columns, i / columns);
            Debug.Log(i % columns);
            Debug.Log(i / columns);
        }


    }

    // Update is called once per frame
    int ctr = 0;
    void Update()
    {
        arrayColStart += updateArrayColStart;
        arrayRowStart += updateArrayRowStart;

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
        cellSize = new Vector2(System.Math.Abs(parentEndX-parentStartX) / (float)columns, System.Math.Abs(parentEndY - parentStartY)/ (float)rows);
        KMapArray = new GameObject[rows, columns]; 
    }

    public void cellSpawner(int x, int y)
    {

        GameObject ob = Instantiate(cell, new Vector3(0, 0, 0), Quaternion.identity);
        ob.transform.SetParent(parent.transform);
        ob.GetComponent<RectTransform>().sizeDelta = cellSize * (ScreenConstants.scale/ ScreenConstants.oldScale); 


        KMapArray[y%rows, x%columns] = ob;

        ob.GetComponent<RectTransform>().localPosition = new Vector3(x * cellSize.x, (-y * cellSize.y)-(cellSize.y *0.66f), 0);
        int coordinate = calculateCoordinate(x,y);


        ob.GetComponentsInChildren<Text>()[1].text = coordinate+"";
    }



    public int calculateCoordinate(int x, int y)
    {
        int coordinate = 0;
        //n should be kmap.variables -1 but for testing its 4
        for (int n = 6; n >= 0; n--)
        {
            if (n % 2 == 1)
            {
                coordinate = (((y >> (n / 2)) & 1) << n) + coordinate;
            }
            else
            {
                coordinate = (((x >> (n / 2)) & 1) << n) + coordinate;

            }
        }

        return coordinate;
    }
    
}