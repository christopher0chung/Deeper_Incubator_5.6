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

        itemTMP.fontSize = 20;
        itemTMP.alignment = TextAlignmentOptions.Center;

        if (GameObject.FindGameObjectWithTag("MainCamera").transform != null)
            itemGameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);

        //itemState = MenuItemStates.Hidden;
    }

    #region Internal

    public GameObject itemGameObject;
    public TMPro.TextMeshPro itemTMP;
    public string visibleLabel;

    //private MenuItemStates _itemState;
    //public MenuItemStates itemState
    //{
    //    get { return _itemState; }
    //    set
    //    {
    //        Debug.Log("state was " + _itemState + ", but is now " + value);
    //        if (value != _itemState)
    //        {
    //            if (_itemState == MenuItemStates.Available)
    //            {
    //                if (value == MenuItemStates.Highlighted)
    //                {
    //                    GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Highlight(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
    //                }
    //                if (value == MenuItemStates.Hidden)
    //                {
    //                    GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Invisible(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
    //                }
    //            }
    //            if (_itemState == MenuItemStates.Highlighted)
    //            {
    //                if (value == MenuItemStates.Available)
    //                {
    //                    GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Unhighlight(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
    //                }
    //                if (value == MenuItemStates.Accessed)
    //                {
    //                    //Task to move to title space
    //                }
    //            }
    //            if (_itemState == MenuItemStates.Accessed)
    //            {
    //                if (value == MenuItemStates.Highlighted)
    //                {
    //                    //Task to move from title to item
    //                }
    //            }
    //            if (_itemState == MenuItemStates.Hidden)
    //            {
    //                if (value == MenuItemStates.Available)
    //                {
    //                    GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Visible(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
    //                }
    //            }
    //            _itemState = value;
    //        }
    //    }
    //}

    private bool _hidden;
    public bool hidden
    {
        get { return _hidden; }
        set
        {
            if (value != _hidden)
            {
                _hidden = value;
                if (_hidden)
                    GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Invisible(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
                else
                    GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Visible(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
            }
        }
    }

    private bool _highlighted;
    public bool highlighted
    {
        get { return _highlighted; }
        set
        {
            if (value != _highlighted)
            {
                _highlighted = value;
                if (_highlighted)
                    GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Highlight(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
                else
                    GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Unhighlight(this, TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
            }
        }
    }

    private MenuItemState3 _state;
    public MenuItemState3 state
    {
        get { return _state; }
        set
        {
            if (value != _state)
            {
                _state = value;
                if (_state == MenuItemState3.Active)
                    Debug.Log("Active");
                else if (_state == MenuItemState3.Inactive)
                    Debug.Log("Inactive");
                else
                    Debug.Log("Open");
            }
        }
    }

    #endregion
}
public enum MenuItemState3 { Active, Open, Inactive }
public enum MenuItemStates { Available, Highlighted, Accessed, Hidden }

public class Deeper_MenuGroup
{
    public Deeper_MenuGroup pathUp;
    public List<Deeper_MenuGroup> menuItemsPath = new List<Deeper_MenuGroup>();

    private List<string> _menuLabels = new List<string>();
    public List<Deeper_MenuItem> menuItems = new List<Deeper_MenuItem>();

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