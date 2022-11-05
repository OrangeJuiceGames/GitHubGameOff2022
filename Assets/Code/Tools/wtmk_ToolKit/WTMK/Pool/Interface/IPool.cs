using System.Collections;
using System.Collections.Generic;

public interface IPool
{
    IPoolable GetPoolable();
    void PlaceInQueue(IPoolable t);
}
