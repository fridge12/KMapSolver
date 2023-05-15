using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CopyToClipBoard : MonoBehaviour
{


    public GameObject Copy;

    public void onClick()
    {
        string s = Copy.GetComponent<Text>().text;

        GUIUtility.systemCopyBuffer = s;
    }
}
