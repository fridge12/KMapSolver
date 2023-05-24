using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//attach to parent object
public class KMapRenderer : MonoBehaviour
{

    public GameObject cell;

    public GameObject[,] KMapArray = new GameObject[8,8];

    public GameObject parent;

    public static int gridX;
    public static int gridY;


    Vector2 cellSize = new Vector2(500f / 8f, 280f / 8f);

    private void Start()
    {
        GameObject ob;
        //cell.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        for (int i = 0; i < 64; i++)
        {
            cellSpawner(i % 8, i / 8);
            Debug.Log(i % 8);
            Debug.Log(i / 8);
        }

        
    }

    public void cellSpawner(int x, int y)
    {

        GameObject ob = Instantiate(cell, new Vector3(0, 0, 0), Quaternion.identity);
        ob.transform.SetParent(parent.transform);
        ob.GetComponent<RectTransform>().sizeDelta = cellSize; 
        KMapArray[y, x] = ob;

        ob.transform.position = new Vector3((x * cellSize.x * ScreenConstants.scale ), (353 * ScreenConstants.scale) - ((y+1) * cellSize.y * ScreenConstants.scale), 0);
        int coordinate = 0;

        //n should be kmap.variables -1 but for testing its 4
        for (int n = 6 - 1; n >= 0; n--)
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

    private void OnRectTransformDimensionsChange()
    {
        RectTransform RT = GetComponent<RectTransform>();
        GridLayoutGroup GL = GetComponent<GridLayoutGroup>();
        GL.cellSize = new Vector2(RT.sizeDelta.x / 16, RT.sizeDelta.y / 8);
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
