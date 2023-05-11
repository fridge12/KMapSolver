using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSpawner : MonoBehaviour
{

    public GameObject GroupContainerScroll;

    public GameObject groupPrefab;

    bool spawned = false;
    private void Update()
    {
        if (!spawned)
        {
            spawnGroup("this is a test aaaa", "testing");
            spawnGroup("this is a test aaaaaaaaaa", "testing");
            spawnGroup("this is a test aaaa", "testing");
            spawnGroup("this is a test aaa", "testing");
            spawnGroup("this is a test", "testing");
            spawnGroup("this is a test", "testing");
            spawnGroup("this is a test", "testing");
            spawnGroup("this is a test", "testing");
            spawnGroup("this is a test", "testing");
        }
        spawned = true;
    }
    public void spawnGroup(string expression, string terms)
    {
        GameObject ob = Instantiate(groupPrefab, GroupContainerScroll.transform);
        UnityEngine.UI.Text[] c =  ob.GetComponentsInChildren<UnityEngine.UI.Text>();

        c[0].text = expression; 
        c[1].text = terms;


        RectTransform[] RT = ob.GetComponentsInChildren<RectTransform>();
        Debug.Log(System.Math.Max(RT[0].sizeDelta.y, RT[1].sizeDelta.y));

    }

}
