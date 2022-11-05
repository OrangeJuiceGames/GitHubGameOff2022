using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingeltonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get => _Instance;
    }

    protected static T _Instance;

    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        if (_Instance == null)
        {
            _Instance = (T)FindObjectOfType(typeof(T));
            if (_Instance == null)
            {
                _Instance = (new GameObject(typeof(T).Name)).AddComponent<T>();
            }
            DontDestroyOnLoad(_Instance.gameObject);
        }
    }
}
