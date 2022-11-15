using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;

    public void Return()
    {
        OnReturnRequest?.Invoke(this);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
