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
    }

    #region Internal

    public GameObject itemGameObject;
    public TMPro.TextMeshPro itemTMP;
    public string visibleLabel;

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

    #endregion
}

//public class Deeper_MenuGroup
//{
//    public Deeper_MenuObject pathUp;
//    public List<Deeper_MenuObject> menuItemsPath = new List<Deeper_MenuObject>();

//    private List<string> _menuLabels = new List<string>();
//    public List<Deeper_MenuItem> menuItems = new List<Deeper_MenuItem>();

//    public Deeper_MenuGroup(Deeper_MenuObject p, List<string> m, List<Deeper_MenuObject> iP)
//    {
//        pathUp = p;
//        menuItemsPath = iP;
//        _menuLabels = m;

//        for (int i = 0; i < _menuLabels.Count; i++)
//        {
//            menuItems.Add(new Deeper_MenuItem(_menuLabels[i]));
//        }
//    }
//}