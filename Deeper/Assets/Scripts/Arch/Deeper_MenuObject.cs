using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Deeper_MenuObject : MonoBehaviour
{
    public Deeper_MenuObject parentPath;

    [Header("The number of names must equal the number of paths")]
    public List<string> testItemsNames = new List<string>();
    public List<Deeper_MenuObject> testPaths = new List<Deeper_MenuObject>();
    private List<Deeper_MenuItem> _testItems;

    private Transform mCam;

    private Vector3 posActive = Vector3.zero;
    private Vector3 posDeactive = Vector3.right * 10;
    private Vector3 posOpen = Vector3.right * -10;

    private FSM<Deeper_MenuObject> _fsm;
    private Deeper_RolloverInt _highlightedItemIndex;
    public int highlightedItemIndex
    {
        get
        {
            if (_highlightedItemIndex == null)
            {
                _highlightedItemIndex = new Deeper_RolloverInt(0, 0, testItemsNames.Count - 1);
                _testItems[_highlightedItemIndex.intVal].highlighted = true;
            }
            return _highlightedItemIndex.intVal;
        }
        set
        {
            if (_highlightedItemIndex == null) _highlightedItemIndex = new Deeper_RolloverInt(0, 0, testItemsNames.Count - 1);

            _testItems[_highlightedItemIndex.intVal].highlighted = false;
            _highlightedItemIndex.intVal = value;
            _testItems[_highlightedItemIndex.intVal].highlighted = true;
        }
    }

#region Intialization
    private void Awake()
    {
        Debug.Assert(testItemsNames.Count == testPaths.Count, "Deeper_MenuObject on " + this.gameObject.name + ": Items and paths mismatch!");
        _fsm = new FSM<Deeper_MenuObject>(this);
    }

    private void Start()
    {
        RewiredRefs();
        _fsm.TransitionTo<Active>();
    }

    private void OnEnable()
    {
        CamReference();
        if (_testItems == null)
        {
            PlaceElements();
        }
    }

    private void PlaceElements()
    {
        transform.parent = mCam;
        transform.position = mCam.position;

        float spread = (testItemsNames.Count - 1) * 3;
        float upperHeight = spread / 2;

        _testItems = new List<Deeper_MenuItem>();

        for (int i = 0; i < testItemsNames.Count; i++)
        {
            _testItems.Add(new Deeper_MenuItem(testItemsNames[i]));
            _testItems[i].itemGameObject.transform.position = mCam.position + mCam.forward * 20 + mCam.up * (upperHeight - 3 * i);
            _testItems[i].itemGameObject.transform.SetParent(this.transform);
        }

        highlightedItemIndex = 0;
    }

    private void CamReference()
    {
        if (mCam == null)
            mCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private Rewired.Player[] players = new Rewired.Player[2];

    private Rewired.Player player0 { get { return ReInput.isReady ? ReInput.players.GetPlayer(0) : null; } }
    private Rewired.Player player1 { get { return ReInput.isReady ? ReInput.players.GetPlayer(1) : null; } }

    private void RewiredRefs()
    {
        players[0] = player0;
        players[1] = player1;
    }

    #endregion

    public void Update()
    {
        _fsm.Update();
        CamReference();
    }

    public void ExternalActivate()
    {
        this.enabled = true;
        _fsm.TransitionTo<Activate>();
    }

#region States for FSM

    private class State_Base : FSM<Deeper_MenuObject>.State
    {

    }

    private class Active : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("The ActiveState is running in " + Context.gameObject.name);
        }

        private bool[] _iFlag = new bool[2];

        public override void Update()
        {
            for (int i = 0; i <= 1; i++)
            {
                if (Mathf.Abs(Context.players[i].GetAxis("Menu Select Vertical")) > .75f && !_iFlag[i])
                {
                    //Debug.Log("Before the set, _iFlag[" + i + "] is set to " + _iFlag[i]);
                    _iFlag[i] = true;
                    //Debug.Log("After the set, _iFlag[" + i + "] is set to " + _iFlag[i]);

                    if (Context.players[i].GetAxis("Menu Select Vertical") > 0)
                    {
                        Context.highlightedItemIndex--;
                    }
                    else if (Context.players[i].GetAxis("Menu Select Vertical") < 0)
                    {
                        Context.highlightedItemIndex++;
                    }
                }
                else if (Mathf.Abs(Context.players[i].GetAxis("Menu Select Vertical")) <= .15f)
                {
                    _iFlag[i] = false;
                }

                if (Context.players[i].GetButtonDown("Menu Accept"))
                {
                    if (Context.testPaths[Context.highlightedItemIndex] != null)
                    {
                        Context.testPaths[Context.highlightedItemIndex].ExternalActivate();
                        TransitionTo<Open>();
                    }
                }

                if (Context.players[i].GetButtonDown("Cancel"))
                {
                    if (Context.parentPath != null)
                    {
                        Context.parentPath.ExternalActivate();
                        TransitionTo<Deactivate>();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Context.highlightedItemIndex++;
            }
        }
    }

    private class Activate : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("The ActivateState is running in " + Context.gameObject.name);

            foreach(Deeper_MenuItem m in Context._testItems)
            {
                m.hidden = false;
            }
        }

        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context.posActive, .08f);
            if (Vector3.Distance(Context.transform.localPosition, Context.posActive) < .01f)
            {
                TransitionTo<Active>();
            }
        }

        public override void OnExit()
        {
            Context.transform.localPosition = Context.posActive;
        }
    }

    private class Deactivate : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("The DectivateState is running in " + Context.gameObject.name);

            foreach (Deeper_MenuItem m in Context._testItems)
            {
                m.hidden = true;
            }
        }

        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context.posDeactive, .08f);
            if (Vector3.Distance(Context.transform.localPosition, Context.posDeactive) < .01f)
            {
                TransitionTo<State_Base>();
            }
        }

        public override void OnExit()
        {
            Context.transform.localPosition = Context.posDeactive;
            Context.enabled = false;
        }
    }

    private class Open : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("The DectivateState is running in " + Context.gameObject.name);

            foreach (Deeper_MenuItem m in Context._testItems)
            {
                m.hidden = true;
            }
        }

        public override void Update()
        {
            Context.transform.localPosition = Vector3.Lerp(Context.transform.localPosition, Context.posOpen, .08f);
            if (Vector3.Distance(Context.transform.localPosition, Context.posOpen) < .01f)
            {
                TransitionTo<State_Base>();
            }
        }

        public override void OnExit()
        {
            Context.transform.localPosition = Context.posOpen;
            Context.enabled = false;
        }
    }

    #endregion
}