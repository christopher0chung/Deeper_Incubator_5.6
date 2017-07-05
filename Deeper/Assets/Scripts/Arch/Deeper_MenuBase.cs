using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Deeper_MenuItem
{
    public Deeper_MenuItem(string l)
    {
        visibleLabel = l;
        itemGameObject = new GameObject();
        itemTMP = itemGameObject.AddComponent<TMPro.TextMeshPro>();
        itemTMP.text = visibleLabel;

        itemState = MenuItemStates.Hidden;
    }

    #region Internal

    public GameObject itemGameObject;
    public TMPro.TextMeshPro itemTMP;
    public string visibleLabel;

    private MenuItemStates _itemState;
    public MenuItemStates itemState
    {
        get { return _itemState; }
        set
        {
            if (value != _itemState)
            {
                if (_itemState == MenuItemStates.Available)
                {
                    if (value == MenuItemStates.Highlighted)
                    {
                        //Task to highlight
                    }
                    if (value == MenuItemStates.Hidden)
                    {
                        //Task to hide
                    }
                }
                if (_itemState == MenuItemStates.Highlighted)
                {
                    if (value == MenuItemStates.Available)
                    {
                        //Task to move to avaiable idle state
                    }
                    if (value == MenuItemStates.Accessed)
                    {
                        //Task to move to title space
                    }
                }
                if (_itemState == MenuItemStates.Accessed)
                {
                    if (value == MenuItemStates.Highlighted)
                    {
                        //Task to move from title to item
                    }
                }
                if (_itemState == MenuItemStates.Hidden)
                {
                    if (value == MenuItemStates.Available)
                    {
                        //Task to go from hidden to visible
                    }
                }
                _itemState = value;
            }
        }
    }
    #endregion
}

public enum MenuItemStates { Available, Highlighted, Accessed, Hidden }

public class Deeper_MenuGroup
{
    public Deeper_MenuGroup pathUp;
    public List<Deeper_MenuGroup> menuItemsPath;

    private List<string> _menuLabels;
    public List<Deeper_MenuItem> menuItems;

    public Deeper_MenuGroup(Deeper_MenuGroup p, List<string> m, List<Deeper_MenuGroup> iP)
    {
        pathUp = p;
        menuItemsPath = iP;
        _menuLabels = m;

        for (int i = 0; i < _menuLabels.Count; i++)
        {
            menuItems.Add(new Deeper_MenuItem(_menuLabels[i]));
        }
    }
}

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
