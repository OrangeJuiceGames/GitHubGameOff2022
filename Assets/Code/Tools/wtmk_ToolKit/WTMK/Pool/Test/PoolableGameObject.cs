using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableGameObject : MonoBehaviour , IPoolable
{
    public event Action<IPoolable> OnReturnRequest;

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        OnReturnRequest?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(false);
    }

    public void Return()
    {
        OnReturnRequest?.Invoke((IPoolable)this);
    }

}
