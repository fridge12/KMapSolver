using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSpawner : MonoBehaviour
{

    public GameObject GroupContainerScroll;

    public GameObject groupPrefab;

    bool spawned = false;


    public void spawnGroupsList()
    {
        //Debug.Log("in spawn groups list");

        //deleting all child objects
        clearGroups();

        Group g;
        //index of g
        int index = 0;

        int size = Kmap.groupsList.Count;
        //iterating through the list
        for (index = 0; index < size; index++)
        {
            g = Kmap.groupsList[index];
            //if it isn't part of any bigger groups spawning it in
            if(g.biggerGroups == 0) spawnGroup(g.terms(),g.reducedExpression(Kmap.SOP_POS));
        }
    }

    //spawns groupCell prefab with terms and expression in its child prefabs
    public void spawnGroup(string terms, string expression)
    {
        //spawning new group prefab
        GameObject ob = Instantiate(groupPrefab, GroupContainerScroll.transform);
        //getting its children's text components (the expression and term prefab text elements)
        UnityEngine.UI.Text[] c =  ob.GetComponentsInChildren<UnityEngine.UI.Text>();
        //adding the terms text to the terms prefab
        c[0].text = terms; 
        //adding the expression text to the expression prefab
        c[1].text = expression;


        //the height of whichever prefab is greatest
        float height = System.Math.Max(c[0].preferredHeight, c[1].preferredHeight);

        //rect transform of it and its children
        RectTransform[] RT = ob.GetComponentsInChildren<RectTransform>();

        //Debug.Log(RT[1].sizeDelta.x + " " + RT[2].sizeDelta.x);
        //ob 
        RT[0].sizeDelta = new Vector2(RT[0].sizeDelta.x,height );
        //terms prefab
        RT[1].sizeDelta = new Vector2(RT[1].sizeDelta.x, height);
        //expressionn prefab
        RT[2].sizeDelta = new Vector2(RT[2].sizeDelta.x, height);

    }

    //deleting all the groupPrefabs from GroupContainerScroll
    public void clearGroups()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
