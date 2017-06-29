using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Deeper_Event
{
    public delegate void Handler(Deeper_Event e);
}

//--------------------------------------------
// Control Event Messages
//--------------------------------------------

public enum ControllersEnum { C0, C1 }
public enum CharactersEnum { Doc, Ops, DANI }
public class Deeper_Event_ControlAssignment: Deeper_Event
{
    public ControllersEnum controller;
    public CharactersEnum character;

    public Deeper_Event_ControlAssignment (ControllersEnum co, CharactersEnum ch)
    {
        controller = co;
        character = ch;
    }
}

public class Deeper_Event_ControlScheme: Deeper_Event
{
    public ControlStates cs;
    public Deeper_Event_ControlScheme (ControlStates state)
    {
        cs = state;
    }
}

//--------------------------------------------
// Level Event Messages
//--------------------------------------------

public enum Levels { Splash, OpeningCutscene }

public class Deeper_Event_LevelUnload : Deeper_Event { }

public class Deeper_Event_LevelLoad : Deeper_Event
{
    public Levels level;
    public Deeper_Event_LevelLoad(Levels l)
    {
        level = l;
    }
}
