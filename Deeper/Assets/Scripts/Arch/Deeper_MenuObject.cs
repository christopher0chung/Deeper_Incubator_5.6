using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_MenuObject : MonoBehaviour
{
    public Deeper_MenuGroup testMG;

    private List<string> testItems = new List<string>();
    private List<Deeper_MenuGroup> testPaths = new List<Deeper_MenuGroup>();

    private Transform mCam;

    private void Awake()
    {
        testItems.Add("Number 1");
        testItems.Add("Number 2");
        testItems.Add("Number 1");
        testItems.Add("Number 2");
        testItems.Add("Number 1");
        testItems.Add("Number 2");

        testPaths.Add(null);
        testPaths.Add(null);
        testPaths.Add(null);
        testPaths.Add(null);
        testPaths.Add(null);
        testPaths.Add(null);
    }

    private void Start()
    {
        testMG = new Deeper_MenuGroup(null, testItems, testPaths);
        CamReference();
        PlaceElements();
        HighlightFirst();
    }

    private void Update()
    {
        CamReference();

    }

    private void PlaceElements()
    {
        float spread = (testItems.Count - 1) * 3;
        float upperHeight = spread / 2;

        for (int i = 0; i < testItems.Count; i++)
        {
            testMG.menuItems[i].itemGameObject.transform.position = mCam.position + mCam.forward * 20 + mCam.up * (upperHeight - 3 * i);
        }
    }

    private void CamReference()
    {
        if (mCam == null)
            mCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void HighlightFirst()
    {
        for (int i = 0; i < testItems.Count; i++)
            testMG.menuItems[i].itemState = MenuItemStates.Available;
        testMG.menuItems[0].itemState = MenuItemStates.Highlighted;
    }
}