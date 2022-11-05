using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keylogger 
{
    // Update is called once per frame
    public void OnUpdate()
    {
        System.Array values = System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in values)
        {
            if (Input.GetKeyDown(code)) { Debug.Log(System.Enum.GetName(typeof(KeyCode), code)); }
        }
    }
}
