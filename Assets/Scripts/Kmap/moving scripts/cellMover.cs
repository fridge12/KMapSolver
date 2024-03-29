using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cellMover : MonoBehaviour
{
    RectTransform RT;
    static int ctr = 0;

    // Start is called before the first frame update
    void Start()
    {
        RT = GetComponent<RectTransform>();
        number = GetComponentsInChildren<Text>()[0];
        coordinateText = GetComponentsInChildren<Text>()[1];
        a = KMapRenderer.calculateXY(int.Parse(coordinateText.text));
    }

    // Update is called once per frame
    void Update()
    {
        if (KMapRenderer.cellVelocity.x != 0 || KMapRenderer.cellVelocity.y != 0)
        {
            updatePosition();
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        ctr = 1000;
        Debug.Log(ScreenConstants.scale + "cell mover");
    }



    Text number;
    Text coordinateText;
    int[] a = {0,0 };
    Group g = new Group(0, 0);
    GroupComparer gc = new GroupComparer();

    void updatePosition()
    {
        //if (KMapRenderer.cellVelocity.Equals(new Vector2(0, 0))) return;

        if (!KMapRenderer.canMoveUp && KMapRenderer.cellVelocity.y > 0) KMapRenderer.cellVelocity.y = 0;
        if (!KMapRenderer.canMoveDown && KMapRenderer.cellVelocity.y < 0) KMapRenderer.cellVelocity.y = 0;
        if (!KMapRenderer.canMoveLeft && KMapRenderer.cellVelocity.x < 0) KMapRenderer.cellVelocity.x = 0;
        if (!KMapRenderer.canMoveRight && KMapRenderer.cellVelocity.x > 0) KMapRenderer.cellVelocity.x = 0;

         RT.localPosition = new Vector3(RT.localPosition.x + (KMapRenderer.cellVelocity.x ), RT.localPosition.y + (KMapRenderer.cellVelocity.y ), 0);

       

        float y = RT.localPosition.y ;
        float x = RT.localPosition.x  ;
        bool positionUpdated = false;
        if (RT.localPosition.y > (KMapRenderer.parentStartY + (RT.sizeDelta.y * 0.33)) )
        {
            y =  KMapRenderer.parentEndY - (RT.sizeDelta.y * 0.66f) +(RT.localPosition.y - (KMapRenderer.parentStartY + (RT.sizeDelta.y * 0.33f))) ;
            //KMapRenderer.updateArrayRowStart = 1;
            a[1] += System.Math.Min(KMapRenderer.maxRows+1, KMapRenderer.rows);

            positionUpdated = true;
        }
        if (RT.localPosition.y < (KMapRenderer.parentEndY - (RT.sizeDelta.y * 0.66)) )
        {
            y = (KMapRenderer.parentStartY + (RT.sizeDelta.y * 0.33f))+(RT.localPosition.y - (KMapRenderer.parentEndY - (RT.sizeDelta.y * 0.66f)));
            //KMapRenderer.updateArrayRowStart = -1;
            a[1] -= System.Math.Min(KMapRenderer.maxRows+1, KMapRenderer.rows);

            positionUpdated = true;
        }


        if (RT.localPosition.x > KMapRenderer.parentEndX )
        {
            x = (KMapRenderer.parentStartX - RT.sizeDelta.x)+(RT.localPosition.x - KMapRenderer.parentEndX);
            //KMapRenderer.updateArrayColStart = -1;
            a[0] -= System.Math.Min(KMapRenderer.maxColumns+1, KMapRenderer.columns); 
            positionUpdated = true;
        }
        if (RT.localPosition.x < (KMapRenderer.parentStartX - RT.sizeDelta.x) )
        {
            x = KMapRenderer.parentEndX +(RT.localPosition.x - (KMapRenderer.parentStartX - RT.sizeDelta.x));
            //KMapRenderer.updateArrayColStart = 1;
            a[0] += System.Math.Min(KMapRenderer.maxColumns+1, KMapRenderer.columns);

            positionUpdated = true;
        }

        uint coordinate = KMapRenderer.calculateCoordinate(a[0], a[1]);
        g.coordinate = coordinate;

        int pos = Kmap.groupsList.BinarySearch(g, gc);
        if ( pos >= 0)
        {
            number.GetComponent<Outline>().effectColor = Kmap.groupsList[pos].borderColour;
            if (Kmap.SOP_POS == "SOP") number.text = "1";
            else number.text = "0";
        }
        else
            number.text = "";

        coordinateText.text = coordinate + "";


        if (a[0] < 0 || a[1] < 0) { 
            coordinateText.gameObject.SetActive(false);
            number.gameObject.SetActive(false);
        }
        else if( a[1]> System.Math.Pow(2,Kmap.variables/2)-1 || a[0] > System.Math.Pow(2,Kmap.variables- (Kmap.variables / 2))-1)
        {
            coordinateText.gameObject.SetActive(false);
            number.gameObject.SetActive(false);
        }

        else { 
            coordinateText.gameObject.SetActive(true);
            number.gameObject.SetActive(true);
        }

        RT.localPosition = new Vector3(x , y, 0);
        
    }
}
