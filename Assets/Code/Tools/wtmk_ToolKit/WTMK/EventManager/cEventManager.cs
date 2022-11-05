using System.Collections.Generic;

public sealed class EventManager
{
    private static readonly EventManager _instance = new EventManager();
    public delegate void EventCallback(string eventName, object eventData);

    public static EventManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private Dictionary<string, List<EventCallback>> _Callbacks = new Dictionary<string, List<EventCallback>>();

    public void RegisterEventCallback(string eventName, EventCallback cb)
    {
        if (!_Callbacks.ContainsKey(eventName))
        {
            _Callbacks[eventName] = new List<EventCallback>();
        }
        if (!_Callbacks[eventName].Contains(cb))
        {
            _Callbacks[eventName].Add(cb);
        }
    }

    public void UnregisterEventCallback(string eventName, EventCallback cb)
    {
        if (_Callbacks.ContainsKey(eventName))
        {
            if (_Callbacks[eventName].Contains(cb))
            {
                _Callbacks[eventName].Remove(cb);
            }
        }
    }

    public void FireEvent(string eventName, object eventData = null)
    {
        if (_Callbacks.ContainsKey(eventName))
        {
            List<EventCallback> fireList = new List<EventCallback>(_Callbacks[eventName]);

            foreach (EventCallback cb in fireList)
            {
                cb(eventName, eventData);
            }
        }
    }

}