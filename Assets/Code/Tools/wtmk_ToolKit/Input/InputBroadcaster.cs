using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBroadcaster : MonoBehaviour
{
    private EventManager _EventManager = EventManager.Instance;
    private StartScreenEvent _StartScreenEvent = new StartScreenEvent();

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _EventManager.FireEvent(_StartScreenEvent.ShowHelpScreen);
        }
    }
}
