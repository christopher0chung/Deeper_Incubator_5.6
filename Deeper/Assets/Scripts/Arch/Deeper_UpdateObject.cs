using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_UpdateObject : MonoBehaviour {

    [HideInInspector] public bool markForDeath;

    private void Awake()
    {
        DoublesCheck();
    }

    // Update is called once per frame
    void Update ()
    {
        Deeper_GameUpdateManager.instance.RunUpdates();

        if (markForDeath)
            Destroy(this.gameObject);
	}

    private void FixedUpdate()
    {
        Deeper_GameUpdateManager.instance.RunFixedUpdate();
    }

    private void DoublesCheck()
    {
        if (GameObject.FindObjectsOfType<Deeper_UpdateObject>().Length > 1)
        {
            Deeper_UpdateObject[] DMArray = GameObject.FindObjectsOfType<Deeper_UpdateObject>();
            for (int i = 1; i < DMArray.Length; i++)
            {
                DMArray[i].markForDeath = true;
            }
        }
    }
}
