using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deeper_SceneManager {

    private Dictionary<Levels, string> _levelDict = new Dictionary<Levels, string>();

    private Deeper_SceneManager _instance;
    public Deeper_SceneManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new Deeper_SceneManager();
            return _instance;
        }
    }

    public void LoadLevel (Levels lvl)
    {
        Deeper_EventManager.instance.Fire(new Deeper_Event_LevelUnload());
        if (lvl == Levels.Splash)
        {

        }
        Deeper_EventManager.instance.Fire(new Deeper_Event_LevelLoad(lvl));
    }
}
