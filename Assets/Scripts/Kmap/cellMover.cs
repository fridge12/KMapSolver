using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cellMover : MonoBehaviour
{
    RectTransform RT;

    // Start is called before the first frame update
    void Start()
    {
        RT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position= new Vector3( transform.position.x + (KMapRenderer.cellVelocity.x * ScreenConstants.scale), transform.position.y+ (KMapRenderer.cellVelocity.y * ScreenConstants.scale), 0 ) ;

        float y = transform.position.y;
        float x=transform.position.x;
        bool updatePosition = false; 
        if (transform.position.y > KMapRenderer.parentEndY * ScreenConstants.scale + (RT.sizeDelta.y * 0.33) * ScreenConstants.scale)
        {
            y = KMapRenderer.parentStartY * ScreenConstants.scale - (RT.sizeDelta.y * 0.33f) * ScreenConstants.scale;
            updatePosition = true;
        }
        if(transform.position.y< KMapRenderer.parentStartY* ScreenConstants.scale - (RT.sizeDelta.y * 0.33) * ScreenConstants.scale)
        {
            y = KMapRenderer.parentEndY * ScreenConstants.scale + (RT.sizeDelta.y * 0.33f) * ScreenConstants.scale;
            updatePosition = true;
        }


        if (transform.position.x > KMapRenderer.parentEndX * ScreenConstants.scale)
        {
            x = (KMapRenderer.parentStartX -RT.sizeDelta.x) * ScreenConstants.scale;
            updatePosition = true;
        }
        if( transform.position.x < (KMapRenderer.parentStartX- RT.sizeDelta.x) * ScreenConstants.scale)
        {
            x = KMapRenderer.parentEndX * ScreenConstants.scale;
            updatePosition = true;
        }

        if(updatePosition)
        {
            transform.position = new Vector3(x, y, 0);
        }
    }
}
