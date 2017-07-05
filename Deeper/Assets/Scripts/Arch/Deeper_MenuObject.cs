using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_MenuObject : MonoBehaviour
{
    private Deeper_MenuGroup testMG;

    private List<string> testItems;
    private List<Deeper_MenuGroup> testPaths;

    private void Awake()
    {
        testItems.Add("Number 1");
        testItems.Add("Number 2");

        testPaths.Add(null);
        testPaths.Add(null);
    }

    private void Start()
    {
        testMG = new Deeper_MenuGroup(null, testItems, testPaths);
        testMG.menuItems[0].itemGameObject.transform.position = Vector3.zero;
        testMG.menuItems[1].itemGameObject.transform.position = Vector3.zero + Vector3.up;
    }

}