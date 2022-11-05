using System;
using System.Collections;
using System.Collections.Generic;

public class Poolable : IPoolable
{
    public event Action<IPoolable> OnReturnRequest;

    public virtual void Return()
    {
        OnReturnRequest?.Invoke(this);
    }

    public virtual void SetActive(bool isActive)
    {
        //
    }
}
