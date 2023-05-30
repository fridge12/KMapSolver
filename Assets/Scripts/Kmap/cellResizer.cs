using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cellResizer : MonoBehaviour
{
    public static readonly bool toPrint = false;

    public GameObject coordinate;
    public GameObject number;

    private void OnRectTransformDimensionsChange()
    {
        RectTransform RT = GetComponent<RectTransform>();

        if (toPrint) Debug.Log(RT.sizeDelta+"prefab size");

        number.GetComponent<RectTransform>().sizeDelta = new Vector2(RT.sizeDelta.x , RT.sizeDelta.y * 0.66f);
        coordinate.GetComponent<RectTransform>().sizeDelta = new Vector2(RT.sizeDelta.x , RT.sizeDelta.y *0.33f);
    }
}
