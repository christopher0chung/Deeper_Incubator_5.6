using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_Component : MonoBehaviour
{
    //-----------------------------------------------------------------------------
    // 1000 - 
    // 2000 - 
    // 3000 - controls
    // 4000 - 
    // 5000 - player objects
    //-----------------------------------------------------------------------------

    [HideInInspector] public int priority;

    public virtual void Initialize(int p)
    {
        priority = p;
        Deeper_GameUpdateManager.instance.Subscribe(this);
        Deeper_EventManager.instance.Register<Deeper_Event_LevelUnload>(Unsub);
    }

    public virtual void Unsub (Deeper_Event e)
    {
        Deeper_GameUpdateManager.instance.Unsubscribe(this);
        Deeper_EventManager.instance.Unregister<Deeper_Event_LevelUnload>(Unsub);
    }

    public virtual void EarlyUpdate() { }
    public virtual void NormUpdate() { }
    public virtual void PostUpdate() { }

    public virtual void PhysUpdate() { }
}
