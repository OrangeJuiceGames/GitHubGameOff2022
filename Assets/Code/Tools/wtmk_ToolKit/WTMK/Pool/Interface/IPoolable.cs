using System.Collections;
using System.Collections.Generic;
using System;

public interface IPoolable 
{
    public event Action<IPoolable> OnReturnRequest;
    public void SetActive(bool isActive);
    public void Return();
}
