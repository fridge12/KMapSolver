using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{

    public GameObject scroll;
    public int xScrollMultiplyer;
    public int yScrollMultiplyer;

    //these many pixels of the game object will always be visible
    public float xVisible;
    public float yVisible;

    float xStart;
    float yStart;

    Vector2 size;

    RectTransform RT;

    // Start is called before the first frame update
    void Start()
    {
        RT = scroll.GetComponent<RectTransform>();
        size = RT.sizeDelta;
        Debug.Log("width "+size.x);
        Debug.Log("height "+size.y);

        xStart = scroll.transform.position.x;
        yStart = scroll.transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        if(RT.sizeDelta != size)
        {
            size = RT.sizeDelta;
            Debug.Log("width "+size.x);
            Debug.Log("height "+size.y);
        }

        Vector2 mouseScrollInput = Input.mouseScrollDelta;

        if(mouseScrollInput.y < 0)
        {
            if(yStart+size.y-yVisible > scroll.transform.position.y)
            {
                scroll.transform.position = scroll.transform.position + new Vector3(0, Input.mouseScrollDelta.y * -yScrollMultiplyer, 0);
            }
            Debug.Log(scroll.transform.position);
        }
        else if (mouseScrollInput.y > 0)
        {
            if (yStart  < scroll.transform.position.y)
            {
                scroll.transform.position = scroll.transform.position + new Vector3(0, Input.mouseScrollDelta.y * -yScrollMultiplyer, 0);
            }
            Debug.Log(scroll.transform.position);

        }



        if (mouseScrollInput.x < 0)
        {
            if (xStart + size.x - xVisible > scroll.transform.position.x)
            {
                scroll.transform.position = scroll.transform.position + new Vector3(Input.mouseScrollDelta.x * -xScrollMultiplyer, 0, 0);
            }
        }
        else if (mouseScrollInput.x > 0)
        {
            if (xStart < scroll.transform.position.x)
            {
                scroll.transform.position = scroll.transform.position + new Vector3(Input.mouseScrollDelta.x * -xScrollMultiplyer,0, 0);
            }
        }
        //Debug.Log(Input.mouseScrollDelta.x);
        //scroll.transform.position = scroll.transform.position + new Vector3(Input.mouseScrollDelta.x * -xScrollMultiplyer, Input.mouseScrollDelta.y * -yScrollMultiplyer, 0);

    }
}
