using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems.UIBehaviour;

public class ScreenConstants : MonoBehaviour
{

    public GameObject canvas;

    //current scale of the canvas
    public static float scale=1;
    //previous scale of the canvas
    public static float oldScale=1;

    // Start is called before the first frame update
    void Start()
    {
        if(Kmap.toPrint) Debug.Log("scale factor"+canvas.GetComponent<Canvas>().scaleFactor);
        scale = canvas.GetComponent<Canvas>().scaleFactor;
        oldScale = scale;
    }

    protected void OnRectTransformDimensionsChange()
    {
        //checking to see if the scale factor has actually changed because sometimes this method is called even when scale factor has not changed
        if(scale != canvas.GetComponent<Canvas>().scaleFactor)
        {
            if (Kmap.toPrint) Debug.Log(scale + " scale has changed " + canvas.GetComponent<Canvas>().scaleFactor);

            oldScale = scale;
            scale = canvas.GetComponent<Canvas>().scaleFactor;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
