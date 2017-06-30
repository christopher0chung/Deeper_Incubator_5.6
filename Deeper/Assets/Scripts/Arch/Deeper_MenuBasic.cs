using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_MenuItem : MonoBehaviour
{
    private bool _highlighted;
    public bool highlighted
    {
        get { return _highlighted; }
        set
        {
            if (value != _highlighted)
            {
                _highlighted = value;

            }
        }
    }

    private bool _active;
    public bool active;
}

public class Deeper_MenuLevel
{
    public Deeper_MenuLevel parent;
    public List<string> menuItems;

    public Deeper_MenuLevel (Deeper_MenuLevel p)
    {
        parent = p;
    }
}

public class Deeper_MenuBasic {


}
