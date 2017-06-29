using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Deeper_ControlRouter : MonoBehaviour {

    #region Internal Dictionaries

    private Dictionary<ControllersEnum, int> _controllerToInt;
    private Dictionary<ControllersEnum, CharactersEnum> _controllerToCharacter;
    private Dictionary<CharactersEnum, ControllersEnum> _characterToController;

    #endregion

    private void Start()
    {
        players[0] = player0;
        players[1] = player1;

        _controllerToInt.Add(ControllersEnum.C0, 0);
        _controllerToInt.Add(ControllersEnum.C1, 1);

        Deeper_EventManager.instance.Register<Deeper_Event_ControlAssignment>(ControlAssignment);
    }

    #region Control Assignments

    private ControlOrientation _aO;
    private ControlOrientation _activeOrientation
    {
        get
        {
            return _aO;
        }
        set
        {
            if (value != _aO)
            {
                _aO = value;
                if (_aO == ControlOrientation.c0Ops_c1Doc)
                {
                    _controllerToCharacter.Clear();
                    _controllerToCharacter.Add(ControllersEnum.C0, CharactersEnum.Ops);
                    _controllerToCharacter.Add(ControllersEnum.C1, CharactersEnum.Doc);

                    _characterToController.Clear();
                    _characterToController.Add(CharactersEnum.Ops, ControllersEnum.C0);
                    _characterToController.Add(CharactersEnum.Doc, ControllersEnum.C1);
                }

                if (_aO == ControlOrientation.c0Doc_c1Ops)
                {
                    _controllerToCharacter.Clear();
                    _controllerToCharacter.Add(ControllersEnum.C0, CharactersEnum.Doc);
                    _controllerToCharacter.Add(ControllersEnum.C1, CharactersEnum.Ops);

                    _characterToController.Clear();
                    _characterToController.Add(CharactersEnum.Ops, ControllersEnum.C1);
                    _characterToController.Add(CharactersEnum.Doc, ControllersEnum.C0);
                }
            }
        }
    }


    private void ControlAssignment (Deeper_Event e)
    {
        Deeper_Event_ControlAssignment c = (Deeper_Event_ControlAssignment)e;

        VoteAssignment(c.controller, c.character);
    }

    private CharactersEnum[] votes = new CharactersEnum[2];

    private void VoteAssignment (ControllersEnum voteCo, CharactersEnum voteCh)
    {
        if (voteCo == ControllersEnum.C0)
            votes[0] = voteCh;
        else
            votes[1] = voteCh;

        if (votes[0] == CharactersEnum.Ops && votes[1] == CharactersEnum.Doc)
            _activeOrientation = ControlOrientation.c0Ops_c1Doc;
        if (votes[0] == CharactersEnum.Doc && votes[1] == CharactersEnum.Ops)
            _activeOrientation = ControlOrientation.c0Doc_c1Ops;
    }

    #endregion

    private Rewired.Player[] players = new Rewired.Player[2];

    private Rewired.Player player0 { get { return ReInput.isReady ? ReInput.players.GetPlayer(0) : null; } }
    private Rewired.Player player1 { get { return ReInput.isReady ? ReInput.players.GetPlayer(1) : null; } }

    private void SetControls (ControlStates cS)
    {
        // If the game interrupts controls, then all maps will be disabled
        if (cS == ControlStates.Interrupted)
        {
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    players[i].controllers.maps.SetMapsEnabled(false, j);
                }
            }
        }
        // Menu applies to both characters
        else if (cS == ControlStates.Menu)
        {
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    players[i].controllers.maps.SetMapsEnabled(false, j);
                }
                players[i].controllers.maps.SetMapsEnabled(true, 1);
            }
        }

        else
        {
            ControllersEnum opsController;
            _characterToController.TryGetValue(CharactersEnum.Ops, out opsController);
            int opsInt;
            _controllerToInt.TryGetValue(opsController, out opsInt);

            ControllersEnum docController;
            _characterToController.TryGetValue(CharactersEnum.Doc, out docController);
            int docInt;
            _controllerToInt.TryGetValue(opsController, out docInt);

            #region Ops

            if (cS == ControlStates.Ops_Inside)
            {
                for (int j = 0; j <= 4; j++)
                {
                    players[opsInt].controllers.maps.SetMapsEnabled(false, j);
                }
                players[opsInt].controllers.maps.SetMapsEnabled(true, 0);
                players[opsInt].controllers.maps.SetMapsEnabled(true, 5);
            }

            if (cS == ControlStates.Ops_Outside)
            {
                for (int j = 0; j <= 4; j++)
                {
                    players[opsInt].controllers.maps.SetMapsEnabled(false, j);
                }
                players[opsInt].controllers.maps.SetMapsEnabled(true, 0);
                players[opsInt].controllers.maps.SetMapsEnabled(true, 3);
            }

            if (cS == ControlStates.Ops_OOC)
            {
                for (int j = 0; j <= 4; j++)
                {
                    players[opsInt].controllers.maps.SetMapsEnabled(false, j);
                }
            }

            #endregion

            #region Doc

            if (cS == ControlStates.Doc_Inside)
            {
                for (int j = 0; j <= 4; j++)
                {
                    players[docInt].controllers.maps.SetMapsEnabled(false, j);
                }
                players[docInt].controllers.maps.SetMapsEnabled(true, 0);
                players[docInt].controllers.maps.SetMapsEnabled(true, 4);
            }

            if (cS == ControlStates.Doc_Outside)
            {
                for (int j = 0; j <= 4; j++)
                {
                    players[docInt].controllers.maps.SetMapsEnabled(false, j);
                }
                players[docInt].controllers.maps.SetMapsEnabled(true, 0);
                players[docInt].controllers.maps.SetMapsEnabled(true, 2);
            }

            if (cS == ControlStates.Doc_OOC)
            {
                for (int j = 0; j <= 4; j++)
                {
                    players[docInt].controllers.maps.SetMapsEnabled(false, j);
                }
            }

            #endregion
        }



    }


}

public enum ControlOrientation { c0Doc_c1Ops, c0Ops_c1Doc }
public enum ControlStates { Interrupted, Ops_Outside, Ops_Inside, Ops_OOC, Doc_Outside, Doc_Inside, Doc_OOC, Menu}
