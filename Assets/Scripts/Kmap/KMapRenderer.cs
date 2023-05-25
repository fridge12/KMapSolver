using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//attach to parent object
public class KMapRenderer : MonoBehaviour
{
    //cell prefab
    public GameObject cell;

    //array containing visible kmap
    public GameObject[,] KMapArray;

    //parent object
    public GameObject parent;

    public static float parentStartY = 353 -280;
    public static float parentStartX = 0;

    public static float parentEndY = 353;
    public static float parentEndX = 500;

    //number of columns and rows
    public int columns;
    public int rows;

    //size of cell
    Vector2 cellSize;

    public static Vector2 cellVelocity = new Vector2(0.125f,0.125f);
    private void Start()
    {

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

    public void initialise()
    {
        cellSize = new Vector2(500f / (float)columns, 280f/ (float)rows);
        KMapArray = new GameObject[rows, columns]; 
    }

    public void cellSpawner(int x, int y)
    {

        GameObject ob = Instantiate(cell, new Vector3(0, 0, 0), Quaternion.identity);
        ob.transform.SetParent(parent.transform);
        ob.GetComponent<RectTransform>().sizeDelta = cellSize * (ScreenConstants.scale/ ScreenConstants.oldScale); 
        KMapArray[y, x] = ob;

        Debug.Log(transform.position.y+"should be 353");

        ob.transform.position = new Vector3((x * cellSize.x * ScreenConstants.scale ), (parentEndY * ScreenConstants.scale) - ((y+1) * cellSize.y * ScreenConstants.scale), 0);
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

        ob.GetComponentsInChildren<Text>()[1].text = coordinate+"";

    }

   /* private void OnRectTransformDimensionsChange()
    {
        RectTransform RT = GetComponent<RectTransform>();
        GridLayoutGroup GL = GetComponent<GridLayoutGroup>();
        GL.cellSize = new Vector2(RT.sizeDelta.x / columns, RT.sizeDelta.y / rows);
      
    }*/

    // Update is called once per frame
    int ctr = 0;
    void Update()
    {
        if(ctr%1000== 0)
        {
            Debug.Log(ctr);
            Debug.Log(parent.GetComponent<RectTransform>().sizeDelta);
            Debug.Log(ScreenConstants.oldScale / ScreenConstants.scale);
        }
        ctr++;
    }
}
