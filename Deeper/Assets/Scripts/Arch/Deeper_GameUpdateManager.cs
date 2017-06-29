using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_GameUpdateManager {

    private static Deeper_GameUpdateManager _instance;
    public static Deeper_GameUpdateManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new Deeper_GameUpdateManager ();
            return _instance;
        }
    }

    private List<Deeper_Component> _subDGO = new List<Deeper_Component>();

    private bool _p;
    private bool _pause
    {
        get
        {
            return _p;
        }
        set
        {
            if (value != _p)
            {
                _p = value;
                if (_p)
                    Time.timeScale = 0;
                else
                {
                    Time.timeScale = 1;
                }
            }
        }
    }
    public void PauseGame(bool pauseTrueUnpauseFalse)
    {
        _pause = pauseTrueUnpauseFalse;
    }

    public bool PauseState()
    {
        return _pause;
    }
    
	public void RunUpdates () {
        Debug.Log(_subDGO.Count);

    	if (!_pause)
        {
            //Only runs if there are sub'd DGO
            if (_subDGO.Count > 0)
            {
                    for (int i = 0; i < _subDGO.Count; i++)
                {
                    _subDGO[i].EarlyUpdate();
                }
                for (int i = 0; i < _subDGO.Count; i++)
                {
                    _subDGO[i].NormUpdate();
                }
                for (int i = 0; i < _subDGO.Count; i++)
                {
                    _subDGO[i].PostUpdate();
                }
            }
        }
	}

    public void RunFixedUpdate()
    {
        if (!_pause)
        {
            //Only runs if there are sub'd DGO
            if (_subDGO.Count > 0)
                for (int i = 0; i < _subDGO.Count; i++)
                {
                    _subDGO[i].PhysUpdate();
                }
        }
    }

    public void Subscribe (Deeper_Component dGO)
    {
        if (_subDGO.Count == 0)
        {
            _subDGO.Add(dGO);
        }
        else
        {
            for (int i = 0; i < _subDGO.Count; i++)
            {
                if (dGO.priority < _subDGO[i].priority)
                {
                    _subDGO.Insert(i, dGO);
                    return;
                }
            }
        }
    }

    public void Unsubscribe (Deeper_Component dGO)
    {
        _subDGO.Remove(dGO);
    }
}
