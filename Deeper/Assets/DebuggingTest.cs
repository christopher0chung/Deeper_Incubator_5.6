using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class DebuggingTest : Deeper_Component {

    private Deeper_TaskManager myTM;

    private Vector3[] moveVector = new Vector3[2];

    private Rewired.Player[] players = new Rewired.Player[2];

    private Rewired.Player player0 { get { return ReInput.isReady ? ReInput.players.GetPlayer(0) : null; } }
    private Rewired.Player player1 { get { return ReInput.isReady ? ReInput.players.GetPlayer(1) : null; } }

    private Text myText;

    private void Awake()
    {
        base.Initialize( 5000);
    }

    private void Start()
    {
        myTM = GameObject.Find("Managers").GetComponent<Deeper_TaskManager>();
        players[0] = player0;
        players[1] = player1;
        myText = GameObject.Find("Text").GetComponent<Text>();
    }

    public override void EarlyUpdate()
    {
        base.EarlyUpdate();
        //Debug.Log("Early Update");
    }

    public override void NormUpdate()
    {
        base.NormUpdate();
        //Debug.Log("Normal Update");
    }

    public override void PostUpdate()
    {
        base.PostUpdate();
        //Debug.Log("Post Update");
    }

    //private int highlightedIndex;
    //private int indexRange;
    //private void ChangeHighlighted(bool upPosDownNeg)
    //{
    //    indexRange = GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems.Count - 1;
    //    if (upPosDownNeg)
    //        highlightedIndex++;
    //    else
    //        highlightedIndex--;

    //    if (highlightedIndex > indexRange)
    //        highlightedIndex = 0;
    //    if (highlightedIndex < 0)
    //        highlightedIndex = indexRange;

    //    Debug.Log(highlightedIndex);

    //    for (int i = 0; i <= indexRange; i++)
    //    {
    //        if (i == highlightedIndex)
    //            GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[i].itemState = MenuItemStates.Highlighted;
    //        else
    //            GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[i].itemState = MenuItemStates.Available;
    //        Debug.Log(i + " was set to " + GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[i].itemState);
    //    }
    //}

    //bool vis = true;

    //private void ChangeVis()
    //{
    //    vis = !vis;
    //    indexRange = GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems.Count - 1;

    //    for (int i = 0; i <= indexRange; i++)
    //    {
    //        if (vis)
    //            GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[i].itemState = MenuItemStates.Available;
    //        else
    //            GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[i].itemState = MenuItemStates.Hidden;
    //        //Debug.Log(i + " was set to " + GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[i].itemState);
    //    }
    //}

    private Deeper_RolloverInt testROI = new Deeper_RolloverInt(5, 5, 9);

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[0].itemState = MenuItemStates.Available;
        //    //GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Highlight(GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[0], TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[0].itemState = MenuItemStates.Highlighted;
        //    //GameObject.Find("Managers").GetComponent<Deeper_TaskManager>().AddTask(new Task_MenuAnimation_Unhighlight(GameObject.Find("Managers").GetComponent<Deeper_MenuObject>().testMG.menuItems[0], TaskCanBeInterrupted.Yes, TaskDoesInterrupt.Yes));
        //}

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    ChangeVis();
        //}

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //    ChangeHighlighted(false);
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //    ChangeHighlighted(true);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            testROI.intVal++;
        }

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

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlAssignment(ControllersEnum.C0, CharactersEnum.Doc));
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlAssignment(ControllersEnum.C1, CharactersEnum.Ops));
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlAssignment(ControllersEnum.C0, CharactersEnum.Ops));
            Deeper_EventManager.instance.Fire(new Deeper_Event_ControlAssignment(ControllersEnum.C1, CharactersEnum.Doc));
        }

        myText.text = "V:" + players[0].GetAxis("Menu Select Vertical") + "\nH:" + players[0].GetAxis("Menu Select Horizontal");

        //for (int i = 0; i <= 1; i++)
        //{
        //    Debug.Log("Player " + i + " Start is: " + players[i].GetButtonDown("Start"));
        //    Debug.Log("Player " + i + " Cancel is: " + players[i].GetButtonDown("Cancel"));
        //    Debug.Log("Player " + i + " Dialogue Skip is: " + players[i].GetButtonDown("Dialogue Skip"));

        //    Debug.Log("Player " + i + " Menu Select Vertical is: " + players[i].GetAxis("Menu Select Vertical"));
        //    Debug.Log("Player " + i + " Menu Select Horizontal: " + players[i].GetAxis("Menu Select Horizontal"));
        //    Debug.Log("Player " + i + " Menu Accept: " + players[i].GetButtonDown("Menu Accept"));

        //    Debug.Log("Player " + i + " Doc Swim Vertical is: " + players[i].GetAxis("Doc Swim Vertical"));
        //    Debug.Log("Player " + i + " Doc Swim Horizontal is: " + players[i].GetAxis("Doc Swim Horizontal"));
        //    Debug.Log("Player " + i + " Doc Light Vertical is: " + players[i].GetAxis("Doc Light Vertical"));
        //    Debug.Log("Player " + i + " Doc Light Horizontal is: " + players[i].GetAxis("Doc Light Horizontal"));
        //    Debug.Log("Player " + i + " Doc Action is: " + players[i].GetButtonDown("Doc Action"));
        //    Debug.Log("Player " + i + " Doc Sub Vertical is: " + players[i].GetAxis("Doc Sub Move Vertical"));
        //    Debug.Log("Player " + i + " Doc Sub Horizontal is: " + players[i].GetAxis("Doc Sub Move Horizontal"));
        //    Debug.Log("Player " + i + " Doc Sub Light Vertical is: " + players[i].GetAxis("Doc Sub Light Vertical"));
        //    Debug.Log("Player " + i + " Doc Sub Light Horizontal is: " + players[i].GetAxis("Doc Sub Light Horizontal"));
        //    Debug.Log("Player " + i + " Doc Sub Egress is: " + players[i].GetButtonDown("Doc Sub Egress"));
        //    Debug.Log("Player " + i + " Doc Dialogue Select is: " + players[i].GetButtonDown("Doc Dialogue Select"));

        //    Debug.Log("Player " + i + " Ops Swim Vertical is: " + players[i].GetAxis("Ops Swim Vertical"));
        //    Debug.Log("Player " + i + " Ops Swim Horizontal is: " + players[i].GetAxis("Ops Swim Horizontal"));
        //    Debug.Log("Player " + i + " Ops Light Vertical is: " + players[i].GetAxis("Ops Light Vertical"));
        //    Debug.Log("Player " + i + " Ops Light Horizontal is: " + players[i].GetAxis("Ops Light Horizontal"));
        //    Debug.Log("Player " + i + " Ops Action is: " + players[i].GetButtonDown("Ops Action"));
        //    Debug.Log("Player " + i + " Ops Sub Vertical is: " + players[i].GetAxis("Ops Sub Move Vertical"));
        //    Debug.Log("Player " + i + " Ops Sub Horizontal is: " + players[i].GetAxis("Ops Sub Move Horizontal"));
        //    Debug.Log("Player " + i + " Ops Sub Light Vertical is: " + players[i].GetAxis("Ops Sub Light Vertical"));
        //    Debug.Log("Player " + i + " Ops Sub Light Horizontal is: " + players[i].GetAxis("Ops Sub Light Horizontal"));
        //    Debug.Log("Player " + i + " Ops Sub Egress is: " + players[i].GetButtonDown("Ops Sub Egress"));
        //    Debug.Log("Player " + i + " Ops Dialogue Select is: " + players[i].GetButtonDown("Ops Dialogue Select"));

        //}
    }
}
