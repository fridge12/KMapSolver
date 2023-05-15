using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{

    public GameObject scroll;
    public int xScrollMultiplyer;
    public int yScrollMultiplyer;

    //these many pixels of the game object will always be visible 
    //these pixels are canvas pixels and thus need to be scaled
    public float xVisible;
    public float yVisible;

    //starting coordinates for the gameObject
    //we are assuming this is as far up as the gameobject can scroll
    public float xStart;
    public float yStart;

    bool mouseInBounds = false;

    Vector2 size;

    RectTransform RT;

    // Start is called before the first frame update
    void Start()
    {
        RT = scroll.GetComponent<RectTransform>();
        size = RT.sizeDelta;
        Debug.Log("width "+size.x);
        Debug.Log("height " + size.y);
    }

    private void OnRectTransformDimensionsChange()
    {
        //everytime the dimensions of a scrollable thing change resetting their position
        RectTransform RT = this.GetComponent<RectTransform>();
        RT.position = new Vector3(xStart * ScreenConstants.scale, yStart * ScreenConstants.scale, 0);
    }

    public void MouseEnter()
    {
        mouseInBounds = true;
    }


    public void MouseExit()
    {
        mouseInBounds = false;
    }


    // Update is called once per frame
    void Update()
    {
        //if the size of the gameobject changes updating the size
        if(RT.sizeDelta != size)
        {
            size = RT.sizeDelta;
            Debug.Log("width "+size.x);
            Debug.Log("height "+size.y);
        }

        //taking input from the mouse
        Vector2 mouseScrollInput = Input.mouseScrollDelta;


        //Debug.Log(Input.mousePosition.x+" "+ Input.mousePosition.y);

        if (!mouseInBounds) return;
        

            //the size of objects within the canvas need to be adjusted as their values are not scaled
            //their world coordinates however do not need to be scaled

            //if scrolling down
            if (mouseScrollInput.y < 0)
            {
                //making sure that it doesn't keep scrolling down, and that atleast yVisible amount of pixels are visible
                //if there is room to move down, move down
                if (ScreenConstants.scale * (yStart + size.y - yVisible) > scroll.transform.position.y)
                {
                    //scrolling down
                    scroll.transform.position = scroll.transform.position + new Vector3(0, ScreenConstants.scale * (Input.mouseScrollDelta.y * -yScrollMultiplyer), 0);
                }
                if (Kmap.toPrint) Debug.Log("down" + scroll.transform.position);
            }
            //if scrolling up
            else if (mouseScrollInput.y > 0)
            {
                //if we can scroll up
                if (ScreenConstants.scale * yStart < scroll.transform.position.y)
                {
                    //scroll up
                    scroll.transform.position = scroll.transform.position + new Vector3(0, ScreenConstants.scale * (Input.mouseScrollDelta.y * -yScrollMultiplyer), 0);
                }
                if (Kmap.toPrint) Debug.Log("up" + scroll.transform.position);

            }



            if (mouseScrollInput.x < 0)
            {
                if (ScreenConstants.scale * (xStart + size.x - xVisible) > scroll.transform.position.x)
                {
                    scroll.transform.position = scroll.transform.position + new Vector3(ScreenConstants.scale * (Input.mouseScrollDelta.x * -xScrollMultiplyer), 0, 0);
                }
            }
            else if (mouseScrollInput.x > 0)
            {
                if (ScreenConstants.scale * xStart < scroll.transform.position.x)
                {
                    scroll.transform.position = scroll.transform.position + new Vector3(ScreenConstants.scale * (Input.mouseScrollDelta.x * -xScrollMultiplyer), 0, 0);
                }
            }
            //Debug.Log(Input.mouseScrollDelta.x);
            //scroll.transform.position = scroll.transform.position + new Vector3(Input.mouseScrollDelta.x * -xScrollMultiplyer, Input.mouseScrollDelta.y * -yScrollMultiplyer, 0);
        
    }
}
