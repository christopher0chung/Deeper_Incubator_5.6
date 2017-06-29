using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class DebuggingTest : Deeper_Component {

    private Deeper_TaskManager myTM;

    private Vector3[] moveVector = new Vector3[2];

    private Rewired.Player[] players = new Rewired.Player[2];

    private Rewired.Player player0 { get { return ReInput.isReady ? ReInput.players.GetPlayer(0) : null; } }
    private Rewired.Player player1 { get { return ReInput.isReady ? ReInput.players.GetPlayer(1) : null; } }

    private void Awake()
    {
        base.Initialize( 5000);
    }

    private void Start()
    {
        myTM = GameObject.Find("Managers").GetComponent<Deeper_TaskManager>();
        players[0] = player0;
        players[1] = player1;
    }

    public override void EarlyUpdate()
    {
        base.EarlyUpdate();
        Debug.Log("Early Update");
    }

    public override void NormUpdate()
    {
        base.NormUpdate();
        Debug.Log("Normal Update");
    }

    public override void PostUpdate()
    {
        base.PostUpdate();
        Debug.Log("Post Update");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Deeper_ServicesLocator.instance.PauseToggle();
            Deeper_ServicesLocator.Interpolate(.1f, Deeper_ServicesLocator.Functions.CircularEaseIn);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Task t = new TestTask();
            t.Then(new TestTask()).Then(new TestTask()).Then(new TestTask());
            myTM.AddTask(t);
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Inside));
            if (Input.GetKeyDown(KeyCode.Alpha9))
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_Outside));
            if (Input.GetKeyDown(KeyCode.Alpha0))
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Doc_OOC));
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Inside));
            if (Input.GetKeyDown(KeyCode.Alpha9))
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_Outside));
            if (Input.GetKeyDown(KeyCode.Alpha0))
                Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Ops_OOC));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Menu));
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlScheme(ControlStates.Interrupted));

        for (int i = 0; i <= 1; i++)
        {
            Debug.Log("Player " + i + " Doc Swim Up is: " + players[i].GetAxis("Doc Swim Up"));
        }
    }
}
