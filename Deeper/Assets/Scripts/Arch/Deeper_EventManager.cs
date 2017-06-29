using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//---------------------------------------
// Event Manager
//---------------------------------------

public class Deeper_EventManager
{
    //---------------------
    // Creates singleton for ease of access
    //---------------------

    static private Deeper_EventManager _instance;
    static public Deeper_EventManager instance
    {
        get
        {
            if (_instance == null)
                return _instance = new Deeper_EventManager();
            else
                return _instance;
        }
    }

    //---------------------
    // Storage of Events
    //---------------------

    private Dictionary<Type, Deeper_Event.Handler> registeredHandlers = new Dictionary<Type, Deeper_Event.Handler>();

    //---------------------
    // Register and Unregister
    //---------------------

    public void Register<T>(Deeper_Event.Handler handler) where T : Deeper_Event
    {
        Type type = typeof(T);
        if (registeredHandlers.ContainsKey(type))
        {
            registeredHandlers[type] += handler;
        }
        else
        {
            registeredHandlers[type] = handler;
        }
    }

    public void Unregister<T>(Deeper_Event.Handler handler) where T : Deeper_Event
    {
        Type type = typeof(T);
        Deeper_Event.Handler handlers;
        if (registeredHandlers.TryGetValue(type, out handlers))
        {
            handlers -= handler;
            if (handlers == null)
            {
                registeredHandlers.Remove(type);
            }
            else
            {
                registeredHandlers[type] = handlers;
            }
        }
    }

    //---------------------
    // Call event
    //---------------------

    public void Fire(Deeper_Event e)
    {
        Type type = e.GetType();
        Deeper_Event.Handler handlers;
        if (registeredHandlers.TryGetValue(type, out handlers))
        {
            handlers(e);
        }
    }
}
