using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Deeper_RewiredIntputTest : MonoBehaviour {

    public GameObject child;

    private int playerId0 = 0;

    private Vector3[] moveVector = new Vector3[2];

    private Rewired.Player[] players = new Rewired.Player[2];

    private Rewired.Player player0 { get { return ReInput.isReady ? ReInput.players.GetPlayer(playerId0) : null; } }
    private Rewired.Player player1 { get { return ReInput.isReady ? ReInput.players.GetPlayer(1) : null; } }

    private void Start()
    {
        players[0] = player0;
        players[1] = player1;
    }

    void Update()
    {
        //Debug.Log("Update for test");
        if (!ReInput.isReady) return;
        //Debug.Log("ReInput is ready");
        if (player0 == null) return;
        //Debug.Log("Player is not null");

        if (Input.GetKeyDown(KeyCode.Return))
        {
            for (int i = 0; i <= 1; i++)
            {
                players[i].controllers.maps.SetMapsEnabled(false, 0);
                players[i].controllers.maps.SetMapsEnabled(true, 1);
            }
        }

        for (int i = 0; i <= 1; i++)
        {
            GetInput(i);
            ProcessInput(i);
        }
        
    }

    private void GetInput(int i)
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        moveVector[i].x = players[i].GetAxis("Move Horizontal"); // get input by name or action id
        moveVector[i].y = players[i].GetAxis("Move Vertical");
    }

    private void ProcessInput(int i)
    {
        if (i== 0)
        {
            // Process movement
            if (moveVector[i].x != 0.0f || moveVector[i].y != 0.0f)
            {
                transform.position += moveVector[i];
                //Debug.Log("Input");
                return;
            }
            //Debug.Log("No Input");
        }
        else
        {
            if (moveVector[i].x != 0.0f || moveVector[i].y != 0.0f)
            {
                child.transform.position += moveVector[i];
            }
        }
    }
}
