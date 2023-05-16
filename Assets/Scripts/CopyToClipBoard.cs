using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CopyToClipBoard : MonoBehaviour
{


    public GameObject Copy;

    public void onClick()
    {
        //text to be copied
        string copyText = Copy.GetComponent<Text>().text;
        //copying text to clipboard
        GUIUtility.systemCopyBuffer = copyText;
    }
}
