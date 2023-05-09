using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    //this script alligns the top left of the game object to 0,0

    public GameObject scroll;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("asdfl");

        RectTransform RT = scroll.GetComponent<RectTransform>();
        Vector3[] v = new Vector3[4];
        RT.GetWorldCorners(v);
        Debug.Log(v[3].x);
        Debug.Log(v[2].x);
        Debug.Log(v[1].x);
        Debug.Log(v[0].x);
        Debug.Log(v[3].y);
        Debug.Log(v[2].y);
        Debug.Log(v[1].y);
        Debug.Log(v[0].y);
        Debug.Log(v[3].z);
        Debug.Log(v[2].z);
        Debug.Log(v[1].z);
        Debug.Log(v[0].z);

        RT.SetInsetAndSizeFromParentEdge(new RectTransform.Edge(), 0, 1);
        scroll.transform.position = scroll.transform.position + new Vector3((v[1].x - v[0].x) / 2, 0);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
