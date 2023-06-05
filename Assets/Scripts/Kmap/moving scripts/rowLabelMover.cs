using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rowLabelMover : MonoBehaviour
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

        RT.localPosition = new Vector3(RT.localPosition.x, RT.localPosition.y + (KMapRenderer.cellVelocity.y), 0);



        float y = RT.localPosition.y;
        float x = RT.localPosition.x;
        bool positionUpdated = false;
        if (RT.localPosition.y > (KMapRenderer.parentStartY + (RT.sizeDelta.y * 0.33)))
        {
            y = KMapRenderer.parentEndY - (RT.sizeDelta.y * 0.66f) + (RT.localPosition.y - (KMapRenderer.parentStartY + (RT.sizeDelta.y * 0.33f)));
            KMapRenderer.updateArrayRowStart += 1;
            positionUpdated = true;
        }
        if (RT.localPosition.y < (KMapRenderer.parentEndY - (RT.sizeDelta.y * 0.66)))
        {
            y = (KMapRenderer.parentStartY + (RT.sizeDelta.y * 0.33f)) + (RT.localPosition.y - (KMapRenderer.parentEndY - (RT.sizeDelta.y * 0.66f)));
            KMapRenderer.updateArrayRowStart -= 1;
            positionUpdated = true;
        }
        RT.localPosition = new Vector3(x, y, 0);

    }
}
