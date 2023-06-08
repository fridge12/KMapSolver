using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (RT.localPosition.y > (KMapRenderer.parentStartY + RT.sizeDelta.y - 28))
        {
            y = KMapRenderer.parentEndY -28  + (RT.localPosition.y - (KMapRenderer.parentStartY) - (RT.sizeDelta.y - 28));
            KMapRenderer.arrayRowStart += 1;
            GetComponent<Text>().text = KMapRenderer.ConstantVariables(KMapRenderer.calculateCoordinate(0, KMapRenderer.arrayRowStart + System.Math.Min(KMapRenderer.rows,KMapRenderer.maxRows+1)) & 0xAAAAAAA);

            positionUpdated = true;
        }
        if (RT.localPosition.y < (KMapRenderer.parentEndY -28))
        {
            y = (KMapRenderer.parentStartY  + (RT.localPosition.y - KMapRenderer.parentEndY+28) + (RT.sizeDelta.y - 28));
            KMapRenderer.arrayRowStart -= 1;
            GetComponent<Text>().text = KMapRenderer.ConstantVariables(KMapRenderer.calculateCoordinate(0, KMapRenderer.arrayRowStart) & 0xAAAAAAA);

            positionUpdated = true;
        }
        RT.localPosition = new Vector3(x, y, 0);

    }
}
