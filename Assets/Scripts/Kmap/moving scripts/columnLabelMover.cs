using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class columnLabelMover : MonoBehaviour
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
        updatePosition(); 
    }
    void updatePosition()
    {
        //if (KMapRenderer.cellVelocity.Equals(new Vector2(0, 0))) return;

        RT.localPosition = new Vector3(RT.localPosition.x + (KMapRenderer.cellVelocity.x), RT.localPosition.y , 0);



        float y = RT.localPosition.y;
        float x = RT.localPosition.x;
        bool positionUpdated = false;
      

        if (RT.localPosition.x > KMapRenderer.parentEndX+50)
        {
            x = (KMapRenderer.parentStartX - RT.sizeDelta.x) + (RT.localPosition.x - KMapRenderer.parentEndX);
            KMapRenderer.updateArrayColStart -= 1;
            positionUpdated = true;
        }
        if (RT.localPosition.x < (KMapRenderer.parentStartX - RT.sizeDelta.x)+50)
        {
            x = KMapRenderer.parentEndX + (RT.localPosition.x - (KMapRenderer.parentStartX - RT.sizeDelta.x));
            KMapRenderer.updateArrayColStart += 1;

            positionUpdated = true;
        }


        RT.localPosition = new Vector3(x, y, 0);

    }
}